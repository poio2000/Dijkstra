using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poio.Graph
{
    [System.Serializable]
    public class Connection
    {
        #region References

        [SerializeField] public Node nodeA;
        [SerializeField] public Node nodeB;
        [SerializeField] public float distanceBetweenNodes;

        #endregion

        #region Public Methods

        public Node RetreiveOtherNodeThan(Node value) 
        {
                if (value == nodeA)
                {
                    return nodeB;
                }
                else
                {
                    return nodeA;
                }
        }

        #endregion
    }

    [System.Serializable]
    public class Route
    {
        [SerializeField] public List<Node> nodes;
        [SerializeField] public float sumDistance;  

        public Route() 
        {
            nodes = new List<Node>();
            sumDistance = 0;
        }

        public Route(List<Node> nodesToClone, float sumDistanceToCopy) 
        {
            nodes = new List<Node>();
            foreach (Node node in nodesToClone)
            {
                nodes.Add(node);
            }

            sumDistance = sumDistanceToCopy;
        }

        #region Public Methods

        public void AddNode(Node nodeValue, float sumValue) {
            nodes.Add(nodeValue);
            sumDistance += sumValue;
        }

        public bool ContainsNodeInRoute(Node value) 
        {
            foreach (Node node in nodes) 
            { 
                if(value == node) 
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
