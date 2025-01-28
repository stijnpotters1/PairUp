import React from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';

interface GenericModalProps {
    title: string;
    show: boolean;
    children: React.ReactNode;
    handleClose: () => void;
    onConfirm: () => void;
    confirmDisabled?: boolean;
}

const GenericModal: React.FC<GenericModalProps> = ({ title, show, handleClose, children, onConfirm, confirmDisabled = false }) => {
    return (
        <Modal show={show} onHide={handleClose} centered>
            <Modal.Header closeButton>
                <Modal.Title>{title}</Modal.Title>
            </Modal.Header>
            <Modal.Body>{children}</Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={onConfirm} disabled={confirmDisabled}>
                    Confirm
                </Button>
            </Modal.Footer>
        </Modal>
    );
};

export default GenericModal;
