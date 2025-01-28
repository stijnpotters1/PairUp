import {CUSTOM_API_BASE_URL} from "../constants/api";
import axios from "axios";
import {getToken} from "../utils/jwt-helper";

const axiosInstance = axios.create({
    baseURL: CUSTOM_API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

axiosInstance.interceptors.request.use(
    (config) => {
        const token = getToken();
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

export default axiosInstance;