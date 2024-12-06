import React from "react";
import { Link } from "react-router-dom";
import "./navigation.css"

interface DropdownItemProps {
    to: string;
    children: React.ReactNode;
    onClick?: () => void;
}

const DropdownItem: React.FunctionComponent<DropdownItemProps> = ({ to, children, onClick }) => (
    <li>
        <Link to={to} className="dropdown-item" onClick={onClick}>
            {children}
        </Link>
    </li>
);

export default DropdownItem;