import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom'; 
import './CourseList.css';
function CourseList() {
  const [courses, setCourses] = useState([]);
  const defaultPhoto = 'https://archwaychandler.greatheartsamerica.org/wp-content/uploads/sites/9/2016/11/default-placeholder-250x250.png'; // URL of your default photo

  useEffect(() => {
    fetchCourses();
  }, []);

  const handleCourseClick = (courseId) => {
    navigate(`/course/${courseId}`); 
  };
  const navigate = useNavigate();


  const fetchCourses = async () => {
    try {
      const response = await axios.get('https://localhost:7105/api/course/gettitles');
      if (response.status === 200) {
        setCourses(response.data);
      }
    } catch (error) {
      console.error('Error fetching courses:', error);
    }
  };
  const formatDateTime = (dateString) => {
    const date = new Date(dateString);
    const options = { year: 'numeric', month: 'numeric', day: 'numeric', hour: '2-digit', minute: '2-digit' };
    return date.toLocaleDateString(undefined, options);
  };
  const formatTime = (dateString) => {
    const date = new Date(dateString);
    const options = { hour: '2-digit', minute: '2-digit' };
    return date.toLocaleTimeString(undefined, options);
  };
  const formatDate = (dateString) => {
    const date = new Date(dateString);
    const options = { 
        weekday: 'long', 
        year: 'numeric', 
        month: 'short', 
        day: 'numeric',
    };
    return date.toLocaleDateString('en-GB', options);
};

return (
  <div className="course-list">
    {courses.map(course => (
      <div className="course-item" key={course.courseId} onClick={() => handleCourseClick(course.courseId)}>
        <img src={course.photoUrl || defaultPhoto} alt={course.courseName} />
        <h3>{course.courseName}</h3>
        <p>Date: {formatDate(course.startDate)} at {formatTime(course.startDate)}</p>
      
      </div>
    ))}
  </div>
);
}

export default CourseList;