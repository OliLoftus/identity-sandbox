import { useEffect } from 'react';
import { UserManager } from 'oidc-client-ts';
import { useNavigate } from 'react-router-dom';
import { authSettings } from './auth-settings';

const LoginCallback = () => {
    const navigate = useNavigate();

    useEffect(() => {
        const processLoginCallback = async () => {
            const userManager = new UserManager(authSettings);
            try {
                await userManager.signinRedirectCallback();
                navigate('/'); // Redirect to home page after successful login
            } catch (error) {
                console.error('Error during login callback:', error);
                navigate('/'); // Or to an error page
            }
        };

        processLoginCallback();
    }, [navigate]);

    return <div>Loading...</div>;
};

export default LoginCallback;