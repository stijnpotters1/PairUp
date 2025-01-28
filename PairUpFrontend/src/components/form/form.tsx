import React, { useEffect, useState } from 'react';
import { UserRequest } from '../../models/user';
import { Role } from '../../models/role';

interface FormProps {
    userData: UserRequest | null;
    onChange: (data: { [key: string]: string }, isValid: boolean) => void;
    roles: Role[] | null;
}

const Form: React.FunctionComponent<FormProps> = ({ userData, onChange, roles }) => {
    const [errors, setErrors] = useState({
        email: '',
        password: '',
    });

    // Validate individual fields
    const validateField = (name: string, value: string) => {
        let emailError = '';
        let passwordError = '';

        if (name === 'email') {
            const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailPattern.test(value)) {
                emailError = 'Invalid email format';
            }
        }

        if (name === 'password') {
            if (value && value.length < 12) {
                passwordError = 'Password must be at least 12 characters';
            }
        }

        setErrors((prev) => ({
            ...prev,
            email: name === 'email' ? emailError : prev.email,
            password: name === 'password' ? passwordError : prev.password,
        }));
    };

    // Validate the entire form
    const validateForm = (formData: { [key: string]: string | undefined }): boolean => {
        const isValid =
            (formData.firstName || '').trim() !== '' &&
            (formData.lastName || '').trim() !== '' &&
            (formData.email || '').trim() !== '' &&
            (roles ? roles.length === 0 || (formData.roleId || '').trim() !== '' : true) &&
            errors.email === '' &&
            errors.password === '';

        return isValid;
    };

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const { name, value } = e.target;
        const newData = { ...userData, [name]: value };

        validateField(name, value); // Validate current field
        const isValid = validateForm(newData); // Validate entire form
        onChange(newData, isValid); // Update parent component
    };

    useEffect(() => {
        // Initial validation when userData changes
        const isValid = validateForm(userData || {});
        onChange(userData || {}, isValid);
    }, [userData, onChange, roles]); // Run when userData or roles change

    return (
        <form>
            {/* Form Fields: First Name, Last Name, Email, Password, Role */}
            <div className="mb-3">
                <label htmlFor="firstName" className="form-label">First Name</label>
                <input
                    type="text"
                    className="form-control"
                    id="firstName"
                    name="firstName"
                    value={userData?.firstName || ''}
                    onChange={handleChange}
                    required
                />
            </div>
            <div className="mb-3">
                <label htmlFor="lastName" className="form-label">Last Name</label>
                <input
                    type="text"
                    className="form-control"
                    id="lastName"
                    name="lastName"
                    value={userData?.lastName || ''}
                    onChange={handleChange}
                    required
                />
            </div>
            <div className="mb-3">
                <label htmlFor="email" className="form-label">Email</label>
                <input
                    type="email"
                    className="form-control"
                    id="email"
                    name="email"
                    value={userData?.email || ''}
                    onChange={handleChange}
                    required
                />
                {errors.email && <small className="text-danger">{errors.email}</small>}
            </div>
            <div className="mb-3">
                <label htmlFor="password" className="form-label">Password</label>
                <input
                    type="password"
                    className="form-control"
                    id="password"
                    name="password"
                    onChange={handleChange}
                    placeholder="Leave blank to keep unchanged"
                />
                {errors.password && <small className="text-danger">{errors.password}</small>}
            </div>
            {roles && roles.length > 0 && (
                <div className="mb-3">
                    <label htmlFor="roleId" className="form-label">Role</label>
                    <select
                        className="form-select"
                        id="roleId"
                        name="roleId"
                        value={userData?.roleId || ''}
                        onChange={handleChange}
                        required
                    >
                        <option value="">Select Role</option>
                        {roles.map((role) => (
                            <option key={role.id} value={role.id}>{role.name}</option>
                        ))}
                    </select>
                </div>
            )}
        </form>
    );
};

export default Form;