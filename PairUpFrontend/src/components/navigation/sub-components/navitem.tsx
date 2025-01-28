import React from "react";
import { Link } from "react-router-dom";
import "./navigation.css"

interface NavItemProps {
    to: string;
    children: React.ReactNode;
    onClick?: () => void;
}

const NavItem: React.FunctionComponent<NavItemProps> = ({ to, children, onClick }) => (
    <li className="d-flex py-2 px-2">
        <Link to={to} className="nav-item" onClick={onClick}>
            {children}
        </Link>
    </li>
);

export default NavItem;