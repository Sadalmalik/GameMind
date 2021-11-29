using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Sadalmalik.GridNavigation
{
	public class NavGridNode : MonoBehaviour
	{
		public float radius;

		public List<NavGridNode> neibours;

		public bool Marked { get; set; } = false;

		public Vector3 position => transform.position;

		private static List<NavGridNode> _allNodes;

		public static List<NavGridNode> AllNodes
		{
			get
			{
				if (_allNodes == null)
					_allNodes = FindObjectsOfType<NavGridNode>().ToList();
				return _allNodes;
			}
		}
		
		public static void Refresh()
		{
			_allNodes = null;
		}

		public static NavGridNode GetNearestNode(Vector3 position)
		{
			return AllNodes
			      .OrderBy(n => Vector3.Distance(n.transform.position, position))
			      .First();
		}

#if UNITY_EDITOR

		[Sirenix.OdinInspector.Button]
		public void GetNeiboursInRadius()
		{
			var newNeibours = FindObjectsOfType<NavGridNode>()
			                 .Where(n => Vector3.Distance(position, n.position) < radius)
			                 .ToList();

			neibours.AddRange(newNeibours);
			neibours.Distinct();
			neibours.Remove(this);

			foreach (var neibour in neibours)
			{
				neibour.neibours.Add(this);
				neibour.neibours.Distinct();
			}
		}

		public void OnDrawGizmos()
		{
			var selected = Selection.objects.Contains(gameObject);

			var currentColor = Handles.color;
			Handles.color = selected ? Color.yellow : Color.gray;

			Handles.DrawWireDisc(transform.position, Vector3.up, radius);

			foreach (var neibour in neibours)
				Handles.DrawLine(position + (selected ? Vector3.up * 0.1f : Vector3.zero), neibour.position);

			Handles.color = currentColor;
		}

#endif
	}
}