import React, { useState } from 'react';
import axios from 'axios';
import './RegisterCourse.css'
const RegisterCourse = () => {
  const [courseData, setCourseData] = useState({
    courseName: '',
    description: '',
    startDate: '',
    startTime: '',
    finishDate: '',
    finishTime: '',
    address: '',
    maxParticipants: ''
  });
  const token = localStorage.getItem('token');

  const handleChange = (e) => {
    setCourseData({ ...courseData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const combinedCourseData = {
      ...courseData,
      startDate: `${courseData.startDate}T${courseData.startTime}`,
      finishDate: `${courseData.finishDate}T${courseData.finishTime}`
    };

    try {
      const response = await axios.post('https://localhost:7105/api/Course/Create', combinedCourseData, {
        headers: { Authorization: `Bearer ${token}` }
      });
      console.log(response);
      if (response.StatusCode === 201) {
        alert('Course registered successfully');
     
      }
    } catch (error) {
      console.error('Error registering course:', error);
      alert('Error registering course');
    }
  };

  return (
    <div className="register-course">
      <h2>Register New Course</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          name="courseName"
          placeholder="Course Name"
          value={courseData.courseName}
          onChange={handleChange}
          required
        />
        <textarea
          name="description"
          placeholder="Description"
          value={courseData.description}
          onChange={handleChange}
          required
        />
        <input
          type="date"
          name="startDate"
          value={courseData.startDate}
          onChange={handleChange}
          required
        />
        <input
          type="time"
          name="startTime"
          value={courseData.startTime}
          onChange={handleChange}
          required
        />
        <input
          type="date"
          name="finishDate"
          value={courseData.finishDate}
          onChange={handleChange}
          required
        />
        <input
          type="time"
          name="finishTime"
          value={courseData.finishTime}
          onChange={handleChange}
          required
        />
        <input
          type="text"
          name="address"
          placeholder="Address"
          value={courseData.address}
          onChange={handleChange}
          required
        />
        <input
          type="number"
          name="maxParticipants"
          placeholder="Max Participants"
          value={courseData.maxParticipants}
          onChange={handleChange}
          required
        />
        <button type="submit">Register Course</button>
      </form>
    </div>
  );
};

export default RegisterCourse;