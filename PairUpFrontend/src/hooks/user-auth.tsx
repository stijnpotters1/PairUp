import React, { createContext, useContext, useEffect, useState } from 'react';
import { UserResponse } from '../models/user';
import { getToken, decodeToken, isTokenExpired } from '../utils/jwt-helper';
import {getUserAsync} from "../services/user-service";
import {toast} from "react-toastify";

interface UserContextType {
    getUser: () => UserResponse | null;
    setUser: (user: UserResponse) => void;
    isAdmin: boolean;
    loading: boolean;
}

const UserContext = createContext<UserContextType | undefined>(undefined);

export const UserProvider: React.FC = ({ children }) => {
    const [user, setUserContext] = useState<UserResponse | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const fetchUser = async () => {
            const token = getToken();
            if (token && !isTokenExpired(token)) {
                const decoded = decodeToken(token);
                try {
                    const userData = await getUserAsync(decoded.userId);
                    setUserContext(userData);
                } catch (error) {
                    toast.error("Failed to fetch user:", error);
                    setUserContext(null);
                }
            }
            setLoading(false);
        };

        fetchUser();
    }, []);

    const isAdmin = user?.role.name === 'Admin';

    const setUser = (user : UserResponse) => {
        setUserContext(user);
    }

    const getUser = () => {
        return user;
    }

    return (
        <UserContext.Provider value={{ getUser, setUser, isAdmin, loading }}>
            {children}
        </UserContext.Provider>
    );
};

export const useUser = () => {
    const context = useContext(UserContext);
    if (context === undefined) {
        throw new Error('useUser must be used within a UserProvider');
    }
    return context;
};
