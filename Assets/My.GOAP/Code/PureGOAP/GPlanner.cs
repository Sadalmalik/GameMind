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

		public GNode FindPlan(MetaVector worldState, MetaVector target, int limit = 1000)
		{
			List<GNode> nodes = new List<GNode>();

			nodes.Add(
				new GNode
				{
					next   = null,
					action = null,
					fitness = MetaVector.SliceDistance(worldState, target),
					state  = worldState
				});
			
			GNode finalNode = null;

			while (limit --> 0 && nodes.Count > 0)
			{
				nodes.Sort((a,b) => a.fitness.CompareTo(b.fitness));
			
				var currentNode = nodes.Last();
				nodes.RemoveAt(nodes.Count-1);
				
				if (MetaVector.ContainsSlice(currentNode.state, target))
				{
					finalNode = currentNode;
					break;
				}

				var availableActions = GetAvailableActions(currentNode.state);

				foreach (var act in availableActions)
				{
					var state = currentNode.state - act.preConditions + act.postEffects;
					
					var node = new GNode
						{
							next   = currentNode,
							action = act,
							fitness = MetaVector.SliceDistance(state, target),
							state  = state
						};
					
					nodes.Add(node);
				}
			}

			return finalNode;
		}

		public List<GAction> GetAvailableActions(MetaVector target)
		{
			var availableActions = new List<GAction>();

			foreach (var action in actions)
				if (MetaVector.ContainsSlice(target, action.preConditions))
					availableActions.Add(action);

			return availableActions;
		}
	}
}