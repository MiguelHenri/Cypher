import { createContext, useEffect, useState, ReactNode } from "react";

interface AuthContextType {
    userName: string;
    token: string;
    clearAuth: () => void;
    saveLogin: (userName: string, token: string) => void;
    setToken: React.Dispatch<React.SetStateAction<string>>;
}

// Create context with a default value
const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
    const [userName, setUserName] = useState<string>('');
    const [token, setToken] = useState<string>('');
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const storedUserName = localStorage.getItem('userName') || '';
        const storedToken = localStorage.getItem('token') || '';
        
        setUserName(storedUserName);
        setToken(storedToken);
        setLoading(false);
    }, []);

    const clearAuth = (): void => {
        setUserName('');
        localStorage.removeItem('userName');
        setToken('');
        localStorage.removeItem('token');
    };

    const saveLogin = (userName: string, token: string): void => {
        setUserName(userName);
        localStorage.setItem('userName', userName);
        setToken(token);
        localStorage.setItem('token', token);
    };

    if (loading) return null; // Waiting for data

    return (
        <AuthContext.Provider value={{ userName, clearAuth, saveLogin, token, setToken }}>
        {children}
        </AuthContext.Provider>
    );
}

export default AuthContext;