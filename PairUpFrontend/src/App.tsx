import React, {useState} from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Navigation from './components/navigation/navigation';
import PageContainer from './containers/page-container';
import {UserProvider} from "./hooks/user-auth";
import Users from "./components/users/users";
import PrivateRoute from "./components/private-route/private-route";
import Profile from "./components/profile/profile";
import {Activity} from "./models/activity";
import Register from "./components/register/register";
import Login from "./components/login/login";

const App: React.FC = () => {
    const [activities, setActivities] = useState<Activity[]>([]);

    return (
        <UserProvider>
            <Router>
                <div className="d-flex flex-column">
                    <Navigation />
                    <Routes>
                        <Route path="/*" element={<Navigate to="/trips" />} />
                        <Route path="/trips" element={<PageContainer setActivities={setActivities} />} />
                        <Route path="/liked" element={<PrivateRoute element={<PageContainer activities={activities} />} roles={['User', 'Admin']} />} />
                        <Route path="/users" element={<PrivateRoute element={<Users />} roles={['Admin']} />} />
                        <Route path="/profile" element={<PrivateRoute element={<Profile />} roles={['User', 'Admin']} />} />
                        <Route path="/register" element={<Register />} />
                        <Route path="/login" element={<Login />} />
                    </Routes>
                </div>
            </Router>
        </UserProvider>
    );
};

export default App;