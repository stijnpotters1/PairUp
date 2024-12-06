import React, { ReactNode } from "react";

interface GridRowProps {
    children: ReactNode;
    className?: string;
}

const GridRow: React.FunctionComponent<GridRowProps> = ({ children, className = "" }) => (
    <div className={`row ${className}`}>
        {children}
    </div>
);

export default GridRow;