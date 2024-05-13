import React from 'react';
import LoginForm from '../components/LoginForm';
import Navbar from '../components/Navbar';
import { useNavigate } from 'react-router-dom'; // Import useNavigate

const LoginPage = () => {
  const navigate = useNavigate(); // Hook for navigation

  const navigateToRegister = () => {
    navigate('/register'); // Navigate to the register page
  };

  return (
    <div>
        <Navbar />
        <div className="row">
            <div className="col-md-12 text-center">
                <div id="centered-div">
                    <LoginForm />
                    <div className="register-link">
                        Not registered? <span onClick={navigateToRegister} style={{ cursor: 'pointer', color: 'blue' }}>Register here</span>.
                    </div>
                </div>
            </div>
        </div>
    </div>
  );
}

export default LoginPage;