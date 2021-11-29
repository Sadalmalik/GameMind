using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Miner : Labourer
{
	public override HashSet<KeyValuePair<string,object>> createGoalState () {
		HashSet<KeyValuePair<string,object>> goal = new HashSet<KeyValuePair<string,object>> ();
		
		goal.Add(new KeyValuePair<string, object>("collectOre", true ));
		return goal;
	}

}

