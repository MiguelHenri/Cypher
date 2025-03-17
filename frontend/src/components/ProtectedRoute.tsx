import { Navigate } from "react-router-dom";
import { ReactNode, useEffect, useState } from "react";
import axios from "axios";
import { CircularProgress } from "@mui/material";
import { useAuth } from "../contexts/useAuth";

type ProtectedRouteProps = {
    children: ReactNode;
};

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
    const { token, clearAuth } = useAuth();
    const [isValid, setIsValid] = useState<boolean | null>(null);

    useEffect(() => {
        if (!token) return;

        axios.get('/api/users/auth', {
            headers: {
                Authorization: `Bearer ${token}`,
            }
        })
            .then(res => {
                if (res.status === 204) {
                    setIsValid(true);
                } else {
                    setIsValid(false);
                }
            })
            .catch(err => {
                console.error('Error checking token validity.', err);
                setIsValid(false);
                clearAuth();
            })
    }, [token, clearAuth]);

    // Loading...
    if (isValid === null && token) return <CircularProgress/>

    if (isValid) return children;

    return <Navigate to='/' replace />
}

export default ProtectedRoute;