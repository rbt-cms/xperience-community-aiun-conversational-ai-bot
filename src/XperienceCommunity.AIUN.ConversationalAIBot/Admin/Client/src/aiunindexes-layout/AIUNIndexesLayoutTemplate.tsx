import React, { useEffect, useState, useRef, useMemo } from "react";
import { usePageCommand } from "@kentico/xperience-admin-base";
import {
    Select, MenuItem, Input, RadioButton,
    RadioGroup, Pagination, Table, TableCell, TableRow
} from "@kentico/xperience-admin-components"
import '../styles/aiun-module.css';
export interface IWebsiteChannelModel {
    clientId: string;
    channelName: string;
}
export interface IIndexesResponseModel {
    items: IIndexItemModel[];
    page: number;
    size: number;
    total: number;
}
export interface IIndexItemModel {
    category: string;
    id: string;
    name: string;
    title: string;
    uploaded_date: string;
    status: string;
    department: string;
}
export interface IIndexItemFilterModel {
    page: number;
    pageSize: number;
    searchTerm: string;
    sortBy: string;
    sortDirection: string;
    typeFilter: string;
    channel: string;
}

export const DEFAULT_PAGE_SIZE = 10;
export const PAGE_SIZE_OPTIONS = [10, 20, 30, 40, 50];
export const Commands = {
    GetIndexes: "GetIndexes"
};

