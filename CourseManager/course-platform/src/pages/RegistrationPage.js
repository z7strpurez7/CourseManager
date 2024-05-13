import RegistrationForm from '../components/RegistrationForm';
import { Outlet } from 'react-router-dom';
import React from 'react'
import Navbar from '../components/Navbar';
function RegistrationPage() {
  return (
    <div>
           <Navbar />
       <div className="container">
        <div className="row">
            <div className="col-md-12 text-center">
                <div id="centered-div">
                <RegistrationForm />
                   
                </div>
            </div>
        </div>
    </div>
    
      <Outlet />
    </div>
  );
}

export default RegistrationPage;