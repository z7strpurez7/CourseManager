import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate,  Outlet } from 'react-router-dom'; // Import useNavigate


function RegistrationForm() {
    const navigate = useNavigate();
    const navigateToLogin = () => {
      navigate('/login'); // Function to navigate to the login page
  };
    const [formData, setFormData] = useState({
      FirstName: '',
      LastName: '',
      UserName: '',
      Email: '',
      Password: '',
      Address: '',
      Birthday:'',
      Role: '',
    });


    const handleChange = (e) => {
      const { name, value } = e.target;
      setFormData({ ...formData, [name]: value });
    };
  
    const handleSubmit = async (e) => {
      e.preventDefault();
      const today = new Date();
      const birthDate = new Date(formData.Birthday);
      var age = today.getFullYear() - birthDate.getFullYear();
      const m = today.getMonth() - birthDate.getMonth();
      if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
      }
    
      if (age < 18) {
        alert('You must be at least 18 years old to register.');
        return;
      }
  
      try {
        const response = await axios.post('https://localhost:7105/api/auth/Register', formData);
        if (response.status === 201) {
          alert('Registration successful. You can now log in.');
          navigate('/login');
        }
      } catch (error) {
        if (error.response) {
          // Server responded with a status other than 2xx
          console.error('Registration error:', error.response);
          const errorMsg = error.response.data.message || 'Error, make sure password is 8 characters';
          alert(errorMsg); // Display error message
        } else if (error.request) {
          // The request was made but no response was received
          console.error('Registration error:', error.request);
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
        <h2>Register</h2>
        <form onSubmit={handleSubmit}>
          <div>
            <label htmlFor="FirstName">First Name:</label>
            <input
              type="text"
              name="FirstName"
              id="FirstName"
              placeholder="First Name"
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label htmlFor="LastName">Last Name:</label>
            <input
              type="text"
              name="LastName"
              id="LastName"
              placeholder="Last Name"
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label htmlFor="UserName">User Name:</label>
            <input
              type="text"
              name="UserName"
              id="UserName"
              placeholder="User Name"
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label htmlFor="Email">Email:</label>
            <input
              type="email"
              name="Email"
              id="Email"
              placeholder="Email"
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label htmlFor="Password">Password:</label>
            <input
              type="password"
              name="Password"
              id="Password"
              placeholder="Password"
              onChange={handleChange}
              required
            />
          </div>
          <div>
            <label htmlFor="Address">Address:</label>
            <input
              type="text"
              name="Address"
              id="Address"
              placeholder="Address"
              onChange={handleChange}
              required
            />
          </div>
          <div>
          <label htmlFor="Birthday">Birthday:</label>
          <input
            type="date" 
            name="Birthday" 
            id="Birthday" 
            onChange={handleChange}
            required
          />
             <div>
            <label htmlFor="Role">Role:</label>
            <select name="Role" id="Role" onChange={handleChange} value={formData.Role} required>
              <option value="">Select a role</option>
              <option value="USER">User</option>
              <option value="LECTURER">Lecturer</option>
              <option value="ADMIN">Admin</option>
            </select>
          </div>
        </div>
          <button type="submit">Register</button>
        </form>
        <div style={{ marginTop: '10px' }}>
            Already have a user? <span onClick={navigateToLogin} style={{ cursor: 'pointer', color: 'blue' }}>Login here</span>.
        </div>
        <Outlet />
      </div>
    );
  }
  
  export default RegistrationForm;