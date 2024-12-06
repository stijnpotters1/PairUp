import React, { ReactNode } from "react";

interface ContainerProps {
    children: ReactNode;
    className?: string;
}

const Container: React.FunctionComponent<ContainerProps> = ({ children, className = "" }) => (
    <div className={`container ${className}`}>
        {children}
    </div>
);

export default Container;