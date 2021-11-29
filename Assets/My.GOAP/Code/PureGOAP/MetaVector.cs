using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOAP
{
	public class MetaVector : Dictionary<string, float>
	{
		public MetaVector()  {}

		public MetaVector(MetaVector other) : base(other) {}
		
		public float magnitude => Magnitude(this);
		
		public static float Magnitude(MetaVector state)
		{
			float magnitude = 0;

			foreach (var pair in state)
				magnitude += pair.Value * pair.Value;

			return Mathf.Sqrt( magnitude );
		}

		public static float SliceDistance(MetaVector state, MetaVector substate)
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

			return Mathf.Sqrt(distance);
		}
		
		public static float FullDistance(MetaVector state, MetaVector substate)
		{
			return Magnitude(state - substate);
		}

		public static MetaVector operator *(MetaVector state, float power)
		{
			var newVector = new MetaVector();

			foreach (var pair in state)
				newVector[pair.Key] = pair.Value * power;

			return newVector;
		}

		public static MetaVector operator +(MetaVector state, MetaVector substate)
		{
			var result = new MetaVector(state);

			foreach (var pair in substate)
			{
				if (!result.ContainsKey(pair.Key))
					result[pair.Key] = pair.Value;
				else
					result[pair.Key] += pair.Value;
			}

			return result;
		}

		public static MetaVector operator -(MetaVector state, MetaVector substate)
		{
			var result = new MetaVector(state);

			foreach (var pair in substate)
			{
				if (!result.ContainsKey(pair.Key))
					result[pair.Key] = -pair.Value;
				else
					result[pair.Key] -= pair.Value;
			}

			return result;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("( ");
			var first = true;
			foreach (var pair in this)
			{
				if (!first)
					sb.Append(", ");
				first = false;
				sb.Append(pair.Key);
				sb.Append(": ");
				sb.Append(pair.Value);
			}
			sb.Append(" )");
			return sb.ToString();
		}
	}
}