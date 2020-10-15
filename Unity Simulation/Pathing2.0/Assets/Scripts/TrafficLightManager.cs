/**
	@file TrafficLightManager.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/**
	This class holds the logic for the traffic light colour changer
*/
public class TrafficLightManager : NetworkBehaviour
{
    [SyncVar]
    public string currentColour;

    public GameObject greenLight;
    public GameObject orangeLight;
    public GameObject redLight;

    /*private void Start()
    {
        redLight.SetActive(false);
        orangeLight.SetActive(false);
        greenLight.SetActive(false);
    }*/

	/// Called every frame and sets correct colour of lights
    private void Update()
    {
        if (currentColour == "Red")
        {
            redLight.SetActive(true);
            orangeLight.SetActive(false);
            greenLight.SetActive(false);
        }
        else if (currentColour == "Orange")
        {
            redLight.SetActive(false);
            orangeLight.SetActive(true);
            greenLight.SetActive(false);
        }
        else if (currentColour == "Green")
        {
            redLight.SetActive(false);
            orangeLight.SetActive(false);
            greenLight.SetActive(true);
        }
    }

	/**
		<Setter that sets colour of light to the one passed in>
		@param Light colour is passed in
	*/
    public void changeLight(string colour)
    {
        currentColour = colour;
    }
}
