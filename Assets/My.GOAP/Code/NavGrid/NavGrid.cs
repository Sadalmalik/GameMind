using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sadalmalik.GridNavigation
{
    public static class NavGrid
    {
        private class Node
        {
            public Node next = null;
            public float fitness = 0;
            public NavGridNode value = null;
        }
        
        public static List<NavGridNode> FindPath(NavGridNode start, NavGridNode end, int limit=10000)
        {
            foreach (var node in NavGridNode.AllNodes)
                node.Marked = false;
            
            var nodes = new List<Node>();
            nodes.Add(new Node{value = start, fitness = Fitness(start, end)});
            Node endNode = null;

            while (limit-->0 && nodes.Count>0)
            {
				nodes.Sort((a,b) => b.fitness.CompareTo(a.fitness));
            
                var last = nodes.Count - 1;
                var node = nodes[last];
                nodes.RemoveAt(last);
                
                node.value.Marked = true;
                
                if (node.value == end)
                {
                    endNode = node;
                    break;
                }

                foreach (var neibour in node.value.neibours)
                {
                    if (neibour.Marked)
                        continue;
                    
                    neibour.Marked = true;
                    nodes.Add(new Node
                    {
                        next = node,
                        value = neibour,
                        fitness = Fitness(neibour, end)
                    });
                }
            }
            
            var steps = new List<NavGridNode>();
            while (endNode != null)
            {
                steps.Add(endNode.value);
                endNode = endNode.next;
            }
            steps.Reverse();
            
            return steps;
        }
        
        private static float Fitness(NavGridNode node, NavGridNode target)
        {
            return Vector3.Distance(node.transform.position, target.transform.position);
        }
    }
}