import React, { useEffect, useState } from 'react';
import axios from 'axios';
import './MyEnrollments.css'
const MyEnrollments = () => {
  const [courses, setEnrolledCourses] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchEnrolledCourses = async () => {
      setIsLoading(true);
      const token = localStorage.getItem('token');

      try {
        const response = await axios.get('https://localhost:7105/api/Course/enrolled-courses', {
          headers: { Authorization: `Bearer ${token}` }
        });

        setEnrolledCourses(response.data);
      } catch (error) {
        console.error('Error fetching enrolled courses:', error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchEnrolledCourses();
  }, []);

  if (isLoading) {
    return <div>Loading enrolled courses...</div>;
  }

  if (courses.length === 0) {
    return <div>You are not enrolled in any courses.</div>;
  }

  return (
    <div>
      <h2>Enrollments</h2>
      {courses.map(course => (
        <div key={course.id} className="enrollment-item">
          <h3>{course.courseName}</h3>
          <p>Description: {course.description}</p>
          <p>Start Date: {new Date(course.startDate).toLocaleDateString()}</p>
          <p>End Date: {new Date(course.finishDate).toLocaleDateString()}</p>
     
        </div>
      ))}
    </div>
  );
};

export default MyEnrollments;