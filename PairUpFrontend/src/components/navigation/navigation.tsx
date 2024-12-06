import React, { useState } from "react";
import Navbar from "./sub-components/navbar";
import NavbarBrand from "./sub-components/narbar-brand";
import NavbarToggler from "./sub-components/navbar-toggler";
import Nav from "./sub-components/nav";
import Dropdown from "./sub-components/dropdown";
import DropdownItem from "./sub-components/dropdown-item";

const Navigation: React.FunctionComponent = () => {
    const [showMegaMenu, setShowMegaMenu] = useState(false);
    const [showDropDownItems, setShowDropDownItems] = useState(false);

    return (
        <Navbar>
            <NavbarBrand
                to="/trips"
                logoSrc="../../../src/assets/pairup-logo-navigation.png"
                altText="Pair Up logo"
            />

            <NavbarToggler onClick={() => setShowMegaMenu(!showMegaMenu)} />

            <div className={`collapse navbar-collapse ${showMegaMenu ? "show" : ""}`}>
                <Nav className={showMegaMenu ? "mega-menu" : ""}>
                    <Dropdown
                        label="Pair Up"
                        isOpen={showDropDownItems}
                        onToggle={() => setShowDropDownItems(!showDropDownItems)}
                    >
                        <DropdownItem to="/about" onClick={() => setShowMegaMenu(false)}>
                            About
                        </DropdownItem>
                        <DropdownItem to="/contact" onClick={() => setShowMegaMenu(false)}>
                            Contact
                        </DropdownItem>
                    </Dropdown>
                </Nav>
            </div>
        </Navbar>
    );
};

export default Navigation;