export const AIUNIndexesLayoutTemplate = (props: any) => {
    const [data, setData] = useState<IIndexesResponseModel | null>(props.indexesResponse || {
        items: [],
        page: 1,
        size: DEFAULT_PAGE_SIZE,
        total: 0
    });
    const [channels, setChannel] = useState<IWebsiteChannelModel[]>(props.websiteChannels);

    const [loading, setLoading] = useState(false);
    const [search, setSearch] = useState("");
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(DEFAULT_PAGE_SIZE);
    const [typeFilter, setTypeFilter] = useState("All");

    // Sorting (UI only)
    const [sortBy, setSortBy] = useState("uploaded_date");
    const [sortDir, setSortDir] = useState<"asc" | "desc">("desc");
    const [selectedchannelClientId, setSelectedChannelClientId] = useState("");

    const { execute: getIndexes } = usePageCommand<IIndexesResponseModel, IIndexItemFilterModel>(
        Commands.GetIndexes,
        {
            
            after: (response: any) => setData(response),
        }
    );

    const fetchData = async () => {

        setLoading(true);
        try {
            await getIndexes({
                page: currentPage,
                pageSize: pageSize,
                searchTerm: search,
                sortBy,
                sortDirection: sortDir,
                typeFilter,
                channel: selectedchannelClientId
            });
        } catch (err) {
            console.error("Failed to fetch indexes:", err);
            setData(null);
        } finally {
            setLoading(false);
        }
    };

    const fetchInitialData = async () => {

        setLoading(true);
        try {
            await getIndexes({
                page: 1,
                pageSize: DEFAULT_PAGE_SIZE,
                searchTerm: "",
                sortBy: "uploaded_date",
                sortDirection: "desc",
                typeFilter: "All",
                channel: ""
            });
        } catch (err) {
            console.error("Failed to fetch indexes:", err);
            setData(null);
        } finally {
            setLoading(false);
        }
    };

    const skipFetch = useRef(false);

    const handleClearSearch = () => {

        skipFetch.current = true;

        // Reset all search inputs
        setSearch("");
        setSelectedChannelClientId("");
        setTypeFilter("All");
        setSortBy("uploaded_date");
        setSortDir("desc");
        setCurrentPage(1);
        setPageSize(DEFAULT_PAGE_SIZE);

        fetchInitialData();
    };

    useEffect(() => {
        if (skipFetch.current) {
            skipFetch.current = false;
            return;
        }

        fetchData();
    }, [currentPage, pageSize, typeFilter, selectedchannelClientId]);

    const handleSort = (field: string) => {
        if (sortBy === field) {
            setSortDir(prev => (prev === "asc" ? "desc" : "asc"));
        } else {
            setSortBy(field);
            setSortDir("asc");
        }
    };

  
    const totalPages = Math.ceil((data?.total ?? 0) / pageSize);
    const sortedItems = useMemo<IIndexItemModel[]>(() => {
        if (!data?.items?.length) return [];
        const arr = [...data.items];
        arr.sort((a, b) => {
            const key = sortBy as keyof IIndexItemModel;
            const aVal = a[key];
            const bVal = b[key];
            return (aVal < bVal ? -1 : aVal > bVal ? 1 : 0) * (sortDir === "asc" ? 1 : -1);
        });
        return arr;
    }, [data, sortBy, sortDir]);

    const visiblePages = () => {
        const maxVisible = 5;
        const pages = [];

        let startPage = Math.max(1, currentPage - Math.floor(maxVisible / 2));
        let endPage = startPage + maxVisible - 1;

        if (endPage > totalPages) {
            endPage = totalPages;
            startPage = Math.max(1, endPage - maxVisible + 1);
        }

        for (let i = startPage; i <= endPage; i++) {
            pages.push(i);
        }

        return pages;
    };

    return (
        <div className="indexes-wrapper">
            <div className="size-m___exMBL indexes-text">Indexes</div>

            {/* Filters (shown only if data !== null) */}
            {data !== undefined && data !== null ? (
                <>
                    <div className="indexes-buttons">
                        <Select
                            clearable={true}
                            placeholder="Select channel"
                            onChange={setSelectedChannelClientId}
                            value={selectedchannelClientId}
                        >
                            {channels.map((c) => (
                                <MenuItem
                                    primaryLabel={c.channelName}
                                    key={c.clientId}
                                    value={c.clientId}
                                />
                            ))}
                        </Select>
                        <Input
                            onChange={(e) => setSearch(e.target.value)}
                            value={search}
                            type="text"
                            onKeyPress={(e) => e.key === 'Enter' && fetchData()}
                        />
                        <button className="button___Ky_Bj type-primary___KNJNo size-l___SKn9m" onClick={fetchData}>
                            Search
                        </button>
                    </div>

                    <div className="indexes-radio-box-wrapper">
                        <RadioGroup
                            inline={true}
                            name="TypeFilter"
                            value={typeFilter}
                            markAsRequired={true}
                            onChange={(e) => {
                                setCurrentPage(1);
                                setTypeFilter(e);
                            }}
                        >
                            {["All", "URL", "Documents"].map((option) => (
                                <RadioButton key={option} value={option}>
                                    {option}
                                </RadioButton>
                            ))}
                        </RadioGroup>
                    </div>
                </>
            ) : (
            <div></div>
            )}
            
            {/* Display error when data === null */}
            {data === undefined || data === null ? (
                <div className="message-pane___Giq7y">
                    <div className="message-pane-text">
                        <div className="size-l___dGpeD">
                            <div className="row___EJ6L9 flex-wrap___Fok8R y-align-center___smjAO">
                                Unable to fetch Indexes
                            </div>
                        </div>
                    </div>
                    <div className="subheadline___Ewicq">
                        Please check the Event Log for more details.
                    </div>
                    <div className="children___nHi_P">
                        <div className="button-wrapper___wvgI6">
                            <button
                                className="button___Ky_Bj type-primary___KNJNo size-l___SKn9m fill-container___I2NBK"
                                type="button"
                                aria-label="Reload the page"
                                data-testid="reload-page"
                                onClick={handleClearSearch}
                            >
                                Try again
                            </button>
                        </div>
                    </div>
                </div>
            ) : data?.items?.length > 0 ? (

                    <>
                        
                    {/* Table */}

                        <table className="aiun-table-wrapper">
                            <thead>
                            <tr>

                                <th 
                                    onClick={() => handleSort("name")}>
                                Name {sortBy === "name" ? (sortDir === "asc" ? "↑" : "↓") : ""}
                            </th>
                                <th 
                                    onClick={() => handleSort("title")}>
                                Title {sortBy === "title" ? (sortDir === "asc" ? "↑" : "↓") : ""}
                            </th>
                                <th 
                                    onClick={() => handleSort("uploaded_date")}>
                                Updated Time {sortBy === "uploaded_date" ? (sortDir === "asc" ? "↑" : "↓") : ""}
                            </th>
                                <th>
                                Status
                            </th>
                                </tr>
                        </thead>
                        <tbody>
                            {sortedItems.map((item,index) => (
                                <tr >
                                    <td className="w-35">
                                        <a href={item.name} target="_blank" rel="noopener noreferrer">{item.name}</a>
                                    </td>
                                    <td className="w-35">{item.title}</td>
                                    <td className="w-20">
                                        {new Date(item.uploaded_date).toLocaleString(undefined, {
                                            year: 'numeric',
                                            month: 'numeric',
                                            day: 'numeric',
                                            hour: 'numeric',
                                            minute: '2-digit',
                                            hour12: true
                                        }).replace(/, /, ' ').replace(/([ap])m/, (match) => match.toUpperCase())}
                                    </td>
                                    <td className="status W-10">
                                            <span className={item.status.toLowerCase()} >
                                                {{
                                                    "PENDING": "QUEUED",
                                                    "PROCESSING": "INDEXING",
                                                    "FAILED": "FAILED",
                                                    "COMPLETED": "COMPLETED"
                                                }[item.status] || item.status}
                                            </span> 
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>

                    {/* Pagination Controls */}

                    <div className="pagination-container">
                            <div className="pagination-wrapper">
                                <a href="#" onClick={(e) => { e.preventDefault(); setCurrentPage(1); }} className={currentPage === 1?"disabled":"enabled" } >
                                &lt;&lt; first
                            </a>
                                <a href="#" onClick={(e) => { e.preventDefault(); setCurrentPage(p => Math.max(1, p - 1)); }} className={currentPage === 1 ? "disabled" : "enabled"} >
                                &lt; previous
                            </a>
                                {visiblePages().map((page) => (
                                    <a
                                        className={currentPage === page ? "pagination-numbers active" : "pagination-numbers"}
                                        key={page}
                                        href="#"
                                        onClick={(e) => { e.preventDefault(); setCurrentPage(page); }}
                                    >
                                        {page}
                                    </a>
                                ))}

                            {totalPages > 6 && (
                                <>
                                        <a href="#" onClick={(e) => { e.preventDefault(); setCurrentPage(currentPage + 1); }} className={currentPage === totalPages ? "disabled" : "enabled"} >
                                        next &gt;
                                    </a>
                                        <a href="#" onClick={(e) => { e.preventDefault(); setCurrentPage(totalPages); }} className={currentPage === totalPages ? "disabled" : "enabled"} >
                                        last &gt;&gt;
                                    </a>
                                </>
                            )}
                        </div>

                            {/* Page Size Dropdown */}
                            <div className="pagination-dropdown-wrapper">
                            <select
                                value={pageSize}
                                onChange={(e) => {
                                    setPageSize(Number(e.target.value));
                                    setCurrentPage(1);
                                }}
                                    className="form-control"
                                   
                            >
                                {PAGE_SIZE_OPTIONS.map(size => (
                                    <option key={size} value={size}>{size}</option>
                                ))}
                            </select>
                        </div>
                    </div>
                </>
            ) : (
                <div className="message-pane___Giq7y">
                    <div style={{ padding: "0px 0px 16px" }}>
                        <div className="size-l___dGpeD">
                            <div className="row___EJ6L9 flex-wrap___Fok8R y-align-center___smjAO">
                                We couldn't find any indexes
                            </div>
                        </div>
                    </div>
                    <div className="subheadline___Ewicq">
                        Try changing your search phrase or start over
                    </div>
                    <div className="children___nHi_P">
                        <div className="button-wrapper___wvgI6">
                            <button
                                className="button___Ky_Bj type-primary___KNJNo size-l___SKn9m fill-container___I2NBK"
                                type="button"
                                aria-label="Clear your search phrase here"
                                data-testid="clear-search"
                                onClick={handleClearSearch}
                            >
                                Clear your search phrase here
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}