using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
	public class GAction
	{
		public string Name;
		public float  cost;

		public Dictionary<string, float> preConditions;
		public Dictionary<string, float> postEffects;
	}
}