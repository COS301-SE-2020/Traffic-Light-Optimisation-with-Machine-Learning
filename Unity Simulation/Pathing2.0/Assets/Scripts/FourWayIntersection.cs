using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

public class FourWayIntersection : IntersectionParent
{
    /*Car Counters*/
    public GameObject inX1;
    public GameObject inX2;
    public GameObject inZ1;
    public GameObject inZ2;

    /*Traffic Lights*/
    public GameObject tlX1;
    public GameObject tlX2;
    public GameObject tlZ1;
    public GameObject tlZ2;

    /*Prefab References*/
    [SerializeField]
    public GameObject prefabTLX1;
    [SerializeField]
    public GameObject prefabTLX2;
    [SerializeField]
    public GameObject prefabTLZ1;
    [SerializeField]
    public GameObject prefabTLZ2;

    string prefabTLX1Colour;
    string prefabTLX2Colour;
    string prefabTLZ1Colour;
    string prefabTLZ2Colour;

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
    bool isXZ = false; // check if one red and other orange
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
        inX1.GetComponent<IncomingCounter>().reset();
        inX2.GetComponent<IncomingCounter>().reset();
        inZ1.GetComponent<IncomingCounter>().reset();
        inZ2.GetComponent<IncomingCounter>().reset();
    }

    /**
        getIntersection() - Returns traffic light object with updates data
    */
    public override TrafficIntersection getIntersection()
    {
        TrafficIntersection intersection = new TrafficIntersection();
        if(isZ){
            intersection.stationaryX = (inX1.GetComponent<IncomingCounter>().getNumberCars() + inX2.GetComponent<IncomingCounter>().getNumberCars());
        }else if(isX){
            intersection.stationaryY = (inZ1.GetComponent<IncomingCounter>().getNumberCars() + inZ2.GetComponent<IncomingCounter>().getNumberCars());
        }
    
        intersection.movingX = (inX1.GetComponent<IncomingCounter>().getMovingCars() + inX2.GetComponent<IncomingCounter>().getMovingCars());
        intersection.movingY = (inZ1.GetComponent<IncomingCounter>().getMovingCars() + inZ2.GetComponent<IncomingCounter>().getMovingCars());

        if (isX)
        {
            intersection.phase = 0;
        }
        else if(isZ)
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
        if (light_configruation)
        {
            tlX1.tag = "Green"; 
            prefabTLX1Colour = "Green";
            tlX2.tag = "Green"; 
            prefabTLX2Colour = "Green";
            tlZ1.tag = "Car";
            prefabTLZ1Colour = "Red";
            tlZ2.tag = "Car";
            prefabTLZ2Colour = "Red";
            isZ = false;
            isX = true;
        }
        else
        {
            tlX1.tag = "Car";
            prefabTLX1Colour = "Red";
            tlX2.tag = "Car";
            prefabTLX2Colour = "Red";
            tlZ1.tag = "Green"; 
            prefabTLZ1Colour = "Green";
            tlZ2.tag = "Green"; 
            prefabTLZ2Colour = "Green";
            isZ = true;
            isX = false;
        }
        isXZ = true;
    }

    /**
        Update() - Sets light colour and starts coroutines
    */
    void Update()
    {
        prefabTLX1.GetComponent<TrafficLightManager>().changeLight(prefabTLX1Colour);
        prefabTLX2.GetComponent<TrafficLightManager>().changeLight(prefabTLX2Colour);
        prefabTLZ1.GetComponent<TrafficLightManager>().changeLight(prefabTLZ1Colour);
        prefabTLZ2.GetComponent<TrafficLightManager>().changeLight(prefabTLZ2Colour);

        StartCoroutine(Waiter());
        if (isMakeChange)
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
                    prefabTLX1Colour = "Red";
                    prefabTLX2Colour = "Red";   
                    prefabTLZ1Colour = "Orange";   
                    prefabTLZ2Colour = "Orange";
                } else {
                    prefabTLX1Colour = "Orange";  
                    prefabTLX2Colour = "Orange";
                    prefabTLZ1Colour = "Red";
                    prefabTLZ2Colour = "Red";
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
        tlX2.tag = "Car";
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
                    prefabTLX1Colour = "Red";
                    prefabTLX2Colour = "Red";

                    prefabTLZ1Colour = "Orange"; 
                    prefabTLZ2Colour = "Orange";               
                } else{
                    prefabTLX1Colour = "Orange";    
                    prefabTLX2Colour = "Orange";

                    prefabTLZ1Colour = "Red";
                    prefabTLZ2Colour = "Red";
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
        inX1.GetComponent<IncomingCounter>().resetGeneration();
        inX2.GetComponent<IncomingCounter>().resetGeneration();
        inZ1.GetComponent<IncomingCounter>().resetGeneration();
        inZ2.GetComponent<IncomingCounter>().resetGeneration();
    }
}
