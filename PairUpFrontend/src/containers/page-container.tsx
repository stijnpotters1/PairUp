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
import "../components/filter/filter.css";

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

    const [isRadiusOpen, setIsRadiusOpen] = useState<boolean>(true);
    const [isTopLevelCategoriesOpen, setIsTopLevelCategoriesOpen] = useState<boolean>(false);
    const [isSubLevelCategoriesOpen, setIsSubLevelCategoriesOpen] = useState<boolean>(false);
    const [isFiltersOpen, setIsFiltersOpen] = useState<boolean>(true);

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

        const handleResize = () => {
            setIsFiltersOpen(window.matchMedia("(min-width: 768px)").matches);
        };

        handleResize();
        window.addEventListener("resize", handleResize);

        return () => window.removeEventListener("resize", handleResize);
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

    const clearAllFilters = () => {
        setRadius(DEFAULT_RADIUS);
        setAppliedRadius(DEFAULT_RADIUS);
        setTopLevelCategories([]);
        setAppliedTopLevelCategories([]);
        setSubLevelCategories([]);
        setAppliedSubLevelCategories([]);
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

    const isFilterChangedTowardsDefault =
        radius !== DEFAULT_RADIUS ||
        topLevelCategories.length !== 0 ||
        subLevelCategories.length !== 0;

    const maxPages = Math.ceil(data?.totalCount / pageSize);

    return (
        <Container className="custom-container px-md-0 px-4 py-3 navigation-margin">
            <Row>
                <Column size="col-12 col-md-3 mb-md-5">
                    <FilterContainer>
                        <div className="d-flex justify-content-between align-items-center">
                            <h4 className="ms-1 d-flex align-self-center">Filters</h4>
                            <div className="d-flex flex-row gap-3">
                                {isFilterChangedTowardsDefault && (
                                    <div
                                        className="d-flex align-items-center text-danger text-decoration-underline cursor-pointer"
                                        onClick={clearAllFilters}
                                    >
                                        Clear
                                    </div>
                                )}

                                <div
                                    className="filter-toggle p-0 text-decoration-none"
                                    onClick={() => setIsFiltersOpen(!isFiltersOpen)}
                                >
                                    <i
                                        className={`bi ${isFiltersOpen ? "bi-chevron-up" : "bi-chevron-down"}`}
                                        style={{ fontSize: "1.5rem", color: "#333" }}
                                    ></i>
                                </div>
                            </div>
                        </div>
                        {isFiltersOpen ? (
                            <>
                                <div className="my-3 ms-1">
                                    <div
                                        className="filter-toggle d-flex justify-content-between"
                                        onClick={() => setIsRadiusOpen(!isRadiusOpen)}
                                    >
                                        <p className="fw-semibold mb-1">Radius</p>
                                        <i
                                            className={`bi ${isRadiusOpen ? "bi-chevron-down" : "bi-chevron-up"}`}
                                        ></i>
                                    </div>

                                    {isRadiusOpen ? (
                                        <input
                                            type="number"
                                            id="radiusInput"
                                            value={radius}
                                            onChange={handleRadiusChange}
                                            min={0}
                                            className="form-control rounded-3"
                                        />
                                    ) : (
                                        <hr className="m-0" />
                                    )}
                                </div>

                                {/* Categories filter */}
                                <div className="mb-3 ms-1">
                                    <div
                                        className="filter-toggle d-flex justify-content-between"
                                        onClick={() => setIsTopLevelCategoriesOpen(!isTopLevelCategoriesOpen)}
                                    >
                                        <p className="fw-semibold mb-1">Categories</p>
                                        <i
                                            className={`bi ${isTopLevelCategoriesOpen ? "bi-chevron-down" : "bi-chevron-up"}`}
                                        ></i>
                                    </div>

                                    {isTopLevelCategoriesOpen ? (
                                        <div>
                                            {Object.keys(TopLevelCategory)
                                                .filter((key) => isNaN(Number(key)))
                                                .map((key) => (
                                                    <div key={key} className="form-check clickable">
                                                        <input
                                                            type="checkbox"
                                                            value={key}
                                                            id={`topLevel-${key}`}
                                                            onChange={() =>
                                                                handleTopLevelCategoryChange(
                                                                    TopLevelCategory[key as keyof typeof TopLevelCategory]
                                                                )
                                                            }
                                                            checked={topLevelCategories.includes(
                                                                TopLevelCategory[key as keyof typeof TopLevelCategory]
                                                            )}
                                                            className="form-check-input rounded-circle"
                                                        />
                                                        <label
                                                            htmlFor={`topLevel-${key}`}
                                                            className="form-check-label"
                                                        >
                                                            {capitalize(key.toString())}
                                                        </label>
                                                    </div>
                                                ))}
                                        </div>
                                    ) : (
                                        <hr className="m-0" />
                                    )}
                                </div>

                                <div className="mb-4">
                                    <div
                                        className="filter-toggle d-flex justify-content-between ms-1"
                                        onClick={() => setIsSubLevelCategoriesOpen(!isSubLevelCategoriesOpen)}
                                    >
                                        <p className="fw-semibold mb-1">Sub Categories</p>
                                        <i
                                            className={`bi ${isSubLevelCategoriesOpen ? "bi-chevron-down" : "bi-chevron-up"}`}
                                        ></i>
                                    </div>
                                    {isSubLevelCategoriesOpen ? (
                                        <div style={{ maxHeight: "300px" }} className="overflow-y-auto">
                                            {isSubLevelLoading ? (
                                                <p>Loading...</p>
                                            ) : (
                                                fetchedSubLevelCategories?.map((subCategory) => (
                                                    <div key={subCategory.id} className="form-check clickable ms-1">
                                                        <input
                                                            type="checkbox"
                                                            value={subCategory.name}
                                                            id={`subLevel-${subCategory.name}`}
                                                            onChange={() =>
                                                                handleSubLevelCategoryChange(subCategory.name)
                                                            }
                                                            checked={subLevelCategories.includes(subCategory.name)}
                                                            className="form-check-input rounded-circle"
                                                        />
                                                        <label
                                                            htmlFor={`subLevel-${subCategory.name}`}
                                                            className="form-check-label"
                                                        >
                                                            {subCategory.name}
                                                        </label>
                                                    </div>
                                                ))
                                            )}
                                        </div>
                                    ) : (
                                        <hr className="m-0" />
                                    )}
                                </div>

                                {/* Apply Filters Button */}
                                <button
                                    className={`w-50 btn btn-primary mb-4 ms-1 ${isFilterChanged ? "active" : ""}`}
                                    onClick={applyFilters}
                                    disabled={!isFilterChanged}
                                >
                                    Apply Filters
                                </button>
                            </>
                        ) : (
                            <hr className="m-0" />
                        )}
                    </FilterContainer>
                </Column>

                {/* Card Container */}
                <hr className="d-md-none" />
                <Column size="col-12 col-md-9">
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
                                            <p className="mt-5 text-center">No activities found with the selected filters...</p>
                                        ) : (
                                            <div className="d-flex flex-column gap-3">
                                                <div className="d-flex justify-content-end">
                                                    <select
                                                        value={pageSize}
                                                        onChange={(e) => handlePageSizeChange(Number(e.target.value))}
                                                        className="form-select w-auto"
                                                    >
                                                        <option value={10}>10</option>
                                                        <option value={20}>20</option>
                                                        <option value={50}>50</option>
                                                    </select>
                                                </div>

                                                <div className="row row-cols-1">
                                                    {data?.items?.map((activity) => (
                                                        <div key={activity.id} className="col mb-4">
                                                            <div className="card animate__animated animate__fadeIn card-hover cursor-pointer">
                                                                <div className="row g-0">
                                                                    <div className="col-md-4">
                                                                        <img
                                                                            src={activity.image}
                                                                            alt={activity.name}
                                                                            className="card-img-top"
                                                                        />
                                                                    </div>
                                                                    <div className="col-md-8">
                                                                        <div className="card-body">
                                                                            <h5 className="card-title">{activity.name}</h5>
                                                                            <p className="card-text">{activity.description}</p>
                                                                            <a href={activity.url} className="btn btn-primary">
                                                                                View Details
                                                                            </a>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    ))}
                                                </div>

                                                <div className="d-flex justify-content-center">
                                                    <button
                                                        onClick={() => handlePageChange(pageNumber - 1)}
                                                        disabled={pageNumber === 1}
                                                        className="btn btn-outline-secondary me-2 rounded-2"
                                                    >
                                                        &lt;
                                                    </button>
                                                    <span className="align-self-center px-2">{pageNumber}</span>
                                                    <button
                                                        onClick={() => handlePageChange(pageNumber + 1)}
                                                        disabled={isLoading || !data || pageNumber === maxPages}
                                                        className="btn btn-outline-secondary ms-2 rounded-2"
                                                    >
                                                        &gt;
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