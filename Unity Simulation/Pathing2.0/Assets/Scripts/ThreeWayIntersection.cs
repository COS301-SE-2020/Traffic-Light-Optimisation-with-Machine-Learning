/**
	@file ThreeWayIntersection.cs
*/

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

/**
	This class holds the logic the Three way intersection
*/
public class ThreeWayIntersection : IntersectionParent
{
    /*Car Counters*/
    public GameObject inX;
    //public GameObject outX;
    public GameObject inZ1;
    public GameObject inZ2;
    //public GameObject outZ1;
    //public GameObject outZ2;

    /*Traffic Lights*/
    public GameObject tlX1;
    public GameObject tlZ1;
    public GameObject tlZ2;

    /*Prefab References*/
    [SerializeField]
    public GameObject prefabTLX1;
    [SerializeField]
    public GameObject prefabTLZ1;
    [SerializeField]
    public GameObject prefabTLZ2;

    /*Traffic Light Reg-Green cycle*/
    [SyncVar]
    private float timeOut = 16.0f;
    [SyncVar]
    private float timeOutBothRed = 4.0f;
    [SyncVar]
    private float timeLeft;
    [SyncVar]
    private float timeLeftBothRed;
    [SyncVar]
    public bool light_configruation = false;
    [SyncVar]
    bool isZ = false;
    [SyncVar]
    bool isX = false;
    [SyncVar]
    bool isXZ = false;
    [SyncVar]
    bool insideLightChange = false;
    [SyncVar]
    bool isMakeChange = false;
    
	/// Called upon initialization of the intersection
    void Start()
    {
        reset();
    }

	/// Resets the time back to 16 seconds and resets the traffic lights including the counter for the number of moving cars
    void reset()
    { 
        timeLeft = timeOut;
        timeLeftBothRed = timeOutBothRed;
        light_configruation = !light_configruation;
        changeLights();
        /*outX.GetComponent<OutgoingCounter>().reset();
        outZ1.GetComponent<OutgoingCounter>().reset();
        outZ2.GetComponent<OutgoingCounter>().reset();*/
        inX.GetComponent<IncomingCounter>().reset();
        inZ1.GetComponent<IncomingCounter>().reset();
        inZ2.GetComponent<IncomingCounter>().reset();
    }

    /**
		@return Returns an intersection object with the stationary and moving car values
	*/
    public override TrafficIntersection getIntersection()
    {
        TrafficIntersection intersection = new TrafficIntersection();
        if(isZ){
            intersection.stationaryX = inX.GetComponent<IncomingCounter>().getNumberCars();
        }else if(isX){
            //intersection.stationaryY += inZ1.GetComponent<IncomingCounter>().getNumberCars();
            //intersection.stationaryY += inZ2.GetComponent<IncomingCounter>().getNumberCars();
            intersection.stationaryY = (inZ1.GetComponent<IncomingCounter>().getNumberCars() + inZ2.GetComponent<IncomingCounter>().getNumberCars());
        }
        //intersection.movingX += outX.GetComponent<OutgoingCounter>().getNumberCars();
        //intersection.movingY += outZ1.GetComponent<OutgoingCounter>().getNumberCars();
        //intersection.movingY += outZ2.GetComponent<OutgoingCounter>().getNumberCars();
        intersection.movingX = inX.GetComponent<IncomingCounter>().getMovingCars();
        intersection.movingY = (inZ1.GetComponent<IncomingCounter>().getMovingCars() + inZ2.GetComponent<IncomingCounter>().getMovingCars());

        if (isX)
        {
            intersection.phase = 0;
        }
        else if (isZ)
        {
            intersection.phase = 1;
        }
        else
        {
            intersection.phase = 2;
        }

        return intersection;
    }

	/// Changes lights to the correct colours
    public void changeLights()
    {
        if(light_configruation)
        {
            tlX1.tag = "Green"; //Green
            prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Green");
            tlZ1.tag = "Car";
            prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Red");
            tlZ2.tag = "Car";
            prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Red");
            isZ = false;
            isX = true;
            isXZ = true;
        }
        else
        {
            tlX1.tag = "Car";
            prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Red");
            tlZ1.tag = "Green"; //Green
            prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Green");
            tlZ2.tag = "Green"; //Green
            prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Green");
            isZ = true;
            isX = false;
            isXZ = true;
        }
    }

    /// Called once per frame and makes sure all the lights are showing the correct colours
    void Update()
    {
        StartCoroutine(Waiter());
        if(isMakeChange)
        {
            StartCoroutine(APILightChange());
        }
    }

	/// Changes traffic lights to the correct intermediate colours (Red and orange)
    IEnumerator Waiter()
    {
        //reset();
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0f && isZ == true && insideLightChange == false)
        {
            //isX = false; isZ = false;
            isXZ = false;
            tlX1.tag = "Car";
            prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Red");
            tlZ1.tag = "Car"; //Orange
            prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Orange");
            tlZ2.tag = "Car"; //Orange
            prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Orange");
            timeLeftBothRed -= Time.deltaTime;
            if(timeLeftBothRed <= 0f)
            {
                reset();
            }
        }
        else if(timeLeft <= 0f && isZ == false && insideLightChange == false)
        {
            //isX = false; isZ = false;
            isXZ = false;
            tlX1.tag = "Car"; //Orange
            prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Orange");
            tlZ1.tag = "Car";
            prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Red");
            tlZ2.tag = "Car";
            prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Red");
            timeLeftBothRed -= Time.deltaTime;
            if(timeLeftBothRed <= 0f) 
            {
                reset();
            }
        }
        yield return null;
    }

	/// Resets time to the 16 seconds
    public override void updateTimeOut(float newTimeOut)
    {
        timeOut = newTimeOut;
    }

	/// Called when new data from the server is received
    public override void makeChange()
    {
        isMakeChange = true;
    }

	/// Called when the server sends data to update this intersection
    IEnumerator APILightChange()
    {
        if (!isXZ)
        {
            //nothing happens
            isMakeChange = false;
        }
        else if (isZ)
        {
            insideLightChange = true;
            tlX1.tag = "Car";
            prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Red");
            tlZ1.tag = "Car"; //Orange
            prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Orange");
            tlZ2.tag = "Car"; //Orange
            prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Orange");

            timeLeftBothRed -= Time.deltaTime;
            if (timeLeftBothRed <= 0f)
            {
                isMakeChange = false;
                reset();
            }
        }
        else if (!isZ)
        {
            insideLightChange = true;
            tlX1.tag = "Car"; //Orange
            prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Orange");
            tlZ1.tag = "Car";
            prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Red");
            tlZ2.tag = "Car";
            prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Red");

            timeLeftBothRed -= Time.deltaTime;
            if (timeLeftBothRed <= 0f)
            {
                isMakeChange = false;
                reset();
            }
        }
        yield return null;
    }
    
	/// Resets the appropriate counters when incrementing generations
    public override void resetGeneration()
    {
        inX.GetComponent<IncomingCounter>().resetGeneration();
        inZ1.GetComponent<IncomingCounter>().resetGeneration();
        inZ2.GetComponent<IncomingCounter>().resetGeneration();
    }
}
