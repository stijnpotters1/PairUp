import {Role} from "./role";
import {Activity} from "./activity";

export interface UserRequest {
    firstName: string;
    lastName: string
    email: string;
    password: string;
    roleId: string;
}

export interface UserResponse {
    id: string;
    firstName: string;
    lastName: string
    email: string;
    role: Role;
    likedActivities: Activity[];
}