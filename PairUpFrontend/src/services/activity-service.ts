import {ErrorResponse} from "../models/error";
import axiosInstance from "./request-instance";
import {CUSTOM_API_ENDPOINT_ACTIVITIES} from "../constants/api";
import {AxiosError} from "axios";
import {PagedActivityRequest, PagedActivityResponse} from "../models/paged-activity";

export const getPagedActivities = async (pagedActivityRequest: PagedActivityRequest): Promise<PagedActivityResponse[]> => {
    try {
        const response = await axiosInstance.post(`${CUSTOM_API_ENDPOINT_ACTIVITIES}`, pagedActivityRequest);
        return response.data as PagedActivityResponse[];
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};