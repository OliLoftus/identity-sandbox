import { createContext, useContext, useEffect, useMemo, useState, type ReactNode } from 'react';
import { User, UserManager } from 'oidc-client-ts';
import { authSettings } from './auth-settings';

// 1. Define the shape of Auth context
interface AuthContextType {
    user: User | null;
    userManager: UserManager;
    login: () => void;
    logout: () => void;
    isAuthenticated: boolean;
    isLoading: boolean;
}

// 2. Create the context with a default undefined value
const AuthContext = createContext<AuthContextType | undefined>(undefined);

// 3. Create the AuthProvider component
export const AuthProvider = ({ children }: { children: ReactNode }) => {
    // Create a stable UserManager instance that doesn't change on re-renders
    const userManager = useMemo(() => new UserManager(authSettings), []);
    const [user, setUser] = useState<User | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        // This function will be called when the component mounts
        const loadUser = async () => {
            try {
                // Check if a user session exists in storage
                const userFromStorage = await userManager.getUser();
                setUser(userFromStorage);
            } catch (error) {
                console.error("AuthProvider: Error loading user", error);
            } finally {
                setIsLoading(false);
            }
        };

        // Subscribe to events from the userManager
        const onUserLoaded = (loadedUser: User) => setUser(loadedUser);
        const onUserUnloaded = () => setUser(null);

        userManager.events.addUserLoaded(onUserLoaded);
        userManager.events.addUserUnloaded(onUserUnloaded);

        // Initial load
        loadUser();

        // Cleanup function to remove event listeners when the component unmounts
        return () => {
            userManager.events.removeUserLoaded(onUserLoaded);
            userManager.events.removeUserUnloaded(onUserUnloaded);
        };
    }, [userManager]);

    const login = () => {
        // Start the login process
        userManager.signinRedirect();
    };

    const logout = () => {
        // Start the logout process
        userManager.signoutRedirect();
    };

    const value = {
        user,
        userManager,
        login,
        logout,
        isAuthenticated: !!user,
        isLoading,
    };

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

// 4. Create a custom hook for easy access to the context
export const useAuth = () => {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};