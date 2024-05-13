import React, { useState, useEffect } from 'react';
import axios from 'axios';

const LogRecords = () => {
  const [logs, setLogs] = useState([]);
  const [logType, setLogType] = useState('myLogs'); // Default to personal logs
  const token = localStorage.getItem('token');
  const userInfo = JSON.parse(localStorage.getItem('userInfo'));
  const isAdminOrOwner = userInfo.roles.includes('ADMIN') || userInfo.roles.includes('OWNER');

  useEffect(() => {
    const fetchLogs = async () => {
      let endpoint = 'https://localhost:7105/api/';

      if (isAdminOrOwner && logType === 'allLogs') {
        endpoint += 'Log'; // Endpoint for all logs
      } else {
        endpoint += 'Log/MyLogs'; // Endpoint for personal logs
      }

      try {
        const response = await axios.get(endpoint, {
          headers: { Authorization: `Bearer ${token}` }
        });
        setLogs(response.data);
      } catch (error) {
        console.error('Error fetching logs:', error);
      }
    };

    fetchLogs();
  }, [logType, token, isAdminOrOwner]);

  const renderLogs = () => {
    if (logs.length === 0) {
      return <div>No logs found.</div>;
    }

    return logs.map(log => (
      <div key={log.id} className="log-item">
        <p><strong>Username:</strong> {log.userName}</p>
        <p><strong>Description:</strong> {log.description}</p>
        <p><strong>Created At:</strong> {new Date(log.createdAt).toLocaleString()}</p>
      </div>
    ));
  };

  return (
    <div>
      <h2>Log Records</h2>
      {isAdminOrOwner && (
        <>
          <button onClick={() => setLogType('myLogs')}>My Logs</button>
          <button onClick={() => setLogType('allLogs')}>All Logs</button>
        </>
      )}
      {renderLogs()}
    </div>
  );
};

export default LogRecords;