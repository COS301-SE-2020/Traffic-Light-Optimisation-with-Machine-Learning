/**
	@file CommandCenter.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System.Text;
using System;

/**
	This class contains logic for the command center it handles the communication between the simlation and the AI on the server
*/
public class CommandCenter : MonoBehaviour
{
    IntersectionParent[] intersections;
    private float timeout = 8f;
    private float timeleft;
    private readonly static string localSpringServerURL = "http://127.0.0.1:8080/simu/addStatistics2"; //needs to be changed when we have a server setup

    [SerializeField]
    public JSONNode apiRequestInfo;

    [SerializeField]
    public string response;

    [SerializeField]
    public string json;

    /// The Start method is run when the command center is first initialized
    void Start()
    {
        reset();
    }

    /// The Update is called once per frame and every 8 seconds it sends data to the server
    void Update()
    {
        if (intersections == null || intersections.Length == 0)
        {
            scanIntersections();
        }
        timeleft -= Time.deltaTime;
        if(timeleft <= 0f)
        {
            reset();
            StartCoroutine(Upload());
        }
    } 

	/// Reset resets the time to be 8 seconds once it runs out
    void reset()
    {
        timeleft = timeout;
        ////Debug.Log("Reseting");
    }

	/// Initializes the intersections array and populates it with all the intersections in the simulation
    void scanIntersections()
    {
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Intersection");
        intersections = new IntersectionParent[tempArray.Length];
        for (int k = 0; k < tempArray.Length; k++)
        {
            intersections[k] = tempArray[k].GetComponent<IntersectionParent>();
        }
    }

	/// Upload is called every 8 seconds and sends data to the server
    IEnumerator Upload()
    {
        int i = 0;
        TrafficIntersection obj = null;
        String json = "{\"statistics\":[";
        for (; i < intersections.Length - 1; i++)
        {
            try
            {
                obj = intersections[i].getIntersection();
            }
            catch (NullReferenceException)
            {
                obj = intersections[i].getIntersection();
            }
            obj.name = "intersection" + (i+1);
            //obj.name = intersections[i].name;
            json += obj.toJson(i+1);
            json += ",";
        }
        ////Debug.Log("length: " + intersections.Length);
        ////Debug.Log("I: " + i);
        try
        {
            obj = intersections[i].getIntersection();
        } 
        catch (NullReferenceException)
        {
            obj = intersections[i].getIntersection();
        }
        obj.name = "intersection" + (i+1);
        //obj.name = intersections[i].name;
        json += obj.toJson(i+1);
        json += "],\"numStationaryCars\":"+ GameObject.Find("GlobalData").GetComponent<MetaData>().stopped+"}";
        //Debug.Log("Sending: " + json);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        UnityWebRequest apiRequest = UnityWebRequest.Put(localSpringServerURL, bytes);//.SetRequestHeader("content-type", "application/json" );
        apiRequest.method = "POST";
        apiRequest.SetRequestHeader("accept", "application/json; charset=UTF-8");
        apiRequest.SetRequestHeader("content-type", "application/json; charset=UTF-8");
        yield return apiRequest.SendWebRequest();

        if (apiRequest.isNetworkError || apiRequest.isNetworkError)
        {
            ////Debug.LogError(apiRequest.error);
            yield break;
        }
        else
        {
            String stringResponse = apiRequest.downloadHandler.text;
            // Example: 32 = 100000
            

            if (stringResponse == "-1")
            {
                GameObject[] carsArray = GameObject.FindGameObjectsWithTag("CarSpawned");
                for (int k = 0; k < carsArray.Length; k++)
                {
                    Destroy(carsArray[k]);
                }

                for (int k = 0; k < intersections.Length; k++)
                {
                    intersections[k].resetGeneration();
                }
            }
            else
            {
                int intResponse = Convert.ToInt32(stringResponse);
                String bitStream = Convert.ToString(intResponse, 2);

                //Debug.Log("Original bitstream: " + bitStream);

                //Debug.Log("Bitstream length: " + bitStream.Length);
                //Debug.Log("Intersections length: " + intersections.Length);

                if (bitStream.Length < intersections.Length)
                {
                    String temp = "";

                    int padAmount = (intersections.Length - bitStream.Length);

                    for (int k = 0; k < padAmount; k++)
                    {
                        temp += "0";
                    }

                    bitStream = temp += bitStream;
                }

                //Debug.Log("After padding: " + bitStream);

                Debug.Log("response: " + (string)apiRequest.downloadHandler.text);

                for (int j = 0; j < intersections.Length; j++)
                {
                    if (bitStream.ToCharArray().GetValue(j).Equals('1'))
                    {
                        Debug.Log("Making change to intersection: " + intersections[j].name);
                        intersections[j].makeChange();
                    }
                }
            }
        }
    }
}


[Serializable]
public class TrafficIntersection
{
    public float stationaryX, stationaryY, movingY, movingX;
    public string name;
    public Int32 phase;
    public TrafficIntersection(){
        this.stationaryX = 0;
        this.stationaryY = 0;
        this.movingX = 0;
        this.movingY = 0;
    }

    public string toJson(int id)
    {
        return "{\"name\":\"" + name + "\"," + 
                "\"phase\":" + phase + "," + //0-xGreen 1-zGreen 2-neitherGreen
                "\"intersection_Id\":" + id + "," +
                "\"stationaryY\":" + stationaryY + ","+
                "\"stationaryX\":" + stationaryX + ","+ 
                "\"movingY\":" + movingY + ","+
                "\"movingX\":" + movingX + "}";
    }
}