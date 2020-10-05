import React, { useState } from 'react';
import { setUserSession } from '../utils/Common';
import { validateCredentials } from '../utils/Validator';
import CryptoJS from 'crypto-js';
import { encrypt } from '../utils/Cryptography';
import axios from 'axios';
import { Button, Container, Card, Form } from 'react-bootstrap';

function Login(props) {
  const PASSWORD = "This is a password";
  const username = useFormInput('');
  const password = useFormInput('');
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
 
  // handle button click of login form
  const handleLogin = () => {
    setError(null);

    if (validateCredentials(username.value.trim(), password.value.trim())) {
      setLoading(true);
      var un = username.value.trim().toString();
      var pw = password.value.trim().toString();
      var iv = CryptoJS.lib.WordArray.random(128 / 8).toString(CryptoJS.enc.Hex);
      var salt = CryptoJS.lib.WordArray.random(128 / 8).toString(CryptoJS.enc.Hex);
      var ciphertext = encrypt(128, 1000, salt, iv, PASSWORD, pw);
      var aesPassword = (iv + "::" + salt + "::" + ciphertext);
      pw = btoa(aesPassword);
      var data = { username: un, password: pw }
      axios.post('http://161.35.167.228:8080/api/login', data).then(response => {
        setLoading(false);
        const data = response.data;
        setUserSession(''/*token*/, data);
        props.history.push('/dashboard');
      }).catch(error => {
        setLoading(false);
        console.log(error);
        setError("Invalid username or password.")
        //if(error.response.status === 401) setError(error.response.data.message);
        //else setError("Something went wrong. Please try again later.")
      });
    } else setError("Illegal characters or empty input fields.")


  }

  const Signup = () => {
    props.history.push("/register")
  }

  const handleKeyPress = (event) => {
    if(event.key === 'Enter'){
      handleLogin();
    }
  }

  return (
    <div>
      <div className="left-column">
        <div>
        </div>
      </div>
      <div className="right-column">
          <div>
            <h1 className="project-heading">Traffic Lights Optimisation with Machine Learning</h1>
            <div className="form-wrapper">
              <h1>Dashboard</h1>
              <h1>Sign in</h1>
              <form onKeyPress={handleKeyPress}>
                <div className="input-wrapper">
                  <div>
                    <label htmlFor="username">Username</label>
                    <input placeholder="Username" type="text" {...username} name="username" id="username" autoComplete="new-password" />
                  </div>
                </div>
                <div className="input-wrapper">
                  <div>
                    <label htmlFor="password">Password</label>
                    <input placeholder="Password" type="password" {...password} name="password" id="password" autoComplete="new-password" />
                  </div>
                </div>
                <div className="error-wrapper">
                {error && <><small style={{ color: 'red' }}>{error}</small></>}
                </div>
                <div className="button-wrapper">
                  <Button className="switch-btn" variant="link" onClick={Signup}>Create Account</Button>
                  <Button className="login-btn" variant="primary" onClick={handleLogin} disabled={loading}>{loading ? 'Loading...' : 'Login'}</Button>
                </div>
              </form>
            </div>
          </div>
      </div>
    </div>
  );
}

const useFormInput = initialValue => {
  const [value, setValue] = useState(initialValue);

  const handleChange = e => {
    setValue(e.target.value);
  }
  return {
    value,
    onChange: handleChange
  }
}

export default Login;