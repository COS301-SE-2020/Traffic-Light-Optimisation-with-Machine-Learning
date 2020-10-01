/**
	@file OutgoingCounter.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
	This class holds the logic for the outgoing counter
*/
public class OutgoingCounter : MonoBehaviour
{
    //bool started;
    private int numMovingCars;
	
    /// Called upon initialization
    void Start()
    {
        //started = true;
        numMovingCars = 0;
    }

    /// Called every frame although currently serves no purpose
    void Update()
    {
        
    }

	/// Resets moving cars to 0
    public void reset(){
        numMovingCars = 0;
    }

	/**
		@return Returns number of moving cars
	*/
    public int getNumberCars(){
        return numMovingCars;
    }

    /*private void OnCollisionEnter(Collision other) {
        ++numMovingCars;
    }*/

	/// Called when car enters collider and increments moving cars
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("CountingTag"))
        {
            numMovingCars += 1;
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CountingTag"))
        {
            numMovingCars += 1;
        }
    }*/

    //Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this
    /*void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (started)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireCube(transform.position, transform.localScale);
    }*/
}
