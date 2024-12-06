import React from "react";

interface NavbarProps {
    children: React.ReactNode;
}

const Navbar: React.FunctionComponent<NavbarProps> = ({ children }) => (
    <nav className="navbar navbar-expand-md bg-primary px-3 fixed-top">
        {children}
    </nav>
);

export default Navbar;