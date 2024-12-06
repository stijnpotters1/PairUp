import React from "react";
import "./navigation.css"

interface DropdownProps {
    label: string;
    children: React.ReactNode;
    isOpen?: boolean;
    onToggle?: () => void;
}

const Dropdown: React.FunctionComponent<DropdownProps> = ({ label, children, isOpen, onToggle }) => (
    <li className="nav-item dropdown">
        <a
            className="nav-link dropdown-toggle fw-medium fs-6"
            role="button"
            onClick={onToggle}
            style={{ cursor: "pointer", color: "#FFFFFF" }}
            aria-expanded={isOpen}
        >
            {label}
        </a>
        <ul className={`dropdown-menu ${isOpen ? "show" : ""}`} aria-labelledby="navbarDropdown">
            {children}
        </ul>
    </li>
);

export default Dropdown;