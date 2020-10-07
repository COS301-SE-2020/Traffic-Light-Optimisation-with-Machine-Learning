import { getUser } from '../utils/Common';
import React, { Component } from 'react';
import axios from 'axios';
import { Container, Button } from 'react-bootstrap';
import '../css/admin.css';
import { ReactComponent as BackIcon } from '../images/chevron-left-solid.svg';
import CodehesionLogo from '../images/codehesion_logo.png';
import AegisLogo from '../images/Aegis_logo.png';

class Admin extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            error: null,
            users: [],
            response: {}
        }
    }

    componentDidMount() {
        if (getUser() !== null) {
            axios.get('http://134.122.106.240:8080/api/getUsers')
                .then(response => {
                    //console.log(response.data);
                    this.setState({
                        users: response.data
                    })
                })
                .catch(error => {
                    console.log(error);
                });
        }
    }

    goBack = () => {
        this.props.history.push('/dashboard');
    }

    deleteUser(id) {
        const { users } = this.state;
        var r = window.confirm("Are you sure?");
        if (r === true) {
            axios.delete('http://134.122.106.240:8080/api/deleteUser/' + id)
                .then(result => {
                    //console.log(result.data);
                    this.setState({
                        response: result,
                        users: users.filter(user => user.id !== id)
                    });
                })
                .catch(error => {
                    this.setState({ error });
                });
        }
    }

    opUser(user_id, rid) {
        const { users } = this.state;
        var r = window.confirm("Are you sure?");
        if (r === true) {
            //console.log(getUser());
            rid = (rid == 1 ? 2 : 1);
            var admin = { username: getUser().username, password: getUser().password, id: user_id, role_Id: rid }
            //console.log(admin);
            axios.post('http://134.122.106.240:8080/api/changeRole', admin)
                .then(result => {
                    //console.log(result.data);
                    if (getUser() !== null) {
                        axios.get('http://134.122.106.240:8080/api/getUsers')
                            .then(response => {
                                //console.log(response.data);
                                this.setState({
                                    users: response.data
                                })
                            })
                            .catch(error => {
                                console.log(error);
                            });
                    }
                })
                .catch(error => {
                    this.setState({ error });
                });
        }
    }

    render() {
        const { error, users } = this.state;

        if (error) {
            return (
                <div>Error: {error.message}</div>
            )
        } else {
            return (
                <div>
                    <div className="topbar-wrapper">
                        <div className="topbar-back">
                            <div onClick={this.goBack}>
                                <BackIcon/>
                                <button variant="danger" onClick={this.goBack}>BACK</button>
                            </div>
                        </div>
                        <div className="topbar-title">
                            <div>TRAFFIC LIGHT OPTIMISATION WITH MACHINE LEARNING</div>
                        </div>
                    </div>
                    <div className="user-wrapper">
                        <h2>USER MANAGEMENT</h2>
                        <table className="table">
                            <thead>
                                <tr>
                                    <th>USERS</th>
                                    <th>ROLE</th>
                                    <th>ADMIN</th>
                                    <th>MANAGE</th>
                                </tr>
                            </thead>
                            <tbody>
                                {users.map((user) => (
                                    <tr key={user.id}>
                                        <td>{user.username}</td>
                                        <td>{user.role.name}</td>
                                        <td className="admin-btn"><Button onClick={() => this.opUser(user.id, user.role_Id)} variant={user.role.name == "admin" ? "light" : "success"}>{user.role.name == "admin" ? "Unassign" : "Assign"}</Button></td>
                                        <td><Button variant="danger" onClick={() => this.deleteUser(user.id)}>Delete</Button></td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                    <div className="footer">
                        <div className="footer-content">
                            <p>DEVELOPED BY</p>
                            <img src={AegisLogo}/>
                        </div>

                        <div className="footer-content">
                            <p>PROJECT BY</p>
                            <img src={CodehesionLogo}/>
                        </div>
                        <div className="footer-content">
                            <p>GITHUB</p>
                            <a href="https://github.com/COS301-SE-2020/Traffic-Light-Optimisation-with-Machine-Learning">https://github.com/COS301-SE-2020/Traffic-Light-Optimisation-with-Machine-Learning</a>
                        </div>
                    </div>
                </div>
            )
        }

    }

}

export default Admin;