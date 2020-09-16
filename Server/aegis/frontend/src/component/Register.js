import React, { useState } from 'react';
import axios from 'axios';
import CryptoJS from 'crypto-js';
import { validatePassword, validateCredentials } from '../utils/Validator';
import { encrypt } from '../utils/Cryptography';
import { Container, Card, Button, Form } from 'react-bootstrap';

function Register(props) {
    const PASSWORD = "This is a password";
    const username = useFormInput('');
    const password = useFormInput('');
    const confPassword = useFormInput('');
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(false);

    // handle button lick of register form
    const handleRegister = () => {
        setError(null);
        if (validateCredentials(username.value.trim(), password.value.trim()) && validatePassword(password.value.trim(), confPassword.value.trim())) {
            setLoading(true);
            var un = username.value.trim().toString();
            var pw = password.value.trim().toString();
            var iv = CryptoJS.lib.WordArray.random(128 / 8).toString(CryptoJS.enc.Hex);
            var salt = CryptoJS.lib.WordArray.random(128 / 8).toString(CryptoJS.enc.Hex);

            var ciphertext = encrypt(128, 1000, salt, iv, PASSWORD, pw);
            var aesPassword = (iv + "::" + salt + "::" + ciphertext);
            console.log("AES:", aesPassword);
            pw = btoa(aesPassword);


            var data = { username: un, password: pw }
            console.log(data);
            axios.post('http://142.93.139.199:8080/api/registerEnc', data).then(response => {
                setLoading(false);
                props.history.push('/');
            }).catch(error => {
                setLoading(false);
                setError("Can not use that username.");
            });
        } else {
            setLoading(false);
            setError("Passwords are empty or do not match.");
        }
    }

    const Login = () => {
        props.history.push("/")
    }

    const handleKeyPress = (event) => {
      if(event.key === 'Enter'){
        handleRegister();
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
              <h1>Sign up</h1>
              <form onKeyPress={handleKeyPress}>
                <div className="input-wrapper">
                  <div className="popper-wrapper-username" id="pw-u">
                    <div className="popper-header">Requirements</div>
                    <div className="popper-content">
                      <ul>
                        <li>Must contain 6 or more characters</li>
                        <li>Must be less than 12 characters</li>
                        <li>Must not contain spaces</li>
                      </ul>
                    </div>
                    <div className="arrow-down"></div>
                  </div>
                  <div>
                    <label htmlFor="username">Username</label>
                    <input type="text" placeholder="Username" {...username} name="username" id="username" onFocus={() => document.getElementById('pw-u').style.display = "block"} onBlur={() => document.getElementById('pw-u').style.display = "none"}/>
                  </div>
                </div>
                <div className="input-wrapper">
                  <div className="popper-wrapper-password" id="pw-p">
                    <div className="popper-header">Requirements</div>
                    <div className="popper-content">
                      <ul>
                        <li>Must contain 8 or more characters</li>
                        <li>Must be less than 20 characters</li>
                        <li>Must not contain spaces</li>
                      </ul>
                    </div>
                    <div className="arrow-down"></div>
                  </div>
                  <div>
                    <label htmlFor="password">Password</label>
                    <input type="password" placeholder="Password" {...password} name="password" id="password" onFocus={() => document.getElementById('pw-p').style.display = "block"} onBlur={() => document.getElementById('pw-p').style.display = "none"}/>
                  </div>
                </div>
                <div className="input-wrapper">
                  <div>
                    <label htmlFor="regEmail">Confirm Password</label>
                    <input  type="password" placeholder="Confirm Password" {...confPassword} name="confPassword" id="confPassword" />
                  </div>
                </div>
                <div className="error-wrapper">
                {error && <><small style={{ color: 'red' }}>{error}</small></>}
                </div>
                <div className="button-wrapper">
                  <Button className="switch-btn" variant="link" onClick={Login}>Sign in</Button>
                  <Button className="login-btn" variant="primary" onClick={handleRegister} disabled={loading}>{loading ? 'Loading...' : 'Sign up'}</Button>
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

export default Register;