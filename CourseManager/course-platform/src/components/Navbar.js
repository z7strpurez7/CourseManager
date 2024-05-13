import React from 'react';
import { Link } from 'react-router-dom';

function Navbar() {
  const isLoggedIn = !!localStorage.getItem('token'); // Check if the user is logged in

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light">
      <Link className="navbar-brand" to="/">Courses</Link>
      <div className="d-flex justify-content-end" style={{ width: "100%" }}>
        <ul className="navbar-nav">
          {isLoggedIn ? (
            <>
              <li className="nav-item">
                <Link className="nav-link" to="/dashboard">Dashboard</Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to="/logout">Logout</Link>
              </li>
            </>
          ) : (
            <>
              <li className="nav-item">
                <Link className="nav-link" to="/login">Login</Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to="/register">Register</Link>
              </li>
            </>
          )}
        </ul>
      </div>
    </nav>
  );
}

export default Navbar;