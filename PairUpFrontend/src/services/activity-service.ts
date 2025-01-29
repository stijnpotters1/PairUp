import {ErrorResponse} from "../models/error";
import axiosInstance from "./request-instance";
import {
    API_ENDPOINT_ACTIVITIES,
    API_ENDPOINT_ACTIVITIES_LIKE, API_ENDPOINT_ACTIVITIES_UNLIKE,
} from "../constants/api";
import {AxiosError} from "axios";
import {PagedActivityRequest, PagedActivityResponse} from "../models/paged-activity";
import {Activity} from "../models/activity";

export const getPagedActivities = async (pagedActivityRequest: PagedActivityRequest): Promise<PagedActivityResponse[]> => {
    try {
        const response = await axiosInstance.post(`${API_ENDPOINT_ACTIVITIES}`, pagedActivityRequest);
        return response.data as PagedActivityResponse[];
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};

export const likeActivityAsync = async (userId: string, activity: Activity): Promise<Activity> => {
    try {
        const response = await axiosInstance.post(`${API_ENDPOINT_ACTIVITIES_LIKE}/${userId}`, activity);
        return response.data as Activity;
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};

export const unlikeActivityAsync = async (userId: string, activityId: string): Promise<boolean> => {
    try {
        const response = await axiosInstance.delete(`${API_ENDPOINT_ACTIVITIES_UNLIKE}/${userId}/activityId/${activityId}`);
        return response.data as boolean;
    } catch (error) {
        const axiosError = error as AxiosError<ErrorResponse>;
        throw new Error(axiosError?.response?.data?.message!);
    }
};