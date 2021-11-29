using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
	public class GAction
	{
		public string Name;
		public float  cost;

		public MetaVector preConditions;
		public MetaVector postEffects;
	}
}