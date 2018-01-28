using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
	public List<Rail> rails;

    public Rail getNextRailOnArrive(Rail fromRail)
    {
        //If there's only one rail, go to the other rail:
        if (rails.Count == 2)
            return this.rails.Where(rail => rail != fromRail).ToList()[0];
        else
            throw new System.Exception("Haven't handled this case yet");
    }
}
