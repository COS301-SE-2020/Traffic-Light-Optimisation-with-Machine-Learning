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
    //public GameObject outX1;
    //public GameObject outX2;
    public GameObject inZ1;
    public GameObject inZ2;
    //public GameObject outZ1;
    //public GameObject outZ2;

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
    bool isXZ = false;
    [SyncVar]
    bool insideLightChange = false;
    [SyncVar]
    bool isMakeChange = false;
    private float defaultCycle = 16.0f;

    void Start()
    {
        reset();
    }

    void reset()
    {
        timeLeft = timeOut;
        timeLeftBothRed = timeOutBothRed;
        light_configruation = !light_configruation;
        changeLights();
        /*outX1.GetComponent<OutgoingCounter>().reset();
        outX2.GetComponent<OutgoingCounter>().reset();
        outZ1.GetComponent<OutgoingCounter>().reset();
        outZ2.GetComponent<OutgoingCounter>().reset();*/
        inX1.GetComponent<IncomingCounter>().reset();
        inX2.GetComponent<IncomingCounter>().reset();
        inZ1.GetComponent<IncomingCounter>().reset();
        inZ2.GetComponent<IncomingCounter>().reset();
    }

    public override TrafficIntersection getIntersection()
    {
        TrafficIntersection intersection = new TrafficIntersection();
        if(isZ){
            intersection.stationaryX = (inX1.GetComponent<IncomingCounter>().getNumberCars() + inX2.GetComponent<IncomingCounter>().getNumberCars());
        }else if(isX){
            intersection.stationaryY = (inZ1.GetComponent<IncomingCounter>().getNumberCars() + inZ2.GetComponent<IncomingCounter>().getNumberCars());
        }
        //intersection.movingX = (outX1.GetComponent<OutgoingCounter>().getNumberCars() + outX2.GetComponent<OutgoingCounter>().getNumberCars());
        //intersection.movingY = (outZ1.GetComponent<OutgoingCounter>().getNumberCars() + outZ2.GetComponent<OutgoingCounter>().getNumberCars());
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

    public void changeLights()
    {
        if (light_configruation)
        {
            tlX1.tag = "Green"; //Green
            //prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Green");
            prefabTLX1Colour = "Green";
            tlX2.tag = "Green"; //Green
            //prefabTLX2.GetComponent<TrafficLightManager>().changeLight("Green");
            prefabTLX2Colour = "Green";
            tlZ1.tag = "Car";
            //prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLZ1Colour = "Red";
            tlZ2.tag = "Car";
            //prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLZ2Colour = "Red";
            isZ = false;
            isX = true;
            isXZ = true;
        }
        else
        {
            tlX1.tag = "Car";
            //prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLX1Colour = "Red";
            tlX2.tag = "Car";
            //prefabTLX2.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLX2Colour = "Red";
            tlZ1.tag = "Green"; //Green
            //prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Green");
            prefabTLZ1Colour = "Green";
            tlZ2.tag = "Green"; //Green
            //prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Green");
            prefabTLZ2Colour = "Green";
            isZ = true;
            isX = false;
            isXZ = true;
        }
    }

    // Update is called once per frame
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

    IEnumerator Waiter()
    {
        //reset();
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f && isZ == true && insideLightChange == false)
        {
            //isX = false; isZ = false;
            isXZ = false;
            tlX1.tag = "Car";
            //prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLX1Colour = "Red";
            tlX2.tag = "Car";
            //prefabTLX2.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLX2Colour = "Red";
            tlZ1.tag = "Car";    //Orange
            //prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Orange");
            prefabTLZ1Colour = "Orange";
            tlZ2.tag = "Car";    //Orange
            //prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Orange");
            prefabTLZ2Colour = "Orange";
            timeLeftBothRed -= Time.deltaTime;
            if (timeLeftBothRed <= 0f)
            {
                reset();
            }
        }
        else if (timeLeft <= 0f && isZ == false && insideLightChange == false)
        {
            //isX = false; isZ = false;
            isXZ = false;
            tlX1.tag = "Car";    //Orange
            //prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Orange");
            prefabTLX1Colour = "Orange";
            tlX2.tag = "Car";    //Orange
            //prefabTLX2.GetComponent<TrafficLightManager>().changeLight("Orange");
            prefabTLX2Colour = "Orange";
            tlZ1.tag = "Car";
            //prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLZ1Colour = "Red";
            tlZ2.tag = "Car";
            //prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLZ2Colour = "Red";
            timeLeftBothRed -= Time.deltaTime;
            if (timeLeftBothRed <= 0f)
            {
                reset();
            }
        }
        yield return null;
    }

    public override void updateTimeOut(float newTimeOut)
    {
        timeOut = newTimeOut;
    }

    public override void makeChange()
    {
        isMakeChange = true;
    }

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
            //prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLX1Colour = "Red";
            tlX2.tag = "Car";
            //prefabTLX2.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLX2Colour = "Red";
            tlZ1.tag = "Car";   //Orange
            //prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Orange");
            prefabTLZ1Colour = "Orange";
            tlZ2.tag = "Car";   //Orange
            //prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Orange");
            prefabTLZ2Colour = "Orange";

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
            tlX1.tag = "Car";    //Orange
            //prefabTLX1.GetComponent<TrafficLightManager>().changeLight("Orange");
            prefabTLX1Colour = "Orange";
            tlX2.tag = "Car";    //Orange
            //prefabTLX2.GetComponent<TrafficLightManager>().changeLight("Orange");
            prefabTLX2Colour = "Orange";
            tlZ1.tag = "Car";
            //prefabTLZ1.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLZ1Colour = "Red";
            tlZ2.tag = "Car";
            //prefabTLZ2.GetComponent<TrafficLightManager>().changeLight("Red");
            prefabTLZ2Colour = "Red";

            timeLeftBothRed -= Time.deltaTime;
            if (timeLeftBothRed <= 0f)
            {
                isMakeChange = false;
                reset();
            }
        }
        yield return null;
    }
    
    public override void resetGeneration()
    {
        inX1.GetComponent<IncomingCounter>().resetGeneration();
        inX2.GetComponent<IncomingCounter>().resetGeneration();
        inZ1.GetComponent<IncomingCounter>().resetGeneration();
        inZ2.GetComponent<IncomingCounter>().resetGeneration();
    }
}
