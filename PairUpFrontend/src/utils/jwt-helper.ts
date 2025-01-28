import { jwtDecode } from 'jwt-decode';

interface DecodedToken {
    exp: number;
    userId: string;
}

export const decodeToken = (token: string): DecodedToken => {
    return jwtDecode<DecodedToken>(token);
};

export const isTokenExpired = (token: string): boolean => {
    const decoded = decodeToken(token);
    const currentTime = Math.floor(Date.now() / 1000);
    return decoded.exp < currentTime;
};

export const getToken = (): string | null => {
    return localStorage.getItem('token') || sessionStorage.getItem('token');
};

export const saveToken = (token: string, rememberMe: boolean): void => {
    if (rememberMe) {
        localStorage.setItem('token', token);
    } else {
        sessionStorage.setItem('token', token);
    }
};

export const removeToken = (): void => {
    localStorage.removeItem('token');
    sessionStorage.removeItem('token');
};
