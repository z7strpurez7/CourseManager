// LoginForm.js
import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function LoginForm() {
  const [credentials, setCredentials] = useState({ userName: '', password: '' });
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setCredentials({ ...credentials, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
        const response = await axios.post('https://localhost:7105/api/auth/login', credentials);

        if (response.status === 200) {
            localStorage.setItem('token', response.data.newToken); // Save token
            localStorage.setItem('userInfo', JSON.stringify(response.data.userInfo)); // Save user info
            navigate('/dashboard');
        }
    } catch (error) {
        if (error.response) {
            // Server responded with a status other than 2xx
            console.error('Login error:', error.response);
            const errorMsg = error.response.data || 'Error logging in';
            alert(errorMsg); // Display error message
        } else if (error.request) {
            // The request was made but no response was received
            console.error('Login error:', error.request);
            alert('No response from server');
        } else {
            // Something happened in setting up the request that triggered an Error
            console.error('Error:', error.message);
            alert('Error: ' + error.message);
        }
    }
};

  return (
    <div>
      <h2>Login</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          name="userName"
          placeholder="Username"
          value={credentials.userName}
          onChange={handleChange}
          required
        />
        <input
          type="password"
          name="password"
          placeholder="Password"
          value={credentials.password}
          onChange={handleChange}
          required
        />
        <button type="submit">Login</button>
      </form>
    </div>
  );
}

export default LoginForm;