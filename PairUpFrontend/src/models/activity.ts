import {TopLevelCategory} from "./top-level-category";
import {SubLevelCategory} from "./sub-level-category";

export interface Activity {
    id: string;
    name: string;
    image: string;
    description: string;
    url: string;
    price: string;
    age: string | null;
    duration: string | null;
    fullAddress: string;
    latitude: number;
    longitude: number;
    topLevelCategory: TopLevelCategory;
    subLevelCategory: SubLevelCategory[];
}