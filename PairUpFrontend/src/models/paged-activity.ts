import {Activity} from "./activity";
import {TopLevelCategory} from "./top-level-category";

export interface PagedActivityRequest {
    topLevelCategories: TopLevelCategory[];
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