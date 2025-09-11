import { createContext, useContext, useEffect, useState, type ReactNode } from 'react';
import { User, UserManager } from 'oidc-client-ts';
import { authSettings } from './auth-settings';

// 1. Define the shape of our Auth context
interface AuthContextType {
    user: User | null;
    userManager: UserManager;
    login: () => void;
    logout: () => void;
    isAuthenticated: boolean;
}

// 2. Create the context with a default undefined value
const AuthContext = createContext<AuthContextType | undefined>(undefined);

// 3. Create the AuthProvider component
export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const userManager = new UserManager(authSettings);
    const [user, setUser] = useState<User | null>(null);

    useEffect(() => {
        // Check if a user is already signed in
        const getUser = async () => {
            const user = await userManager.getUser();
            setUser(user);
        };
        getUser();
    }, [userManager]);

    const login = () => {
        userManager.signinRedirect();
    };

    const logout = () => {
        userManager.signoutRedirect();
    };

    const value = {
        user,
        userManager,
        login,
        logout,
        isAuthenticated: !!user,
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