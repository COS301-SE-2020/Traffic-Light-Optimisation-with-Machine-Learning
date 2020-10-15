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
    private float defaultCycle = 16.0f;

    /**
        Start() - Reset called upon start of script
    */
    void Start()
    {
        reset();
    }

    /**
        reset() - Resets timers, sets light config flag, resets stationary vehicles stats
    */
    void reset()
    {
        timeLeft = timeOut;
        timeLeftBothRed = timeOutBothRed;
        light_configruation = !light_configruation;
        changeLights();
        inX.GetComponent<IncomingCounter>().reset();
        inZ1.GetComponent<IncomingCounter>().reset();
        inZ2.GetComponent<IncomingCounter>().reset();
    }

    /**
        getIntersection() - Returns traffic light object with updates data
		@return Returns an intersection object with the stationary and moving car values
	*/
    public override TrafficIntersection getIntersection()
    {
        TrafficIntersection intersection = new TrafficIntersection();
        if(isZ){
            intersection.stationaryX = inX.GetComponent<IncomingCounter>().getNumberCars();
        }else if(isX){
            intersection.stationaryY = (inZ1.GetComponent<IncomingCounter>().getNumberCars() + inZ2.GetComponent<IncomingCounter>().getNumberCars());
        }
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

        intersection.period = (float)Math.Floor(defaultCycle - timeLeft);
        if (timeLeft < 0.0f)
            intersection.period = defaultCycle;

        return intersection;
    }

    /**
        changeLights() - Changes light tags and updates light configuration flags
    */
    public void changeLights()
    {
        if(light_configruation)
        {
            tlX1.tag = "Green"; 
            prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Green");
            tlZ1.tag = "Car";
            prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Red");
            tlZ2.tag = "Car";
            prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Red");
            isZ = false;
            isX = true;
        }
        else
        {
            tlX1.tag = "Car";
            prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Red");
            tlZ1.tag = "Green"; 
            prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Green");
            tlZ2.tag = "Green"; 
            prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Green");
            isZ = true;
            isX = false;
        }
        isXZ = true;
    }

    /**
        Update() - Starts coroutines
    */
    void Update()
    {
        StartCoroutine(Waiter());
        if(isMakeChange)
        {
            StartCoroutine(APILightChange());
        }
    }

    /**
        Waiter() - Default traffic light changing logic (failsafe)
    */
    IEnumerator Waiter()
    {
        timeLeft -= Time.deltaTime;
        if(timeLeft <= 0f && !insideLightChange){
            if(timeLeftBothRed > 0.0f){
                if (isZ)
                {
                    prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Red");
                    prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Orange");
                    prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Orange");
                } else {
                    prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Orange");
                    prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Red");
                    prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Red");
                }
                isXZ = false;
                timeLeftBothRed -= Time.deltaTime;
                updateTrafficLightTagsToCar();
            } else {
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

    /**
        makeChange() - Sets changing flag
    */
    public override void makeChange()
    {
        isMakeChange = true;
    }

    /**
        updateTrafficLightTagsToCar() - Updates traffic light tags
    */
    private void updateTrafficLightTagsToCar(){
        tlX1.tag = "Car";
        tlZ1.tag = "Car";
        tlZ2.tag = "Car";
    }

    /**
        APILightChange() - Traffic light change logic
    */
    IEnumerator APILightChange()
    {
        if (!isXZ) // X-direction is red and Z-direction is orange
        {
            //nothing happens
            isMakeChange = false;
        }
        else 
        {
            insideLightChange = true;

            timeLeftBothRed -= Time.deltaTime;

            if (timeLeftBothRed > 0.0f)
            {
                updateTrafficLightTagsToCar();
                if (isZ){
                    prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Red");
                    prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Orange");
                    prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Orange");              
                } else{
                    prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Orange");
                    prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Red");
                    prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Red");
                }
            } else {
                isMakeChange = false;
                reset();
            }
        }
        yield return null;
    }
    
    /**
        resetGeneration() - Resets moving Cars
    */
    public override void resetGeneration()
    {
        inX.GetComponent<IncomingCounter>().resetGeneration();
        inZ1.GetComponent<IncomingCounter>().resetGeneration();
        inZ2.GetComponent<IncomingCounter>().resetGeneration();
    }
}
