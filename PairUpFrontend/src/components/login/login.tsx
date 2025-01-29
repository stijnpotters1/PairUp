import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { loginAsync } from '../../services/auth-service';
import { toast } from 'react-toastify';
import { useUser } from '../../hooks/user-auth';
import { LoginRequest } from "../../models/authentication";
import Spinner from "../spinner/spinner";

const Login: React.FC = () => {
    const { setUser } = useUser();

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [rememberMe, setRememberMe] = useState(false);
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleLogin = async (event: React.FormEvent) => {
        event.preventDefault();
        setLoading(true);

        const loginData: LoginRequest = {
            email,
            password
        };

        try {
            const user = await loginAsync(loginData, rememberMe);
            if (user) {
                setUser(user);
                navigate('/trips');
                toast.success("Logged in successfully");
            } else {
                toast.error("Login failed. No user returned.");
            }
        } catch (error) {
            toast.error(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container-fluid d-flex justify-content-center align-items-center min-vh-100 navigation-padding">
            {loading ? (
                <Spinner />
            ) : (
                <div className="col-12 col-md-6 col-lg-4">
                    <form onSubmit={handleLogin} className="border p-4 shadow rounded bg-light">
                        <h2 className="text-center mb-4">Login</h2>

                        <div className="form-group mb-3">
                            <label htmlFor="email">Email</label>
                            <input
                                type="email"
                                id="email"
                                className="form-control"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                placeholder="Enter your email"
                                required
                            />
                        </div>

                        <div className="form-group mb-3">
                            <label htmlFor="password">Password</label>
                            <input
                                type="password"
                                id="password"
                                className="form-control"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                placeholder="Enter your password"
                                required
                            />
                        </div>

                        <div className="form-check mb-3">
                            <input
                                type="checkbox"
                                id="rememberMe"
                                className="form-check-input"
                                checked={rememberMe}
                                onChange={(e) => setRememberMe(e.target.checked)}
                            />
                            <label className="form-check-label" htmlFor="rememberMe">
                                Remember Me
                            </label>
                        </div>

                        <div className="d-grid">
                            <button type="submit" className="btn btn-primary mb-3">
                                Login
                            </button>
                        </div>

                        <div className="text-center">
                            <Link to="/register">Don't have an account? Register</Link>
                        </div>
                    </form>
                </div>
            )}
        </div>
    );
};

export default Login;