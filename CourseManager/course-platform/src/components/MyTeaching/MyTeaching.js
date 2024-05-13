import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './MyTeaching.css'
const MyTeaching = () => {
  const [requests, setRequests] = useState([]);
  const [requestType, setRequestType] = useState('active');

  useEffect(() => {
    const fetchRequests = async () => {
      const token = localStorage.getItem('token');
      let endpoint = 'https://localhost:7105/';
    
      switch (requestType) {
        case 'active':
          endpoint += 'api/Request/MyRequests';
          break;
        case 'accepted':
          endpoint += 'api/Request/MyAcceptedRequests';
          break;
          case 'rejected':
          endpoint += 'api/Request/MyRejectedRequests';
          break;
        default:
          return;
      }

      try {
        const response = await axios.get(endpoint, {
          headers: { Authorization: `Bearer ${token}` }
        });
        setRequests(response.data);
      } catch (error) {
        console.error(`Error fetching ${requestType} teaching requests:`, error);
      }
    };

    fetchRequests();
  }, [requestType]);

  const renderRequests = () => {
    if (requests.length === 0) {
      return <div>No {requestType} teaching requests found.</div>;
    }
  
    return requests.map(request => (
      <div key={request.id} className="request-item">
        <h3>{request.courseName}</h3>
        <p><strong>Description:</strong> {request.description}</p>
        <p><strong>Course ID:</strong> {request.courseId}</p>
        <p><strong>Teacher Name:</strong> {request.senderUserName}</p>
        <p><strong>Status:</strong> {request.requestStatus}</p>
        <p><strong>Submitted On:</strong> {new Date(request.createdAt).toLocaleDateString()}</p>
        
      </div>
    ));
  };

  return (
    <div>
      <h2>My Teaching Requests</h2>
      <button onClick={() => setRequestType('active')}>Pending Requests</button>
      <button onClick={() => setRequestType('accepted')}>Accepted Requests</button>
      <button onClick={() => setRequestType('rejected')}>Rejected Requests</button>
      {renderRequests()}
    </div>
  );
};

export default MyTeaching;