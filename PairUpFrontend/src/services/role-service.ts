import axiosInstance from "./request-instance";
import {AxiosError} from "axios";
import {ErrorResponse} from "../models/error";
import {Role} from "../models/role";
import {API_ENDPOINT_ROLE} from "../constants/api";

export const getRolesAsync = async () : Promise<Role[]> => {
    try {
        const response = await axiosInstance.get(API_ENDPOINT_ROLE);
        return response.data as Role[];
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};