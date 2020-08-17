import React from 'react';
import { getUser, removeUserSession } from '../utils/Common';
import '../css/dashboard.css';
import { ReactComponent as ChartIcon } from '../images/chart-line-solid.svg';
import { ReactComponent as UserIcon } from '../images/users-cog-solid.svg';
import { ReactComponent as LogoutIcon } from '../images/sign-out-alt-solid.svg';
import CodehesionLogo from '../images/codehesion_logo.png';
import AegisLogo from '../images/Aegis_logo.png';


function Dashboard(props) {
    const user = getUser();

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
                <div className="navbar-user">Hi, {user.username}</div>
                <div className="navbar-link-wrapper">
                    <div className="navbar-link" onClick={handleStatistics}>
                        <ChartIcon/>
                        <input type="button" value="VIEW STATISTICS"/>
                    </div>
                    {user.role.id === 1 && <div className="navbar-link"  onClick={handleAdmin}><UserIcon/><input type="button" value="MANAGE USERS"></input></div>}
                    <div className="navbar-link" onClick={handleLogout}>
                        <LogoutIcon/>
                        <input type="button" value="LOG OUT" />
                    </div>
                </div>
            </div>
            <div className="main">
                <div className="main-wrapper">
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

                    <div className="main-content">
                        <div className="main-title">
                            <div className="main-title-front">THE SIMULATION</div>
                            <div className="main-title-back">THE SIMULATION</div>
                        </div>
                        <p>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
                        Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata.</p>
                    </div>

                    <div className="main-content">
                        <div className="main-title">
                            <div className="main-title-front">ARTIFICIAL INTELLIGENCE</div>
                            <div className="main-title-back">ARTIFICIAL INTELLIGENCE</div>
                        </div>
                        <p>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
                        Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata.</p>
                    </div>

                    <div className="main-content">
                        <div className="main-title">
                            <div className="main-title-front">ABOUT US</div>
                            <div className="main-title-back">ABOUT US</div>
                        </div>
                        <p>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
                        Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata.</p>
                    </div>
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
        </div>
       
    );
}

export default Dashboard;