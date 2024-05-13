import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

function Logout() {
  const navigate = useNavigate();

  useEffect(() => {
    // Clear token and user info from localStorage
    localStorage.removeItem('token');
    localStorage.removeItem('userInfo');
    
    // Redirect to home or login page
    navigate('/login');
  }, [navigate]);

  return (
    <div>Logging out...</div>
  );
}

export default Logout;