/**
	@file IncomingCounter.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
	This class contains the counting logic for counting cars entering an intersection
*/
public class IncomingCounter : MonoBehaviour
{
    /*bool started;
    public LayerMask layerMask;

    private Collider[] hitColliders;*/

    private int numStationaryCars;
    private int numMovingCars;

	/// Method is ran upon initialization
    void Start()
    {
        //started = true;
        numStationaryCars = 0;
        numMovingCars = 0;
    }

	/// Resets number of moving cars
    public void reset()
    {
        numMovingCars = 0;
    }

	/// Resets number of moving and stationary cars
    public void resetGeneration()
    {
        numStationaryCars = 0;
        numMovingCars = 0;
    }

	/**
		@return Returns the number of stationary cars
	*/
    public int getNumberCars(){
        //hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, layerMask);
        //Debug.Log("<color=blue> Number stat cars: "+hitColliders.Length+"</color>");
        //return hitColliders.Length;
        return numStationaryCars;
    }

	/**
		@return Returns the number of moving cars
	*/
    public int getMovingCars()
    {
        return numMovingCars;
    }

	/// Called when a car enters the collider at intersections and increments number of stationary cars of this intersection
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CountingTag"))
        {
            numStationaryCars += 1;
        }
    }

	/// Called when a car exits the collider at intersections and increments number of moving cars as well as decrements number of stationary cars of this intersection
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CountingTag"))
        {
            numStationaryCars -= 1;
            numMovingCars += 1;
        }
    }

    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (started){
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
            
    }*/
}
