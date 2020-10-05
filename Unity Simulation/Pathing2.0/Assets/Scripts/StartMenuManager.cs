/**
    @file StartMenuManager
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
	This class manages the start menu
*/
public class StartMenuManager : MonoBehaviour
{
    public GameObject startMenuObject;

    public GameObject simulationCanvasObject;
    MainMenu startMenu;

	/// Called upon initialization and sets the startMenu variable
    void Start(){
        startMenu = startMenuObject.GetComponent<MainMenu>();
    }
	
	/// This method handles leaving the simulation and starts the main menu back up
    public void LeaveSimulation(){
        startMenu.Reset();
        startMenu.LeaveSimulation();
        startMenuObject.SetActive(true);
        simulationCanvasObject.SetActive(false);
    }
}
