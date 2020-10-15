/**
    @file RadioTowerLight
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
	This class handles the light atop the radio tower
*/
public class RadioTowerLight : MonoBehaviour
{
    public Light myLight;
    float lightRange = 0;
    bool middle = false;
    bool end = false;

    float middleTimer = 2f;

    float endTimer = 2f;

	/// Called upon initialization and starts the light flashing process
    void Start(){
        lightStart();
    }
	
	/// Called every update and handles changing the light intensity
    void Update()
    {
        //StartCoroutine(Flash());

        /*if(lightRange == 0.2f){
            
        } else{
            myLight.range = Mathf.Lerp(0f, 0.2f, Time.deltaTime);
            lightRange = myLight.range;
        }*/

        if (middle)
        {
            if (middleTimer > 0)
            {
                middleTimer -= Time.deltaTime;
            }
            else
            {
                /*allowMovement = true;
                movementTimerIsRunning = false;
                movementTimeRemaining = 4;*/
                lightEnd();
            }
        }

        if (end)
        {
            if (endTimer > 0)
            {
                endTimer -= Time.deltaTime;
            }
            else
            {
                /*allowMovement = true;
                movementTimerIsRunning = false;
                movementTimeRemaining = 4;*/
                lightStart();
            }
        }
    }

	/// Called first and starts the flashing process
    public void lightStart(){
        end = false;
        endTimer = 2f;

        //while(lightRange < 0.2f){
            myLight.intensity = Mathf.Lerp(0f, 1f, Time.deltaTime);
            lightRange = myLight.intensity;
        //}
        lightMiddle();
    }

	/// Called second and this triggers a timer that waits 2 seconds before starting the intensity detransition
    public void lightMiddle(){
        /*while(lightRange < 0.2f){
            myLight.range = Mathf.Lerp(0f, 0.2f, Time.deltaTime);
            lightRange = myLight.range;
        }
        lightEnd();*/
        middle = true;
    }

	/// Called third and detransitions the flashing process back to the beginning
    public void lightEnd(){
        middle = false;
        middleTimer = 2f;

        //while(lightRange > 0.0f){
            myLight.intensity = Mathf.Lerp(1f, 0f, Time.deltaTime);
            lightRange = myLight.intensity;
        //}
        lightEndTimer();
    }

	/// Called last and this triggers a timer that waits 2 seconds before starting the intensity again
    public void lightEndTimer(){
        /*while(lightRange > 0.0f){
            myLight.range = Mathf.Lerp(0.2f, 0f, Time.deltaTime);
            lightRange = myLight.range;
        }
        lightStart();*/
        end = true;
    }

    /*float totalSeconds = 2;     // The total of seconds the flash wil last
    float maxIntensity = 10;     // The maximum intensity the flash will reach
            // Your light

    public IEnumerator Flash()
    {
        float waitTime = totalSeconds / 2;                        
        // Get half of the seconds (One half to get brighter and one to get darker)
        while (myLight.intensity < maxIntensity) {
            myLight.intensity += Time.deltaTime / waitTime;        // Increase intensity
            yield return null;
        }
        while (myLight.intensity > 0) {
            myLight.intensity -= Time.deltaTime / waitTime;        //Decrease intensity
            yield return null;
        }
        yield return null;
    }*/
}
