using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GOAP
{
	public class GPlanner
	{
		public List<GAction> actions;

		public GNode FindPlan(Dictionary<string, float> worldState, Dictionary<string, float> target, int limit = 1000)
		{
			List<GNode> nodes = new List<GNode>();

			nodes.Add(
				new GNode
				{
					next   = null,
					action = null,
					fitness = Distance(worldState, target),
					state  = worldState
				});
			
			GNode finalNode = null;

			while (limit --> 0 && nodes.Count > 0)
			{
				nodes.Sort((a,b) => a.fitness.CompareTo(b.fitness));
			
				var currentNode = nodes.Last();
				nodes.RemoveAt(nodes.Count-1);
				
				Debug.Log($"[TEST] node: '{currentNode.action?.Name}'/{currentNode.action?.cost}\nstate:\n{DumpState(currentNode.state)}");

				if (ContainsSubstate(currentNode.state, target, true))
				{
					finalNode = currentNode;
					break;
				}

				var availableActions = GetAvailableActions(currentNode.state);

				foreach (var act in availableActions)
				{
					var state = CombineStates(currentNode.state, act.preConditions, -1, true);
					state = CombineStates(state, act.postEffects, 1, true);
					
					var node = new GNode
						{
							next   = currentNode,
							action = act,
							fitness = Distance(state, target),
							state  = state
						};
					
					nodes.Add(node);
				}
			}

			return finalNode;
		}

		public List<GAction> GetAvailableActions(Dictionary<string, float> target)
		{
			var availableActions = new List<GAction>();

			foreach (var action in actions)
				if (ContainsSubstate(target, action.preConditions))
					availableActions.Add(action);

			return availableActions;
		}

		public bool ContainsSubstate(Dictionary<string, float> state, Dictionary<string, float> substate, bool dump = false)
		{
			var containsAll = true;
			foreach (var pair in substate)
			{
				if (!state.ContainsKey(pair.Key)
				 || state[pair.Key] < pair.Value)
					containsAll = false;
			}
			
			if (dump)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Compare two states.");
				sb.AppendLine("First:");
				DumpState(state, sb);
				sb.AppendLine("Second:");
				DumpState(substate, sb);
				sb.AppendLine($"Is first contains second: {containsAll}");
				Debug.Log(sb.ToString());
			}
			
			return containsAll;
		}
		
		public float Distance(Dictionary<string, float> state, Dictionary<string, float> substate)
		{
			return Distance(state, substate, false);
		}
		
		public float Distance(Dictionary<string, float> state, Dictionary<string, float> substate, bool dump)
		{
			float distance = 0;
			
			foreach (var pair in substate)
			{
				if (!state.ContainsKey(pair.Key))
				{
					var delta = pair.Value - state[pair.Key];
					distance += delta * delta;
				}
			}
			
			distance = Mathf.Sqrt(distance);
			
			if (dump)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine("Compare two states.");
				sb.AppendLine("First:");
				DumpState(state, sb);
				sb.AppendLine("Second:");
				DumpState(substate, sb);
				sb.AppendLine($"Distance: {distance}");
				Debug.Log(sb.ToString());
			}
			
			return distance;
		}

		public Dictionary<string, float> CombineStates(
			Dictionary<string, float> state,
			Dictionary<string, float> substate,
			float                     power,
			bool                      clampBottom = false)
		{
			var result = new Dictionary<string, float>(state);

			foreach (var pair in substate)
			{
				if (!result.ContainsKey(pair.Key))
					result[pair.Key] = power * pair.Value;
				else
					result[pair.Key] += power * pair.Value;
				if (clampBottom)
					result[pair.Key] = Mathf.Max(0, result[pair.Key]);
			}

			return result;
		}
		
		
		public static StringBuilder DumpState(Dictionary<string, float> state, StringBuilder sb=null)
		{
			if (sb==null)
				sb = new StringBuilder();

			foreach (var pair in state)
				sb.AppendLine($"\t{pair.Key}: {pair.Value}");
			
			return sb;
		}
	}
}