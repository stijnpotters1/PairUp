import React, { useState, useEffect } from 'react';
import { Link, useLocation } from 'react-router-dom';
import "./navigation.css";

const Navigation: React.FunctionComponent = () => {
    const location = useLocation();
    const [showMegaMenu, setShowMegaMenu] = useState(false);

    const renderNavLinks = () => (
        <ul className="navbar-nav ms-auto gap-2">
            <li className="nav-item dropdown">
                <a
                    className={`nav-link dropdown-toggle fw-medium fs-6`}
                    id="navbarDropdown"
                    data-bs-toggle="dropdown"
                    aria-expanded="false"
                    style={{
                        color: !location.pathname.startsWith('/trips') ? '#808080' : '#FFFFFF'
                    }}
                >
                    Pair Up
                </a>
                <ul className="dropdown-menu" aria-labelledby="navbarDropdown">
                    <li>
                        <Link
                            to="/about"
                            className="dropdown-item"
                            onClick={() => setShowMegaMenu(false)}
                        >
                            About
                        </Link>
                    </li>
                    <li>
                        <Link
                            to="/contact"
                            className="dropdown-item"
                            onClick={() => setShowMegaMenu(false)}
                        >
                            Contact
                        </Link>
                    </li>
                </ul>
            </li>
        </ul>
    );

    return (
        <nav
            className={`navbar navbar-expand-lg bg-primary px-3 fixed-top`}
        >
            <Link to={"/trips"} className="navbar-brand d-flex flex-row gap-3 align-items-center">
                <img
                    className="img-fluid"
                    src="../../../src/assets/pairup-logo-navigation.png"
                    width="50"
                    height="auto"
                    alt="Pair Up logo"
                />
            </Link>

            <button
                className="navbar-toggler d-lg-none"
                type="button"
                onClick={() => setShowMegaMenu(!showMegaMenu)}
            >
                <span className="navbar-toggler-icon"></span>
            </button>

            <div className={`collapse navbar-collapse ${showMegaMenu ? 'show' : ''}`}>
                {showMegaMenu ? (
                    <ul className="navbar-nav ms-auto gap-2 mega-menu">
                        {renderNavLinks()}
                    </ul>
                ) : (
                    renderNavLinks()
                )}
            </div>
        </nav>
    );
};

export default Navigation;
