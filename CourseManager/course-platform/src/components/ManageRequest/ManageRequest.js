import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './ManageRequest.css'
const ManageRequest = () => {
  const [requests, setRequests] = useState([]);
  const [active, setActive] = useState(true); // Default to active requests
  const token = localStorage.getItem('token');

  useEffect(() => {
    const endpoint = active ? 'https://localhost:7105/api/Request/activeRequests' : 'https://localhost:7105/api/Request/inactiveRequests';
    fetchRequests(endpoint);
  }, [active]);

  const fetchRequests = async (endpoint) => {
    try {
      const response = await axios.get(endpoint, { headers: { Authorization: `Bearer ${token}` } });
      setRequests(response.data);
    } catch (error) {
      console.error('Error fetching requests:', error);
      alert('Error fetching requests');
    }
  };

  const handleRequestAction = async (requestId, action) => {
    try {
      const endpoint = `https://localhost:7105/api/Request/${action}Request/${requestId}`;
      await axios.put(endpoint, {}, { headers: { Authorization: `Bearer ${token}` } });
      alert(`Request ${action === 'Accept' ? 'accepted' : 'rejected'} successfully`);
      fetchRequests(active ? 'https://localhost:7105/api/Request/activeRequests' : 'https://localhost:7105/api/Request/inactiveRequests');
    } catch (error) {
      console.error(`Error ${action.toLowerCase()}ing request:`, error);
      alert(`Error ${action.toLowerCase()}ing request`);
    }
  };

  return (
    <div>
        <h2>Manage Teaching Requests</h2>
        <button onClick={() => setActive(true)}>Active Requests</button>
        <button onClick={() => setActive(false)}>Inactive Requests</button>
        <div className="requests-container">
            {requests.map(request => (
                <div key={request.id} className="request-item">
                    <p><strong>Course:</strong> {request.courseName}</p>
                    <p><strong>Teacher:</strong> {request.senderUserName}</p>
                    <p><strong>Status:</strong> {request.requestStatus}</p>
            {active && (
              <>
                <button onClick={() => handleRequestAction(request.id, 'Accept')}>Accept</button>
                <button onClick={() => handleRequestAction(request.id, 'Reject')}>Reject</button>
              </>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default ManageRequest;