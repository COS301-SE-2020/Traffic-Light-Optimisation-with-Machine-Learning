import { getUser } from '../utils/Common';
import React, { Component, useEffect } from 'react';
import axios from 'axios';
import Chart from 'react-apexcharts';
import '../css/statistics.css';
import { ReactComponent as BackIcon } from '../images/chevron-left-solid.svg';
import CodehesionLogo from '../images/codehesion_logo.png';
import AegisLogo from '../images/Aegis_logo.png';

class Statistics extends Component {
    chartRef = React.createRef();
    showStat = true;
    //socket      = new WebSocket("ws://localhost:4444");; 
    constructor(props) {
        super(props);
        this.state = {
            //intersections: []
            ai: [],
            normal : []
        };
    }

    updateState(self) {
        if(this.showStat){
            console.log("updating");
            //var tl_objects = [];
            var int_objects = [];
            var ai_int_objects = [];
            var normal_int_objects = [];
            axios.get('http://134.122.106.240:8080/simu/getIntersections2')
                .then(response => {
                    var int_data        = response.data;
                    var ai_int_data     = int_data.ai;
                    var normal_int_data = int_data.normal;
                    ai_int_data.forEach((intersection, index) => {
                        var dataStationary = [];
                        var dataMoving = [];
                        var timestamps = [];
                        var i_name = intersection.name;
                        intersection.statistics.forEach((statistic, index) => {
                            const totalStationary = statistic.stationaryX + statistic.stationaryY;
                            const totalMoving = statistic.movingX + statistic.movingY;
                            const timestamp = Date.parse(statistic.timestamp);
    
                            dataStationary.push(totalStationary);
                            dataMoving.push(totalMoving);
                            timestamps.push(timestamp);
    
                        });
    
                        var int_object = {
                            name: i_name,
                            series: [{
                                name: "Stationary Vehicles",
                                data: dataStationary
                            },
                            {
                                name: "Moving Vehicles",
                                data: dataMoving
                            }
                            ],
                            options: {
                                legend: {
                                    labels: {
                                        colors: '#fff',
                                        useSeriesColors: false
                                    },
                                },
                                xaxis: {
                                    categories: timestamps,
                                    labels: {
                                        style: {
                                            colors: '#fff',
                                        },
                                    },
                                },
                                yaxis: [
                                    {
                                        title: {
                                            text: "Statioanry Vehicles",
                                            style: {
                                                color: '#d1d1d1'
                                            },
                                        },
                                        labels: {
                                            style: {
                                                colors: '#fff',
                                            },
                                        },
                                    },
                                    {
                                        opposite: true,
                                        title: {
                                            text: "Moving Vehicles",
                                            style: {
                                                color: '#d1d1d1'
                                            },
                                        },
                                        labels: {
                                            style: {
                                                colors: '#fff',
                                            },
                                        },
                                    },
                                ],
                            }
    
                        };
                        ai_int_objects.push(int_object);
                    });
    
                    normal_int_data.forEach((intersection, index) => {
                        var dataStationary = [];
                        var dataMoving = [];
                        var timestamps = [];
                        var i_name = intersection.name;
                        intersection.statistics.forEach((statistic, index) => {
                            const totalStationary = statistic.stationaryX + statistic.stationaryY;
                            const totalMoving = statistic.movingX + statistic.movingY;
                            const timestamp = Date.parse(statistic.timestamp);
    
                            dataStationary.push(totalStationary);
                            dataMoving.push(totalMoving);
                            timestamps.push(timestamp);
    
                        });
    
                        var int_object = {
                            name: i_name,
                            series: [{
                                name: "Stationary Vehicles",
                                data: dataStationary
                            },
                            {
                                name: "Moving Vehicles",
                                data: dataMoving
                            }
                            ],
                            options: {
                                legend: {
                                    labels: {
                                        colors: '#fff',
                                        useSeriesColors: false
                                    },
                                },
                                xaxis: {
                                    categories: timestamps,
                                    labels: {
                                        style: {
                                            colors: '#fff',
                                        },
                                    },
                                },
                                yaxis: [
                                    {
                                        title: {
                                            text: "Statioanry Vehicles",
                                            style: {
                                                color: '#d1d1d1'
                                            },
                                        },
                                        labels: {
                                            style: {
                                                colors: '#fff',
                                            },
                                        },
                                    },
                                    {
                                        opposite: true,
                                        title: {
                                            text: "Moving Vehicles",
                                            style: {
                                                color: '#d1d1d1'
                                            },
                                        },
                                        labels: {
                                            style: {
                                                colors: '#fff',
                                            },
                                        },
                                    },
                                ],
                            }
    
                        };
                        normal_int_objects.push(int_object);
                    });
    
                    const ai        = ai_int_objects;
                    const normal    = normal_int_objects;
                    //const intersections = { ai: ai_int_objects, normal: normal_int_objects };
                    //self.setState({ intersections });
                    self.setState({ ai: ai, normal: normal });
                    function sleep(ms) {
                        return new Promise(resolve => setTimeout(resolve, ms));
                    }
                    async function update() {
                        await sleep(5000);
                        self.updateState(self);
                    }
                    update();
    
                }).catch(error => {
                    console.log(error);
                });
        }
    }




    componentDidMount() {
        if (getUser() !== null) {
            this.updateState(this);

        }else{
            this.props.history.push("/login"); 
        }
        /*if (getUser() !== null) {
            this.updateState(); 
        }*/
    }

    componentWillUnmount() {
        // use intervalId from the state to clear the interval
    }

    goBack = () => {
        this.showStat = false;
        this.props.history.push("/dashboard");
    }

    render() {
        return (
            <div>
                <div className="topbar-wrapper">
                    <div className="topbar-back">
                        <div onClick={this.goBack}>
                            <BackIcon />
                            <button variant="danger" onClick={this.goBack}>BACK</button>
                        </div>
                    </div>
                    <div className="topbar-title">
                        <div>TRAFFIC LIGHT OPTIMISATION WITH MACHINE LEARNING</div>
                    </div>
                </div>

                <div className="stat-wrapper">
                    <h2>Statistics</h2>

                    <p className="stat-info">
                        The following graphs displays each intersections' moving and stationary vehicles in realtime, retrieved from the Unity simulation. 
                        Intersections' data from an optimisation artificial intelligence enabled traffic light simulation is displayed along side with data 
                        from a simulation without the optimisation aritificial intelligence.
                    </p>

                    <div className="scene-wrapper row"> 

                        <div className="col-12 col-md-6">
                        <h1 className="stat-type-title">Artificial Intelligence</h1>
                        {this.state.ai.map(function (trafficlight, index) {
                            return (
                                <div className="mixed-chart chart-wrapper" key={index}>
                                    <h4>{trafficlight.name}</h4>
                                    <Chart
                                        options={trafficlight.options}
                                        series={trafficlight.series}
                                        width="100%"
                                        height="300"
                                    />
                                </div>
                            )
                        })} 
                        </div>
                        <div className="col-12 col-md-6"> 
                        <h1 className="stat-type-title">Normal</h1>
                        {this.state.normal.map(function (trafficlight, index) {
                            return (
                                <div className="mixed-chart chart-wrapper" key={index}>
                                    <h4>{trafficlight.name}</h4>
                                    <Chart
                                        options={trafficlight.options}
                                        series={trafficlight.series}
                                        width="100%"
                                        height="300"
                                    />
                                </div>
                            )
                        })}
                        </div>
                        
                    </div>
                </div>

                <div className="footer">
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
        )
    }

}


export default Statistics;