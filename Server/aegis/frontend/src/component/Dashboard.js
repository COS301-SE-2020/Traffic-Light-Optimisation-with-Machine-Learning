import React from 'react';
import { getUser, removeUserSession } from '../utils/Common';
import '../css/dashboard.css';
import { ReactComponent as ChartIcon } from '../images/chart-line-solid.svg';
import { ReactComponent as UserIcon } from '../images/users-cog-solid.svg';
import { ReactComponent as LogoutIcon } from '../images/sign-out-alt-solid.svg';
import CodehesionLogo from '../images/codehesion_logo.png';
import AegisLogo from '../images/Aegis_logo.png';
import DashboardImage from '../images/dashboard.png';
import SimulationImage from '../images/simulation.png';
import MainImage from '../images/main.gif';
import BrandImage from '../images/Brand.png';


function Dashboard(props) {
    /*const user = getUser();
    if (user == null) {
        props.history.push("/login");
    } else {*/
        //handle click event of logout button
        const handleLogout = () => {
            removeUserSession();
            props.history.push('/');
        }

        const goBack = () => {
            props.history.goBack();
        }

        const handleAdmin = () => {
            props.history.push("/admin");
        }

        const handleStatistics = () => {
            props.history.push("/statistics");
        }

        return (
            <div>
                <div className="navbar-wrapper">
                    <div className="navbar-title">TRAFFIC LIGHT OPTIMISATION WITH MACHINE LEARNING</div>
                    {/* <div className="navbar-user">Hi, {user.username}</div> */}
                    <div className="navbar-link-wrapper">
                        <div className="navbar-link" onClick={handleStatistics}>
                            <ChartIcon />
                            <input type="button" value="VIEW STATISTICS" />
                        </div>
                        {/* {user.role.id === 1 && <div className="navbar-link" onClick={handleAdmin}><UserIcon /><input type="button" value="MANAGE USERS"></input></div>} */}
                        {/* <div className="navbar-link" onClick={handleLogout}>
                            <LogoutIcon />
                            <input type="button" value="LOG OUT" />
                        </div> */}
                    </div>
                </div>
                <div className="main">
                    <div className="main-wrapper container">

                        <div className="col-12 brand">
                            <img src={BrandImage} />
                        </div>

                        <div className="download-wrapper">
                            <h2>Download client</h2>
                            <div className="download-link-wrapper col-12">
                                <a href="/BuildForDownload/Windows.zip" download="Windows.zip"><div className="download-link">Windows</div></a>
                                <a href="/BuildForDownload/Mac.zip" download="Mac.zip"><div className="download-link">Mac</div></a>
                                <a href="/BuildForDownload/Linux_DebianBased.zip" download="Linux_DebianBased.zip"><div className="download-link">Linux</div></a>
                            </div>
                            <a href="https://docs.google.com/document/d/1x6ZiMAT8Qi2fKVTq8rmmD2MvIuykocylTufLMkb5HN8/edit#heading=h.35161rjx73iu" target="_blank" className="manuals"><div>Installation manual</div></a>
                            <a href="https://docs.google.com/document/d/1LhNsORokHXgobBF2hBluUihjAdw82lQBsTMyffnl3xo/edit#heading=h.v9vp2xrsc9by" target="_blank" className="manuals"><div>User manual</div></a>
                        </div>


                        <div className="main-content">
                            <div className="main-title">
                                <div className="main-title-front">ABOUT THE PROJECT</div>
                                <div className="main-title-back">ABOUT THE PROJECT</div>
                            </div>
                            <p>Traffic in big cities can quickly become an issue and in today's times having people reset traffic timers is just not cutting it anymore.
                            The idea involves creating a traffic optimisation system using machine learning, image processing and tracking to improve the flow of traffic
                            at various times of day, under various kinds of conditions. The aim of the project is to use artificial intelligence to affect traffic light states
                            to better control the flow of traffic in a Unity simulation representing real life traffic situations. </p>
                        </div>
                        <img className="banner" src={MainImage} />

                        <div className="main-content">
                            <div className="main-title">
                                <div className="main-title-front">THE DASHBOARD</div>
                                <div className="main-title-back">THE DASHBOARD</div>
                            </div>
                            <p>The dashboard provides a visual interface for the user to view the statistical value of each individual intersection that runs on the Unity simulation
                            in form of a graph. The graph provides nurmerical data of the amount of stationary and moving vehicles are at the specific intersection, showcasing the
                            efficiency of the artificial intelligence  and its impact on the traffic flow in the Unity simulation. The ideal scenario is when the moving vehicle value is
                            above the stationary vehicles to show that the flow of the traffic is in a good state.
                        </p>
                        </div>
                        <img className="banner" src={DashboardImage} />

                        <div className="main-content">
                            <div className="main-title">
                                <div className="main-title-front">THE SIMULATION</div>
                                <div className="main-title-back">THE SIMULATION</div>
                            </div>
                            <p>We decided to use the Unity game engine as our tool of choice to represent a real-life traffic scenario. The Unity game level communicates using WebRequests with an
                            API to send traffic data in real time to our custom built artificial intelligence, which sends back commands that affect the traffic light timings within the
                            game level to optimize the movement of cars. Traffic data for both the AI-simulation and Non-AI-simulation is represented in graph form on this website.
                        </p>
                        </div>
                        <img className="banner" src={SimulationImage} />

                        <div className="main-content">
                            <div className="main-title">
                                <div className="main-title-front">ARTIFICIAL INTELLIGENCE</div>
                                <div className="main-title-back">ARTIFICIAL INTELLIGENCE</div>
                            </div>
                            <p>For our custom built machine learning system - Deep Q-Learning, a kind of Reinforcement Learning - was used. The Unity Simulation, the environment,
                            sends it's state to the Reinforcement Learning Agent, the Aritifical Intelligence, where the Agent determines which action to take.
                            The Agent assesses the quality of the action and assigns a reward accordingly. In this way, the Agent learns which actions to take.</p>
                        </div>

                        <div className="main-content">
                            <div className="main-title">
                                <div className="main-title-front">ABOUT US</div>
                                <div className="main-title-back">ABOUT US</div>
                            </div>
                            <p>We are five University of Pretoria students, from three different degree programs: BSc Computer Science, BIS Multimedia, and BSc Information and Knowledge Systems. For more information on individual members,
                            take a look at the student links found on our team's GitHub page. (<a href="https://github.com/COS301-SE-2020/Traffic-Light-Optimisation-with-Machine-Learning">https://github.com/COS301-SE-2020/Traffic-Light-Optimisation-with-Machine-Learning</a>) </p>
                        </div>
                    </div>

                    <div className="footer container">
                        <div className="footer-content">
                            <p>DEVELOPED BY</p>
                            <img src={AegisLogo} />
                        </div>

                        <div className="footer-content">
                            <p>PROJECT BY</p>
                            <img src={CodehesionLogo} />
                        </div>
                        <div className="footer-content">
                            <p>GITHUB</p>
                            <a href="https://github.com/COS301-SE-2020/Traffic-Light-Optimisation-with-Machine-Learning">https://github.com/COS301-SE-2020/Traffic-Light-Optimisation-with-Machine-Learning</a>
                        </div>
                    </div>
                </div>
            </div>

        );
    /*}
    return (<div></div>);*/

}

export default Dashboard;