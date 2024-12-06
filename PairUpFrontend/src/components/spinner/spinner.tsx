import React from 'react';

const Spinner: React.FunctionComponent = () => (
    <div className="d-flex justify-content-center align-items-center min-vh-100">
        <div className="spinner-border text-secondary" role="status" />
    </div>
);

export default Spinner;