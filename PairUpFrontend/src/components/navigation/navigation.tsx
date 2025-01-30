import React, { useState } from "react";
import Navbar from "./sub-components/navbar";
import NavbarBrand from "./sub-components/narbar-brand";
import NavbarToggler from "./sub-components/navbar-toggler";
import Nav from "./sub-components/nav";
import NavItem from "./sub-components/navitem";
import {isAuthenticated, logout} from "../../services/auth-service";
import {useUser} from "../../hooks/user-auth";

const Navigation: React.FunctionComponent = () => {
    const { isAdmin } = useUser();

    const [showMegaMenu, setShowMegaMenu] = useState(false);

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
                    <NavItem to="/trips">
                        Trips
                    </NavItem>

                    {isAuthenticated() ? (
                        <>
                            <NavItem to="/liked">
                                Liked
                            </NavItem>

                            {isAdmin && (
                                <NavItem to="/users">
                                    Users
                                </NavItem>
                            )}

                            <NavItem to="/profile">
                                Profile
                            </NavItem>

                            <NavItem to="/logout" onClick={() => logout()}>
                                Logout
                            </NavItem>
                        </>
                        ) : (
                        <NavItem to="/login">
                            Login
                        </NavItem>
                    )}
                </Nav>
            </div>
        </Navbar>
    );
};

export default Navigation;