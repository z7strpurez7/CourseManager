import React from 'react';
import { Route, Routes} from 'react-router-dom';
import HomePage from './pages/HomePage';
import LoginPage from './pages/LoginPage';
import Dashboard from './pages/Dashboard/Dashboard';
import RegistrationPage from './pages/RegistrationPage';
import CoursePage from './pages/CoursePage/CoursePage';
import Logout from './components/Logout'

import 'bootstrap/dist/css/bootstrap.min.css';


function App() {
  return (
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/course/:courseId" element={<CoursePage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/Register" element={<RegistrationPage />}/>
        <Route path="/Dashboard" element={<Dashboard />} />
        <Route path="/Logout" element={<Logout />}/>
      </Routes>
  
  );
}

export default App;