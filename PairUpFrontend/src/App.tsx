import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Navigation from './components/navigation/navigation';
import PageContainer from './containers/page-container';

const App: React.FC = () => {
    return (
        <Router>
            <div className="d-flex flex-column">
                <Navigation />
                <Routes>
                    <Route path="/*" element={<Navigate to="/trips" />} />
                    <Route path="/trips" element={<PageContainer />} />
                </Routes>
            </div>
        </Router>
    );
};

export default App;