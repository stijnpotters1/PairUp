import React, { useState, useEffect } from "react";
import { useQuery } from "react-query";
import Container from "../components/reusable-layout-components/container";
import Row from "../components/reusable-layout-components/grid-row";
import Column from "../components/reusable-layout-components/grid-column";
import FilterContainer from "./filter-container";
import CardContainer from "./card-container";
import { TopLevelCategory } from "../models/top-level-category";
import { capitalize } from "../utils/capitalize";
import Spinner from "../components/spinner/spinner";
import { getSubLevelCategories } from "../services/sub-level-category-service";
import { getPagedActivities } from "../services/activity-service";
import { PagedActivityRequest } from "../models/paged-activity";

const PageContainer: React.FunctionComponent = () => {
    const DEFAULT_RADIUS = 30;
    const DEFAULT_PAGE_NUMBER = 1;
    const DEFAULT_PAGE_SIZE = 20;

    const [radius, setRadius] = useState<number>(DEFAULT_RADIUS);
    const [topLevelCategories, setTopLevelCategories] = useState<number[]>([]);
    const [subLevelCategories, setSubLevelCategories] = useState<string[]>([]);

    const [appliedRadius, setAppliedRadius] = useState<number>(DEFAULT_RADIUS);
    const [appliedTopLevelCategories, setAppliedTopLevelCategories] = useState<number[]>([]);
    const [appliedSubLevelCategories, setAppliedSubLevelCategories] = useState<string[]>([]);

    const [pageNumber, setPageNumber] = useState<number>(DEFAULT_PAGE_NUMBER);
    const [pageSize, setPageSize] = useState<number>(DEFAULT_PAGE_SIZE);

    const [location, setLocation] = useState<{ latitude: number; longitude: number } | null>(null);

    const { data: fetchedSubLevelCategories, isLoading: isSubLevelLoading } = useQuery(
        "subLevelCategories",
        getSubLevelCategories
    );

    const { data, error, isLoading, isFetching, refetch } = useQuery(
        ['activities', appliedRadius, appliedTopLevelCategories, appliedSubLevelCategories, location, pageNumber, pageSize],
        () => {
            const request: PagedActivityRequest = {
                topLevelCategories: appliedTopLevelCategories,
                subLevelCategories: appliedSubLevelCategories,
                radius: appliedRadius,
                latitude: location?.latitude || 0,
                longitude: location?.longitude || 0,
                pageNumber,
                pageSize,
            };
            return getPagedActivities(request);
        },
        {
            keepPreviousData: true,
            enabled: !!location,
            refetchOnWindowFocus: false,
        }
    );

    useEffect(() => {
        isLoading == true;
    })

    useEffect(() => {
        if ("geolocation" in navigator) {
            navigator.geolocation.getCurrentPosition(
                (position) => {
                    setLocation({
                        latitude: position.coords.latitude,
                        longitude: position.coords.longitude,
                    });
                },
                (error) => console.error("Geolocation error:", error),
                { enableHighAccuracy: true }
            );
        } else {
            console.error("Geolocation is not supported by this browser.");
        }
    }, []);

    const handleRadiusChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setRadius(Number(e.target.value));
    };

    const handleTopLevelCategoryChange = (category: TopLevelCategory) => {
        setTopLevelCategories((prev) =>
            prev.includes(category)
                ? prev.filter((item) => item !== category)
                : [...prev, category]
        );
    };

    const handleSubLevelCategoryChange = (subCategoryName: string) => {
        setSubLevelCategories((prev) =>
            prev.includes(subCategoryName)
                ? prev.filter((item) => item !== subCategoryName)
                : [...prev, subCategoryName]
        );
    };

    const applyFilters = () => {
        setAppliedRadius(radius);
        setAppliedTopLevelCategories(topLevelCategories);
        setAppliedSubLevelCategories(subLevelCategories);
        setPageNumber(1);
        refetch();
    };

    const handlePageChange = (newPageNumber: number) => {
        setPageNumber(newPageNumber);
        refetch();
    };

    const handlePageSizeChange = (newPageSize: number) => {
        setPageSize(newPageSize);
        setPageNumber(1);
        refetch();
    };

    const isFilterChanged =
        radius !== appliedRadius ||
        topLevelCategories.toString() !== appliedTopLevelCategories.toString() ||
        subLevelCategories.toString() !== appliedSubLevelCategories.toString();

    const maxPages = Math.ceil(data?.totalCount / pageSize);

    return (
        <Container className="custom-container px-5 py-3 navigation-margin">
            <Row>
                <Column size="col-12 col-md-4">
                    <FilterContainer>
                        <h4>Filters</h4>
                        <div className="mb-3">
                            <label htmlFor="radiusInput">Radius (km):</label>
                            <input
                                type="number"
                                id="radiusInput"
                                defaultValue={radius}
                                onChange={handleRadiusChange}
                                min={0}
                                className="form-control"
                            />
                        </div>
                        <div className="mb-3">
                            <h5>Top Level Categories</h5>
                            {Object.keys(TopLevelCategory)
                                .filter((key) => isNaN(Number(key)))
                                .map((key) => (
                                    <div key={key}>
                                        <label>
                                            <input
                                                type="checkbox"
                                                value={key}
                                                onChange={() =>
                                                    handleTopLevelCategoryChange(TopLevelCategory[key as keyof typeof TopLevelCategory])
                                                }
                                                checked={topLevelCategories.includes(TopLevelCategory[key as keyof typeof TopLevelCategory])}
                                            />
                                            {capitalize(key.toString())}
                                        </label>
                                    </div>
                                ))}
                        </div>

                        <div className="mb-3">
                            <h5>Sub Categories</h5>
                            {isSubLevelLoading ? (
                                <p>Loading...</p>
                            ) : (
                                <div style={{ maxHeight: '150px', overflowY: 'auto' }}>
                                    {fetchedSubLevelCategories?.map((subCategory) => (
                                        <div key={subCategory.id}>
                                            <label>
                                                <input
                                                    type="checkbox"
                                                    value={subCategory.name}
                                                    onChange={() => handleSubLevelCategoryChange(subCategory.name)}
                                                    checked={subLevelCategories.includes(subCategory.name)}
                                                />
                                                {subCategory.name}
                                            </label>
                                        </div>
                                    ))}
                                </div>
                            )}
                        </div>

                        <button
                            className={`btn btn-primary ${isFilterChanged ? "active" : ""}`}
                            onClick={applyFilters}
                            disabled={!isFilterChanged}
                        >
                            Apply Filters
                        </button>
                    </FilterContainer>
                </Column>
                <Column size="col-12 col-md-8">
                    <CardContainer>
                        {location ? (
                            <>
                                {isLoading || isFetching ? (
                                    <Spinner />
                                ) : error ? (
                                    <p>Error: {error.message}</p>
                                ) : (
                                    <div>
                                        {data && data.items && data.items.length === 0 ? (
                                            <p>No activities found with the selected filters.</p>
                                        ) : (
                                            <div className="d-flex flex-column gap-3">
                                                <div className="d-flex justify-content-end">
                                                    <label htmlFor="pageSizeSelect">Page Size:</label>
                                                    <select
                                                        id="pageSizeSelect"
                                                        value={pageSize}
                                                        onChange={(e) => handlePageSizeChange(Number(e.target.value))}
                                                    >
                                                        <option value={10}>10</option>
                                                        <option value={20}>20</option>
                                                        <option value={50}>50</option>
                                                    </select>
                                                </div>

                                                <div>
                                                    {data?.items?.map((activity) => (
                                                        <div key={activity.id}>
                                                            <h5>{activity.name}</h5>
                                                            <p>{activity.description}</p>
                                                        </div>
                                                    ))}
                                                </div>

                                                <div className="d-flex justify-content-center">
                                                    <button onClick={() => handlePageChange(pageNumber - 1)} disabled={pageNumber === 1}>
                                                        <i className="bi bi-arrow-left-short"></i>
                                                    </button>
                                                    <span>Page {pageNumber}</span>
                                                    <button
                                                        onClick={() => handlePageChange(pageNumber + 1)}
                                                        disabled={isLoading || !data || pageNumber === maxPages}
                                                    >
                                                        <i className="bi bi-arrow-right-short"></i>
                                                    </button>
                                                </div>
                                            </div>
                                        )}
                                    </div>
                                )}
                            </>
                        ) : (
                            <p>Ensure location services are enabled in your browser for accurate results.</p>
                        )}
                    </CardContainer>
                </Column>
            </Row>
        </Container>
    );
};

export default PageContainer;