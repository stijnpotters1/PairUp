import axiosInstance from './request-instance';
import {
    API_ENDPOINT_AUTH_LOGIN,
    API_ENDPOINT_AUTH_REGISTER
} from '../constants/api';
import { decodeToken, saveToken, removeToken, isTokenExpired, getToken } from '../utils/jwt-helper';
import { UserResponse } from '../models/user';
import {getUserAsync} from './user-service';
import {AuthenticationResponse, LoginRequest, RegisterRequest} from '../models/authentication';
import {AxiosError} from "axios";
import {ErrorResponse} from "../models/error";

export const loginAsync = async (loginRequest: LoginRequest, rememberMe: boolean): Promise<UserResponse | null> => {
    try {
        const response = await axiosInstance.post<AuthenticationResponse>(API_ENDPOINT_AUTH_LOGIN, loginRequest);

        const { token } = response.data;

        if (token) {
            saveToken(token, rememberMe);
            const decodedToken = decodeToken(token);
            return await getUserAsync(decodedToken.userId);
        }

        return null;
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};

export const registerAsync = async (registerRequest: RegisterRequest, rememberMe: boolean): Promise<UserResponse | null> => {
    try {
        const response = await axiosInstance.post<AuthenticationResponse>(API_ENDPOINT_AUTH_REGISTER, registerRequest);

        const { token } = response.data;

        if (token) {
            saveToken(token, rememberMe);
            const decodedToken = decodeToken(token);
            return await getUserAsync(decodedToken.userId);
        }

        return null;
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};

export const logout = (): void => {
    removeToken();
    window.location.href = '/trips';
};

export const isAuthenticated = (): boolean => {
    const token = getToken();
    return token !== null && !isTokenExpired(token);
};
