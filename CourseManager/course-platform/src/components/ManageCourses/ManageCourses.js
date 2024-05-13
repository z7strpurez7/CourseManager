import React, { useState, useEffect } from 'react';
import axios from 'axios';
import RegisterCourse from './RegisterCourse'; // Ensure this component is correctly imported

const ManageCourses = () => {
  const [courses, setCourses] = useState([]);
  const [showRegisterCourse, setShowRegisterCourse] = useState(true);
  const token = localStorage.getItem('token');

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        const response = await axios.get('https://localhost:7105/api/Course/GetCourses', {
          headers: { Authorization: `Bearer ${token}` }
        });
        setCourses(response.data);
      } catch (error) {
        console.error('Error fetching courses:', error);
      }
    };

    if (!showRegisterCourse) {
      fetchCourses();
    }
  }, [showRegisterCourse, token]);

  return (
    <div>
      <h2>Manage Courses</h2>
      <button onClick={() => setShowRegisterCourse(true)}>Register New Course</button>
      <button onClick={() => setShowRegisterCourse(false)}>View All Courses</button>

      {showRegisterCourse ? (
        <RegisterCourse />
      ) : (
        <div className="courses-list">
          {courses.map(course => (
            <div key={course.id} className="course-item">
              <h3>{course.courseName}</h3>
              <p>Description: {course.description}</p>
              <p>Start Date: {new Date(course.startDate).toLocaleDateString()}</p>
              <p>End Date: {new Date(course.finishDate).toLocaleDateString()}</p>
              <p>Address: {course.address}</p>
              <p>Max Participants: {course.maxParticipants}</p>
              <p>Enrollments: {course.enrollments}</p>
              <p>Teacher: {course.teacherName || 'Not Assigned'}</p>
              <button disabled>Coming Soon (Edit)</button>
              <button disabled>Coming Soon (Delete)</button>
              <button disabled>Coming Soon (Set Teacher)</button>
              <button disabled>Coming Soon (view Enrollments)</button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ManageCourses;