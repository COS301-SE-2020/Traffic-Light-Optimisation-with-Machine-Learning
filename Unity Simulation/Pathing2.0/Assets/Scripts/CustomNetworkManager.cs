/**
	@file CustomNetworkManager.cs
*/

using System.Collections;
using UnityEngine;
using Mirror;

/**
	This class contains the custom network manager we implemented, it inherits from the base network manager and adds extra functionality
*/
public class CustomNetworkManager : NetworkManager
{
    /*public override void Start()
    {
        StartServer();
    }*/

	///Called when a new connection is made to the simulation
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        //car = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "car"));
        //NetworkServer.Spawn(car);
    }

	///Called when a conenction is terminated from simulation
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        /*if (car != null)
            NetworkServer.Destroy(car);*/

        base.OnServerDisconnect(conn);
    }
}
