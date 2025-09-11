import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from './AuthProvider';

const LoginCallback = () => {
    const navigate = useNavigate();
    const { userManager } = useAuth();

    useEffect(() => {
        const processLoginCallback = async () => {
            try {
                await userManager.signinRedirectCallback();
                navigate('/');
            } catch (error) {
                console.error('Error during login callback:', error);
                navigate('/');
            }
        };

        processLoginCallback();
    }, [navigate, userManager]);

    return <div>Loading...</div>;
};

export default LoginCallback;