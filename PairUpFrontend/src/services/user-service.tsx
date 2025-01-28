import axiosInstance from './request-instance';
import { CUSTOM_API_ENDPOINT_USER } from '../constants/api';
import {UserRequest, UserResponse} from "../models/user";
import {AxiosError} from "axios";
import {ErrorResponse} from "../models/error";

export const getUsersAsync = async () : Promise<UserResponse[]> => {
    try {
        const response = await axiosInstance.get(CUSTOM_API_ENDPOINT_USER);
        return response.data as UserResponse[];
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};

export const getUserAsync = async (userId: string) : Promise<UserResponse> => {
    try {
        const response = await axiosInstance.get(`${CUSTOM_API_ENDPOINT_USER}/${userId}`);
        return response.data as UserResponse;
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};

export const postUserAsync = async (user: UserRequest) : Promise<UserResponse> => {
    try {
        const response = await axiosInstance.post(CUSTOM_API_ENDPOINT_USER, user);
        return response.data as UserResponse;
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};

export const updateUserAsync = async (userId: string, updatedUser: UserRequest) : Promise<UserResponse> => {
    try {
        const response = await axiosInstance.put(`${CUSTOM_API_ENDPOINT_USER}/${userId}`, updatedUser);
        return response.data as UserResponse;
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};

export const deleteUserAsync = async (userId: string) : Promise<boolean> => {
    try {
        const response = await axiosInstance.delete(`${CUSTOM_API_ENDPOINT_USER}/${userId}`);
        return response.data as boolean;
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};
