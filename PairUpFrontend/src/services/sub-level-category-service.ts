import {ErrorResponse} from "../models/error";
import axiosInstance from "./request-instance";
import {API_ENDPOINT_SUB_LEVEL_CATEGORIES} from "../constants/api";
import {AxiosError} from "axios";
import {SubLevelCategory} from "../models/sub-level-category";

export const getSubLevelCategories = async (): Promise<SubLevelCategory[]> => {
    try {
        const response = await axiosInstance.get(`${API_ENDPOINT_SUB_LEVEL_CATEGORIES}`);
        return response.data as SubLevelCategory[];
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};