/**
	@file IntersectionParent.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/**
	This class is the base class all intersections inherit from
*/
public class IntersectionParent : NetworkBehaviour
{
	/// @return Returns null unless implemented to return an intersection object as is intended
    public virtual TrafficIntersection getIntersection()
    {
        return null;
    }

	/// Supposed to be implemented in inheriting classes to reset timer to the 16 seconds
    public virtual void updateTimeOut(float newTimeOut) 
    {

    }

	/// Supposed to be implemented in inheriting classes to be called when new data from the server is received
    public virtual void makeChange()
    {

    }
    
	/// Supposed to be implemented in inheriting classes to reset the appropriate counters when incrementing generations
    public virtual void resetGeneration()
    {

    }
}
