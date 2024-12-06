import React from "react";
import "./navigation.css"

interface NavbarTogglerProps {
    onClick: () => void;
}

const NavbarToggler: React.FunctionComponent<NavbarTogglerProps> = ({ onClick }) => (
    <button
        className="navbar-toggler d-lg-none"
        type="button"
        onClick={onClick}
        aria-expanded="false"
    >
        <span className="navbar-toggler-icon"></span>
    </button>
);

export default NavbarToggler;