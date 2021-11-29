using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GOAP
{
	public class GTest : SerializedMonoBehaviour
	{
		public bool findPlan;
		public int searchLimnit=1000;
		[Space]
		public Dictionary<string, float> worldState;
		public Dictionary<string, float> targetState;
		[Space]
		public GPlanner planner;
		
		public void Update()
		{
			if (findPlan)
			{
				findPlan = false;
				
				FindPlan();
			}
		}
		
		private void FindPlan()
		{
			var finalNode = planner.FindPlan(worldState, targetState, searchLimnit);
			
			if (finalNode == null)
			{
				Debug.Log("No plan found!");
				return;
			}
			
			List<GNode>path = new List<GNode>();
			while (finalNode!=null)
			{
				path.Add(finalNode);
				finalNode = finalNode.next;
			}
			path.Reverse();
			
			var sb = new StringBuilder();
			sb.AppendLine("Found the plan:");
			foreach (var node in path)
			{
				sb.AppendLine($"node: '{node.action?.Name}'/{node.action?.cost ?? 0}\nstate:");
				GPlanner.DumpState(node.state, sb);
				sb.AppendLine();
			}
			
			Debug.Log(sb.ToString());
		}
	}
}