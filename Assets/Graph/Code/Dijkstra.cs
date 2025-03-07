using Poio.FiniteStateMachine;
using Poio.FiniteStateMachine.Agents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Poio.Graph
{
    public class Dijkstra : MonoBehaviour
    {
        #region References

        [SerializeField] protected Node initialNode;
        [SerializeField] protected Node finalNode;

        [SerializeField] protected List<Node> graph;
        

        [SerializeField] EnemyInteractiveScript_ScriptableObject dikjstraScriptableObject;



        #endregion

        #region Runtime Variables

        protected Route initialRoute;
        [SerializeField] protected List<Route> allroutes;
        [SerializeField] protected List<Route> succesfullRoutes;
        [SerializeField] protected Route theBestRoute;

        [SerializeField] protected GameObject nodePrefab;
        [SerializeField] protected int rows = 5;
        [SerializeField] protected int columns = 5;
        [SerializeField] protected float spacing = 1f; 

        #endregion

        #region GUILayoutButton

        public void ProbeNodes() 
        {
            int numeroDeNodo = 1;

            for (int x = 0; x < columns; x++)
            {
                for (int z = 0; z < rows; z++)
                {
                    Vector3 spawnPosition = new Vector3(x * spacing, 0, z * spacing);
                    GameObject nodeToPlace = Instantiate(nodePrefab, spawnPosition, Quaternion.identity);
                    nodeToPlace.transform.parent = transform;
                    graph.Add(nodeToPlace.GetComponent<Node>());
                    nodeToPlace.GetComponent<RaycastNode>().RaycastToNode();
                    nodeToPlace.gameObject.name = "Nodo " + numeroDeNodo;
                    numeroDeNodo++;
                }
            }
        }

        public void ConnectNodes()
        {
            foreach (Node node in graph)
            {
                node.ShootRaycastsLookingNodes();
            }
        }

        public void CalculateallRoutes() 
        {
            initialRoute = new Route();
            initialRoute.AddNode(initialNode, 0);

            allroutes = new List<Route>();
            allroutes.Add(initialRoute);

            ExploreBranchTree(initialRoute, initialNode);
            GetAllSuccesfullRoutes();
            GetBestRoute();
        }

        public void CalculateAllDijkstraSteps()
        {
            FillTheScriptableObject();
        }

        public void CleanAllPreviousCalculations()
        {
            foreach (Node node in graph)
            {
                DestroyImmediate(node.gameObject);
            }
            graph.Clear();
            allroutes.Clear();
            succesfullRoutes.Clear();
            theBestRoute.sumDistance = 100;
            theBestRoute.nodes.Clear();
            dikjstraScriptableObject.patrolScript.Clear();
        }

        #endregion

        #region LocalMethods

        protected void ExploreBranchTree(Route previousRoute, Node actualNodeToExplore) 
        {
            if(actualNodeToExplore == finalNode) 
            {
                return;
            } 
            else 
            {
                foreach (Connection connectionOfTheActualNode in actualNodeToExplore.GetConnections) {
                    Node nextNode = connectionOfTheActualNode.RetreiveOtherNodeThan(actualNodeToExplore);
                    if (!previousRoute.ContainsNodeInRoute(nextNode)) {
                        Route newRoute = new Route(previousRoute.nodes, previousRoute.sumDistance);
                        newRoute.AddNode(nextNode, connectionOfTheActualNode.distanceBetweenNodes);
                        allroutes.Add(newRoute);
                        ExploreBranchTree(newRoute, nextNode);
                    } 
                    
                }

            }
        }

        public void GetAllSuccesfullRoutes()
        {
            foreach(Route route in allroutes)
            {
                if (route.ContainsNodeInRoute(finalNode))
                {
                    succesfullRoutes.Add(route);
                }
            }
        }

        public void GetBestRoute()
        {
            foreach(Route route in succesfullRoutes)
            {
                if(route.sumDistance < theBestRoute.sumDistance)
                {
                    theBestRoute = route;
                }
            }
        }

        public void FillTheScriptableObject()
        {
            foreach(Node node in theBestRoute.nodes)
            {
                PatrolScript patrolScript;
                patrolScript.actionToExecute = Actions.WALK;
                patrolScript.speedOrTime = 5;
                patrolScript.destinyVector = node.transform.position;
                dikjstraScriptableObject.patrolScript.Add(patrolScript);
            }
        }

        #endregion
    }
}
