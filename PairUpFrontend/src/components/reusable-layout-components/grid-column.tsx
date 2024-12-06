import React, { ReactNode } from "react";

interface GridColumnProps {
    children: ReactNode;
    className?: string;
    size?: string;
}

const GridColumn: React.FunctionComponent<GridColumnProps> = ({ children, className = "", size = "" }) => (
    <div className={`${size} ${className}`}>{children}</div>
);

export default GridColumn;