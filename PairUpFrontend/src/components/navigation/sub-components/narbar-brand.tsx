import React from "react";
import { Link } from "react-router-dom";

interface NavbarBrandProps {
    to: string;
    logoSrc: string;
    altText?: string;
}

const NavbarBrand: React.FunctionComponent<NavbarBrandProps> = ({ to, logoSrc, altText = "" }) => (
    <Link to={to} className="navbar-brand d-flex flex-row gap-3 align-items-center">
        <img className="img-fluid" src={logoSrc} width="50" height="auto" alt={altText} />
    </Link>
);

export default NavbarBrand;