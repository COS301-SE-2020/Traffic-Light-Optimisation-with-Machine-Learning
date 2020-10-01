/**
	@file CustomNetworkManagerHUD.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/**
	This class contains the custom network manager HUD we implemented, it inherits from the base network manager hud and adds extra functionality
*/
public class CustomNetworkManagerHUD : NetworkManagerHUD
{
	/// Called when started and sets up the UI
    void OnGUI()
    {
        if (!showGUI)
            return;

        GUILayout.BeginArea(new Rect(10 + offsetX, 30 + offsetY, 215, 9999));
        if (!NetworkClient.isConnected)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        // client ready
        if (NetworkClient.isConnected && !ClientScene.ready)
        {
            //if (GUILayout.Button("Client Ready"))
            //{
                ClientScene.Ready(NetworkClient.connection);

                if (ClientScene.localPlayer == null)
                {
                    ClientScene.AddPlayer(NetworkClient.connection);
                }
            //}
        }

        StopButtons();

        GUILayout.EndArea();
    }

	/// Contains the buttons for connecting to the two different simulations
    void StartButtons()
    {
        if (!NetworkClient.active)
        {
            TelepathyTransport tpt = GetComponent<TelepathyTransport>();
            // Client + IP
            //GUILayout.BeginHorizontal();
            if (GUILayout.Button("Join AI"))
            {
                //Add right ip with port
                manager.networkAddress = "142.93.139.199";
                tpt.port = 7777;
                manager.StartClient();
            }
            //manager.networkAddress = "142.93.139.199"/*GUILayout.TextField(manager.networkAddress)*/;
            //GUILayout.EndHorizontal();
            if (GUILayout.Button("Join no AI"))
            {
                //Add right ip with port
                manager.networkAddress = "142.93.139.199";
                tpt.port = 7778;
                manager.StartClient();
            }
        }
        else
        {
            // Connecting
            GUILayout.Label("Connecting to " + manager.networkAddress + "..");
            if (GUILayout.Button("Cancel Connection Attempt"))
            {
                manager.StopClient();
            }
        }
    }

	/// Method for displaying the status of the connection
    void StatusLabels()
    {
        if (NetworkClient.isConnected)
        {
            GUILayout.Label("Client: address=" + manager.networkAddress);
        }
    }

	/// Button for leaving the simulation
    void StopButtons()
    {
        if (NetworkClient.isConnected)
        {
            if (GUILayout.Button("Leave simulation"))
            {
                manager.StopClient();
            }
        }
    }
}
