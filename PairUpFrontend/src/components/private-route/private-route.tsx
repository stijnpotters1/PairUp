import React, { useEffect } from 'react';
import { Navigate, useLocation } from 'react-router-dom';
import { useUser } from '../../hooks/user-auth';

interface PrivateRouteProps {
    element: React.ReactElement;
    roles?: string[];
}

const PrivateRoute: React.FC<PrivateRouteProps> = ({ element, roles }) => {
    const { getUser, loading } = useUser();
    const location = useLocation();

    if (loading) {
        return <div>Loading...</div>;
    }

    const user = getUser();

    if (!user) {
        return <Navigate to="/trips" state={{ from: location }} />;
    }

    if (roles && !roles.includes(user.role.name)) {
        return <Navigate to="/trips" state={{ from: location }} />;
    }

    return element;
};

export default PrivateRoute;
