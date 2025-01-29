// API Base url
export const API_BASE_URL = 'https://localhost:7247/api';

// Authentication
export const API_ENDPOINT_AUTH_REGISTER = `${API_BASE_URL}/Authentication/register`;
export const API_ENDPOINT_AUTH_LOGIN = `${API_BASE_URL}/Authentication/login`;

// Users
export const API_ENDPOINT_USER = `${API_BASE_URL}/User`;

// Roles
export const API_ENDPOINT_ROLE = `${API_BASE_URL}/Role`;

// Activities
export const API_ENDPOINT_ACTIVITIES = `${API_BASE_URL}/Activity`;
export const API_ENDPOINT_ACTIVITIES_LIKE = `${API_ENDPOINT_ACTIVITIES}/like`;
export const API_ENDPOINT_ACTIVITIES_UNLIKE = `${API_ENDPOINT_ACTIVITIES}/unlike`;

// Sub Categories
export const API_ENDPOINT_SUB_LEVEL_CATEGORIES = `${API_BASE_URL}/SubLevelCategory`;