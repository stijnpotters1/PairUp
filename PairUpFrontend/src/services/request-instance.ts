import {CUSTOM_API_BASE_URL} from "../constants/api";
import axios from "axios";

export const axiosInstance = axios.create({
    baseURL: CUSTOM_API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});