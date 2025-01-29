import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { registerAsync } from '../../services/auth-service';
import { toast } from 'react-toastify';
import { useUser } from "../../hooks/user-auth";
import { RegisterRequest } from "../../models/authentication";
import Spinner from "../spinner/spinner";

const Register: React.FC = () => {
    const { setUser } = useUser();

    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [rememberMe, setRememberMe] = useState(false);
    const [loading, setLoading] = useState(false); // Add loading state
    const navigate = useNavigate();

    const handleRegister = async (event: React.FormEvent) => {
        event.preventDefault();

        const registerData: RegisterRequest = {
            firstName,
            lastName,
            email,
            password,
            confirmPassword
        };

        setLoading(true);
        try {
            const user = await registerAsync(registerData, rememberMe);
            if (user) {
                setUser(user);
                navigate('/trips');
                toast.success("Registered and logged in successfully");
            } else {
                toast.error("Registration failed. No user returned.");
            }
        } catch (error) {
            toast.error(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="container-fluid d-flex justify-content-center align-items-center min-vh-100 navigation-padding">
            <div className="col-12 col-md-6 col-lg-4">
                {loading ? (
                    <Spinner />
                ) : (
                    <form onSubmit={handleRegister} className="border p-4 shadow rounded bg-light">
                        <h2 className="text-center mb-4">Register</h2>

                        <div className="form-group mb-3">
                            <label htmlFor="firstName">First Name</label>
                            <input
                                type="text"
                                id="firstName"
                                className="form-control"
                                value={firstName}
                                onChange={(e) => setFirstName(e.target.value)}
                                placeholder="Enter your first name"
                                required
                            />
                        </div>

                        <div className="form-group mb-3">
                            <label htmlFor="lastName">Last Name</label>
                            <input
                                type="text"
                                id="lastName"
                                className="form-control"
                                value={lastName}
                                onChange={(e) => setLastName(e.target.value)}
                                placeholder="Enter your last name"
                                required
                            />
                        </div>

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

                        <div className="form-group mb-3">
                            <label htmlFor="confirmPassword">Confirm Password</label>
                            <input
                                type="password"
                                id="confirmPassword"
                                className="form-control"
                                value={confirmPassword}
                                onChange={(e) => setConfirmPassword(e.target.value)}
                                placeholder="Confirm your password"
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
                                Register
                            </button>
                        </div>

                        <div className="text-center">
                            <Link to="/login">Already have an account? Login</Link>
                        </div>
                    </form>
                )}
            </div>
        </div>
    );
};

export default Register;