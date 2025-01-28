import React, { useState } from 'react';
import { useMutation } from 'react-query';
import { updateUserAsync } from '../../services/user-service';
import { UserRequest } from '../../models/user';
import { toast } from 'react-toastify';
import { useUser } from '../../hooks/user-auth';
import Form from "../form/form";
import GenericModal from "../generic-modal/generic-modal";
import Spinner from "../spinner/spinner";

const Profile: React.FC = () => {
    const { getUser, setUser } = useUser();
    const currentUser = getUser();

    const [formData, setFormData] = useState<UserRequest>({
        firstName: currentUser?.firstName || '',
        lastName: currentUser?.lastName || '',
        email: currentUser?.email || '',
        password: '',
        roleId: currentUser?.role.id || '',
    });

    const [isFormValid, setFormValid] = useState(false);
    const [isModalOpen, setModalOpen] = useState(false);

    const updateUserProfileMutation = useMutation(
        (userData: UserRequest) => updateUserAsync(currentUser!.id, userData),
        {
            onSuccess: (userData) => {
                toast.success('Profile updated successfully.');
                setUser(userData);
            },
            onError: () => {
                toast.error('Error updating profile.');
            },
        }
    );

    const handleFormChange = (data: { [key: string]: string }, isValid: boolean) => {
        const userData: UserRequest = {
            firstName: data.firstName,
            lastName: data.lastName,
            email: data.email,
            password: data.password,
            roleId: data.roleId,
        };

        if (JSON.stringify(formData) !== JSON.stringify(userData) || isFormValid !== isValid) {
            setFormData(userData);
            setFormValid(isValid);
        }
    };

    const handleConfirmSave = () => {
        if (isFormValid) {
            setModalOpen(true);
        }
    };

    const handleModalConfirm = () => {
        updateUserProfileMutation.mutate(formData);
        setModalOpen(false);
    };

    const handleModalCancel = () => {
        setModalOpen(false);
    };

    const isFormUnchanged = () => {
        return (
            formData.firstName === currentUser?.firstName &&
            formData.lastName === currentUser?.lastName &&
            formData.email === currentUser?.email &&
            formData.roleId === currentUser?.role.id &&
            !formData.password
        );
    };

    return (
        <div className="container min-vh-100">
            {updateUserProfileMutation.isLoading ? (
                <Spinner />
            ) : (
                <div className="navigation-padding">
                    <div className="my-4">
                        <h2 className="fw-bold">Profile</h2>
                        <hr className="my-3" />
                    </div>
                    <div className="row justify-content-center">
                        <div className="col-12 col-md-8">
                            <div className="card shadow-sm p-4">
                                <Form
                                    userData={formData}
                                    onChange={handleFormChange}
                                    roles={null}
                                />
                                <div className="mt-4 d-flex justify-content-end">
                                    <button
                                        className="btn btn-primary"
                                        onClick={handleConfirmSave}
                                        disabled={!isFormValid || isFormUnchanged()}
                                    >
                                        Save Profile
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <GenericModal
                        title="Confirm Save"
                        show={isModalOpen}
                        handleClose={handleModalCancel}
                        onConfirm={handleModalConfirm}
                    >
                        <div className="px-3 py-4 bg-danger bg-opacity-25">
                            <p className="mb-0">Are you sure you want to save these changes?</p>
                        </div>
                    </GenericModal>
                </div>
            )}
        </div>
    );
};

export default Profile;