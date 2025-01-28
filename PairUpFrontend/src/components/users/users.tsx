import React, { useState } from 'react';
import { useQuery, useMutation } from 'react-query';
import {
    deleteUserAsync,
    getUsersAsync,
    postUserAsync,
    updateUserAsync
} from '../../services/user-service';
import { getRolesAsync } from "../../services/role-service";
import { UserRequest, UserResponse } from '../../models/user';
import { Role } from "../../models/role";
import GenericModal from "../generic-modal/generic-modal";
import Form from '../form/form';
import { toast } from "react-toastify";
import { useUser } from "../../hooks/user-auth";
import Spinner from "../spinner/spinner";

const Users: React.FunctionComponent = () => {
    const { data: users = [], isLoading: isLoadingUsers, isError, refetch } = useQuery<UserResponse[]>('users', getUsersAsync);
    const { data: roles = [], isLoading: isLoadingRoles } = useQuery<Role[]>('roles', getRolesAsync);

    const [selectedUser, setSelectedUser] = useState<UserResponse | null>(null);
    const [isDeleteModalOpen, setDeleteModalOpen] = useState(false);
    const [isUpdateModalOpen, setUpdateModalOpen] = useState(false);
    const [isAddModalOpen, setAddModalOpen] = useState(false);
    const [isFormValid, setFormValid] = useState(false);
    const [formData, setFormData] = useState<UserRequest>({
        firstName: '',
        lastName: '',
        email: '',
        password: '',  // Keep the password field empty for now
        roleId: ''
    });

    const { getUser } = useUser();

    const deleteUserMutation = useMutation(deleteUserAsync, {
        onSuccess: () => {
            refetch();
            toast.success('User deleted successfully.');
        },
        onError: () => {
            toast.error('Error deleting user.');
        }
    });

    const updateUserMutation = useMutation((userId: string) => {
        if (!formData) return;
        return updateUserAsync(userId, formData);
    }, {
        onSuccess: () => {
            refetch();
            toast.success('User updated successfully.');
            setUpdateModalOpen(false);
            resetFormData();
        },
        onError: () => {
            toast.error('Error updating user.');
        }
    });

    const createUserMutation = useMutation(postUserAsync, {
        onSuccess: () => {
            refetch();
            toast.success('User created successfully.');
            setAddModalOpen(false);
            resetFormData();
        },
        onError: () => {
            toast.error('Error creating user.');
        }
    });

    const resetFormData = () => {
        setFormData({
            firstName: '',
            lastName: '',
            email: '',
            password: '',  // Reset password
            roleId: ''
        });
    };

    const handleModalToggle = (user: UserResponse | null, modalType: 'update' | 'delete' | 'add') => {
        if (modalType === 'update') {
            setSelectedUser(user);
            setFormData(user ? {
                firstName: user.firstName,
                lastName: user.lastName,
                email: user.email,
                password: '',  // Keep the password field empty for updates
                roleId: user.role.id
            } : {
                firstName: '',
                lastName: '',
                email: '',
                password: '',
                roleId: ''
            });
            setUpdateModalOpen(true);
        } else if (modalType === 'delete') {
            setSelectedUser(user);
            setDeleteModalOpen(true);
        } else if (modalType === 'add') {
            setSelectedUser(null);
            resetFormData();  // Reset form data for adding a new user
            setAddModalOpen(true);
        }
    };

    const handleDeleteUser = (id: string) => {
        deleteUserMutation.mutate(id);
        setDeleteModalOpen(false);
    };

    const handleFormChange = (data: { [key: string]: string }, isValid: boolean) => {
        const userData: UserRequest = {
            firstName: data.firstName,
            lastName: data.lastName,
            email: data.email,
            password: data.password,
            roleId: data.roleId,
        };

        // Update form data only if changed
        if (JSON.stringify(formData) !== JSON.stringify(userData) || isFormValid !== isValid) {
            setFormData(userData);
            setFormValid(isValid);
        }
    };

    const handleConfirmUser = () => {
        if (selectedUser) {
            updateUserMutation.mutate(selectedUser.id);
        } else {
            createUserMutation.mutate(formData); // Create user
        }
    };

    // Function to check if the form data is unchanged
    const isFormUnchanged = () => {
        if (selectedUser) {
            return (
                formData.firstName === selectedUser.firstName &&
                formData.lastName === selectedUser.lastName &&
                formData.email === selectedUser.email &&
                formData.roleId === selectedUser.role.id &&
                !formData.password // Ignore password in comparison
            );
        }
        return false; // If no selected user, consider it changed for add case
    };

    const isLoading = isLoadingUsers || isLoadingRoles || deleteUserMutation.isLoading || updateUserMutation.isLoading || createUserMutation.isLoading;

    return (
        <div className="container min-vh-100">
            {isLoading ? (
                <Spinner />
            ) : (
                <div className="navigation-padding">
                    <div className="text-center my-4">
                        <div className="d-flex flex-row justify-content-between">
                            <h2 className="fw-bold">Users</h2>
                            <button className="btn btn-primary px-3" onClick={() => handleModalToggle(null, 'add')}>
                                Add User
                            </button>
                        </div>
                        <hr className="my-2" />
                    </div>

                    {isError ? (
                        <div className="alert alert-danger" role="alert">
                            Error fetching users.
                        </div>
                    ) : users.length > 0 ? (
                        <ul className="list-group mt-3">
                            {users
                                .filter(user => user.id !== getUser()?.id)
                                .map((user) => (
                                    <li key={user.id} className="list-group-item">
                                        <div className="row align-items-center">
                                            <div className="col-md-4 d-none d-md-block text-truncate">
                                                <span>{user.firstName} {user.lastName}</span>
                                            </div>
                                            <div className="col-7 col-md-4 text-truncate">
                                                <span>{user.email}</span>
                                            </div>
                                            <div className="col-3 col-md-2 text-truncate">
                                                <span>{user.role.name}</span>
                                            </div>
                                            <div className="col-2 d-flex justify-content-end">
                                                <i className="bi bi-pen cursor-pointer me-3" onClick={() => handleModalToggle(user, 'update')}></i>
                                                <i className="bi bi-trash text-danger cursor-pointer" onClick={() => handleModalToggle(user, 'delete')}></i>
                                            </div>
                                        </div>
                                    </li>
                                ))
                            }
                        </ul>
                    ) : (
                        <div className="text-center mt-5">
                            <p className="text-muted">No users available.</p>
                        </div>
                    )}

                    <GenericModal
                        title="Add User"
                        show={isAddModalOpen}
                        handleClose={() => setAddModalOpen(false)}
                        onConfirm={handleConfirmUser}
                        confirmDisabled={!isFormValid || isFormUnchanged()}
                    >
                        <Form
                            userData={formData}
                            onChange={handleFormChange}
                            roles={roles}
                        />
                    </GenericModal>

                    <GenericModal
                        title="Update User"
                        show={isUpdateModalOpen}
                        handleClose={() => setUpdateModalOpen(false)}
                        onConfirm={handleConfirmUser}
                        confirmDisabled={!isFormValid || isFormUnchanged()}
                    >
                        <Form
                            userData={formData}
                            onChange={handleFormChange}
                            roles={roles}
                        />
                    </GenericModal>

                    <GenericModal
                        title="Confirm Delete"
                        show={isDeleteModalOpen}
                        handleClose={() => setDeleteModalOpen(false)}
                        onConfirm={() => handleDeleteUser(selectedUser?.id!)}
                        confirmDisabled={false}
                    >
                        <div className="px-3 py-4 bg-danger bg-opacity-25">
                            <p className="mb-0">Are you sure you want to delete user {selectedUser?.firstName} {selectedUser?.lastName}?</p>
                        </div>
                    </GenericModal>
                </div>
            )}
        </div>
    );
};

export default Users;