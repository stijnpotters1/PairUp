import {Activity} from "./activity";

export interface PagedActivityRequest {
    topLevelCategories: number[];
    subLevelCategories: string[];
    latitude: number;
    longitude: number;
    radius: number;
    pageNumber: number;
    pageSize: number;
}

export interface PagedActivityResponse {
    items: Activity[];
    totalCount: number;
    pageNumber: number;
    pageSize: number;
}