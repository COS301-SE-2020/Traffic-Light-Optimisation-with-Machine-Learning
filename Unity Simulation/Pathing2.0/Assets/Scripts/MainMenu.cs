/**
    @file MainMenu
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/**
	This class contains the logic for the main menu
*/
public class MainMenu : MonoBehaviour
{
    //public GameObject startMenuCanvas;
    public GameObject simulationCanvas;
    public GameObject networkManagerObject;
    public GameObject mainMenuUI;

    public GameObject mainCamera;

    public GameObject connectingText;
    CustomNetworkManager networkManager;
    TelepathyTransport tpt;

    ControlCamera mainCameraControls;

    float connectingTimeRemaining = 2;
    bool connectingTimerIsRunning = false;

    public Transform startCameraPosition;
    public Transform endCameraPosition;

    public GameObject mainMenuLightingManagerObject;

    public GameObject aiIndicator;

    public GameObject noAiIndicator;

    bool joinedAi;


	/// Called upon initialization
    void Start()
    {
        networkManager = networkManagerObject.GetComponent<CustomNetworkManager>(); 
        tpt = networkManagerObject.GetComponent<TelepathyTransport>();
        mainCameraControls = mainCamera.GetComponent<ControlCamera>();
        mainCameraControls.DeTransition();
    }

	/// Resets the main menu camera to the original position
    public void Reset(){
        DeTransitionCamera();
    }
	
	/// Called every frame and manages the connecting text as well as check if the user is connected
    void Update(){

        if (connectingTimerIsRunning)
        {
            if (connectingTimeRemaining > 0)
            {
                connectingTimeRemaining -= Time.deltaTime;
            }
            else
            {
                connectingText.SetActive(false);
                connectingTimerIsRunning = false;
                connectingTimeRemaining = 2;

                //ClientScene.AddPlayer(NetworkClient.connection);
                /*connectingText.SetActive(false);
                simulationCanvas.SetActive(true);
                TransitionCamera();
                mainMenuUI.SetActive(false);
                if(joinedAi){
                    aiIndicator.SetActive(true);
                } else{
                    noAiIndicator.SetActive(true);
                }*/
            }
        }

        if (NetworkClient.isConnected && ClientScene.ready)
        {
           // ClientScene.Ready(NetworkClient.connection);

            if (ClientScene.localPlayer == null)
            {
                ClientScene.AddPlayer(NetworkClient.connection);
                connectingText.SetActive(false);
                simulationCanvas.SetActive(true);
                TransitionCamera();
                mainMenuUI.SetActive(false);
                if(joinedAi){
                    aiIndicator.SetActive(true);
                } else{
                    noAiIndicator.SetActive(true);
                }
            }
        }
    }

	/// Logic for connecting to the AI simulation
    public void JoinAI(){
        connectingText.SetActive(true);
        connectingTimerIsRunning = true;
        networkManager.networkAddress = "134.122.106.240";
        tpt.port = 7777;
        networkManager.StartClient();
        mainMenuLightingManagerObject.SetActive(false);
        joinedAi = true;
    }

	/// Logic for connecting to the No-AI simulation
    public void JoinNoAI(){
        connectingText.SetActive(true);
        connectingTimerIsRunning = true;
        networkManager.networkAddress = "134.122.106.240";
        tpt.port = 7778;
        networkManager.StartClient();
        mainMenuLightingManagerObject.SetActive(false);
        joinedAi = false;
    }

	/// Closes the application when the quit button is pressed
    public void QuitSimulation(){
        Application.Quit();
    }

	/// Logic for when the user returns to the main menu from the simulation
    public void LeaveSimulation(){
        networkManager.StopClient();
        mainMenuLightingManagerObject.SetActive(true);
        aiIndicator.SetActive(false);
        noAiIndicator.SetActive(false);
    }

	/// Transitions the camera to the end point for the simulation
    public void TransitionCamera(){
        mainCameraControls.Transition();
    }

	/// Transitions the camera back to the starting position to show main menu
    public void DeTransitionCamera(){
        mainCameraControls.DeTransition();
    }

    /*if (GUILayout.Button("Cancel Connection Attempt"))
    {
        manager.StopClient();
    }*/

    /*if (GUILayout.Button("Leave simulation"))
    {
        manager.StopClient();
    }*/

    /*if (NetworkClient.isConnected)
    {
        GUILayout.Label("Client: address=" + manager.networkAddress);
    }*/

    /*if (NetworkClient.isConnected && !ClientScene.ready)
    {
        //if (GUILayout.Button("Client Ready"))
        //{
            ClientScene.Ready(NetworkClient.connection);

            if (ClientScene.localPlayer == null)
            {
                ClientScene.AddPlayer(NetworkClient.connection);
            }
        //}
    }*/
}
