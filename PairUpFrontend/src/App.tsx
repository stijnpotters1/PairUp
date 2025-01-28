import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Navigation from './components/navigation/navigation';
import PageContainer from './containers/page-container';
import {UserProvider} from "./hooks/user-auth";
import Users from "./components/users/users";
import PrivateRoute from "./components/private-route/private-route";

const App: React.FC = () => {
    return (
        <UserProvider>
            <Router>
                <div className="d-flex flex-column">
                    <Navigation />
                    <Routes>
                        <Route path="/*" element={<Navigate to="/trips" />} />
                        <Route path="/trips" element={<PageContainer />} />
                        <Route path="/users" element={<PrivateRoute element={<Users />} roles={['Admin']} />} />
                        <Route path="/register" element={<Navigate to="/register" />} />
                        <Route path="/login" element={<Navigate to="/login" />} />
                        <Route path="/profile" element={<Navigate to="/profile" />} />
                    </Routes>
                </div>
            </Router>
        </UserProvider>
    );
};

export default App;