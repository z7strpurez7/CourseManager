import React, { useState } from 'react';
import ManageUsers from '../../components/ManageUsers';
import MyTeaching from '../../components/MyTeaching/MyTeaching';
import MyEnrollments from '../../components/MyEnrollments/MyEnrollments';
import LogRecords from '../../components/LogRecords';
import ManageRequest from '../../components/ManageRequest/ManageRequest';
import ManageCourses from '../../components/ManageCourses/ManageCourses';
import Navbar from '../../components/Navbar';
import 'bootstrap/dist/css/bootstrap.css';
import './Dashboard.css';

function Dashboard() {
  const token = localStorage.getItem('token');
  const userInfo = JSON.parse(localStorage.getItem('userInfo'));
  const [activeComponent, setActiveComponent] = useState('default');

  if (!token) {
    return <div>You must be logged in to view this page.</div>;
  }

  const userRoles = userInfo ? userInfo.roles : [];

  const renderContent = () => {
    if (userRoles.includes('OWNER') || userRoles.includes('ADMIN')) {
      switch(activeComponent) {
        case 'manageUsers':
          return <ManageUsers />;
        case 'manageCourses':
          return <ManageCourses />;
        case 'manageRequests':
          return <ManageRequest />;
        case 'teachingRequests':
          return <MyTeaching />;
        case 'logs':
          return <LogRecords />;
        default:
          return <div>Welcome, Admin/Owner</div>;
      }
    } else if (userRoles.includes('LECTURER')) {
      switch(activeComponent) {
        case 'teachingRequests':
          return <MyTeaching />;
        case 'enrollments':
          return <MyEnrollments />;
        case 'logs':
          return <LogRecords />;
        default:
          return <div>Welcome, Lecturer</div>;
      }
    } else if (userRoles.includes('USER')) {
      switch(activeComponent) {
        case 'enrollments':
          return <MyEnrollments />;
        case 'logs':
          return <LogRecords />;
        default:
          return <div>Welcome, User</div>;
      }
    }
  };

  return (
    <div>
      <Navbar />
      <div className="dashboard">
        <div className="sidebar">
          {userRoles.includes('OWNER') || userRoles.includes('ADMIN') && (
            <>
              <button className="sidebar-button" onClick={() => setActiveComponent('manageUsers')}>Manage Users</button>
              <button className="sidebar-button" onClick={() => setActiveComponent('manageCourses')}>Manage Courses</button>
              <button className="sidebar-button" onClick={() => setActiveComponent('manageRequests')}>Manage Requests</button>
              <button className="sidebar-button" onClick={() => setActiveComponent('teachingRequests')}>Teaching Requests</button>
            </>
          )}

          {userRoles.includes('LECTURER') && (
            <>
              <button className="sidebar-button" onClick={() => setActiveComponent('teachingRequests')}>Teaching Requests</button>
              <button className="sidebar-button" onClick={() => setActiveComponent('enrollments')}>Enrollments</button>
            </>
          )}

          {userRoles.includes('USER') && (
            <button className="sidebar-button" onClick={() => setActiveComponent('enrollments')}>Enrollments</button>
          )}

          {/* Logs button at the bottom */}
          <div className="sidebar-bottom">
            <button className="sidebar-button" onClick={() => setActiveComponent('logs')}>Logs</button>
          </div>
        </div>

        <div className="main-content">
          {renderContent()}
        </div>
      </div>
    </div>
  );
}

export default Dashboard;