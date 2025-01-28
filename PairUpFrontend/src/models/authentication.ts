export interface RegisterRequest {
    firstName: string;
    lastName: string
    email: string;
    password: string;
    confirmPassword: string;
}
export interface LoginRequest {
    email: string;
    password: string;
}

export interface AuthenticationResponse {
    token: string;
}