import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import './CoursePage.css'
import Navbar from '../../components/Navbar';

function CoursePage() {
  const [course, setCourse] = useState(null);
  const { courseId } = useParams();
  const navigate = useNavigate();

  useEffect(() => {
      const fetchCourse = async () => {
          try {
              const response = await axios.get(`https://localhost:7105/api/Course/${courseId}`);
              if (response.status === 200) {
                  setCourse(response.data);
              }
          } catch (error) {
              console.error('Error fetching course details:', error);
          }
      };

      fetchCourse();
  }, [courseId]);

  const handleAttendClick = async () => {
    const token = localStorage.getItem('token');
    if (!token) {
        navigate('/login');
        return;
    }

    try {
        const response = await axios.post(`https://localhost:7105/api/Enrollment/${courseId}`, {}, {
            headers: { Authorization: `Bearer ${token}` }
        });

        const responseData = response.data;

        if (responseData.isSuccess) {
            alert('Enrolled successfully, take a look in the dashboard');
        } else {
            // Handle different error scenarios based on the statusCode
            switch (responseData.statusCode) {
                case 409:
                    alert(responseData.message); // Already enrolled or max capacity reached
                    break;
                case 400:
                    alert(responseData.message); // Enrollment too close to start date
                    break;
                case 403:
                    alert('You are not authorized to enroll in this course.');
                    break;
                case 404:
                    alert('Course not found.');
                    break;
                default:
                    alert(responseData.message || 'Error enrolling in course');
            }
        }
    } catch (error) {
        console.error('Enrollment error:', error);
        alert('Error enrolling in course');
    }
};
const handleTeachClick = async () => {
  const token = localStorage.getItem('token');
  const userInfo = JSON.parse(localStorage.getItem('userInfo'));

  if (!token || !userInfo) {
      navigate('/login');
      return;
  }

  // Check if the user is a lecturer
  if (userInfo.roles.includes('LECTURER')) {
      try {
          // Send teaching request
          const response = await axios.post(`https://localhost:7105/api/Request/Create`, { courseId: courseId }, {
              headers: { Authorization: `Bearer ${token}` }
          });

          const responseData = response.data;

          if (responseData.isSuccess) {
              switch (responseData.statusCode) {
                  case 201:
                      alert('Teaching request sent successfully');
                      break;
                  case 400: 
                      alert('Teaching request already exists for this course');
                      break;
                  default:
                      alert('Operation completed with status code: ' + responseData.statusCode);
              }
          } else {
              alert(responseData.message || 'Error sending teaching request');
          }
      } catch (error) {
          console.error('Error sending teaching request:', error);
          alert('Error sending teaching request');
      }
  } else {
      alert('Only users with the LECTURER role can send teaching requests.');
  }
};

  if (!course) {
      return <div>Loading...</div>;
  }

  return (
      <div>
          <Navbar />
          <h1>{course.courseName}</h1>
          <p>Description: {course.description}</p>
          <p>Start Date: {new Date(course.startDate).toLocaleString()}</p>
          <p>Finish Date: {new Date(course.finishDate).toLocaleString()}</p>
          <p>Address: {course.address}</p>
          <p>Max Participants: {course.maxParticipants}</p>
          <p>Current Enrollments: {course.enrollments}</p>
          <p>Created At: {new Date(course.createdAt).toLocaleString()}</p>
          <p>Teacher: {course.teacherName}</p>
          <button onClick={handleAttendClick}>Attend</button>
          <button onClick={handleTeachClick}>Teach</button>
      </div>
  );
}

export default CoursePage;
