using System.Collections.Generic;

namespace GOAP
{
	public class GNode
	{
		public GNode   next;
		public GAction action;
		public float fitness;

		public MetaVector state = new MetaVector();
	}
}