import React from "react";

interface NavProps {
    children: React.ReactNode;
    className?: string;
}

const Nav: React.FunctionComponent<NavProps> = ({ children, className }) => (
    <ul className={`navbar-nav ms-auto gap-2 ${className ?? ""}`}>{children}</ul>
);

export default Nav;