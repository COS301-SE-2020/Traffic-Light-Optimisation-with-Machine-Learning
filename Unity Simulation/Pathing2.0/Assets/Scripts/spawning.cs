/**
	@file spawning.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/**
	This class holds the logic for spawning cars
*/
[System.Serializable]
public class spawning : NetworkBehaviour{

    // Start is called before the first frame update

	[SerializeField]
	[Range(1f,200f)]
	public float speed;

    private GameObject car;

	/// Called upon initilization and spawns a car on the network
    IEnumerator Start()
    {	
    	while(true){
            car = Instantiate(GameObject.Find("NetworkManager").GetComponent<NetworkManager>().spawnPrefabs.Find(prefab => prefab.name == "car"));
            NetworkServer.Spawn(car);
            yield return new WaitForSeconds(60/speed);
        }
    }

    /// Called every frame although currently serves no purpose
    void Update()
    {
        
    }
}
