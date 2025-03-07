using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poio.Graph
{
    public class Node : MonoBehaviour
    {
        #region References

        [SerializeField] protected List<Connection> connections;
        [SerializeField] protected int nodesFound;

        #endregion

        #region UnityMethods

        private void OnDrawGizmos()
        {
            if(connections == null)
            {
                return;
            }

            foreach (Connection connection in connections)
            {
                if (connection.nodeA == null || connection.nodeB == null)
                {
                    return;
                }
                Debug.DrawLine(
                        connection.nodeA.transform.position,
                        connection.nodeB.transform.position,
                        Color.white,
                        0.01666666f
                    );
            }
        }

        #endregion

        #region PublicMethods

        public void ShootRaycastsLookingNodes()
        {
            if(gameObject.GetComponent<RaycastNode>().theNodeIsNotActive != false)
            {
                float angleBetweenRays = 360 / 8;

                Vector3 Direction = Vector3.forward;

                for (int i = 0; i < 8; i++)
                {
                    RaycastHit hit;
                    float rayDirectionAngle = angleBetweenRays * i;
                    Direction = RotateAngle(rayDirectionAngle, Direction);
                    if (Physics.Raycast(transform.position, Direction, out hit, 20))
                    {
                        if (hit.collider.gameObject.CompareTag("Node") && hit.collider.gameObject.GetComponent<RaycastNode>().theNodeIsNotActive != false 
                            && ContainsNodeInRoute(hit.collider.gameObject.GetComponent<Node>()) != true)
                        {
                            connections.Add(new Connection());
                            connections[nodesFound].nodeA = gameObject.GetComponent<Node>();
                            connections[nodesFound].nodeB = hit.collider.gameObject.GetComponent<Node>();
                            connections[nodesFound].distanceBetweenNodes = Vector3.Distance(gameObject.transform.position, hit.collider.gameObject.transform.position);
                            ++nodesFound;
                        }
                    }
                }
            }
        }

        Vector3 RotateAngle(float rotateAngleInDegrees, Vector3 Direction)
        {
            Vector3 rotation = Quaternion.AngleAxis(rotateAngleInDegrees, Vector3.up) * Direction;
            return rotation.normalized;
        }

        public bool ContainsNodeInRoute(Node value)
        {
            if(connections == null)
            {
                return false;
            }
            foreach (Connection connection in connections)
            {
                if (value == connection.nodeB)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Getters

        public List<Connection> GetConnections 
        {
            get { return connections; }
        }

        #endregion
    }
}

