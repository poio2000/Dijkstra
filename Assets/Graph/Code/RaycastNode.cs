using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poio.Graph
{
    public class RaycastNode : MonoBehaviour
    {
        #region KnobsParameters

        public List<Vector3> startingPoints;

        #endregion

        #region RuntimeVariables

        [SerializeField] protected Material invalidNodeMaterial;
        [SerializeField] protected bool isTheNodeActive = true;

        #endregion

        #region PublicMethods

        public void RaycastToNode()
        {
            RaycastHit _rayH;
            foreach (Vector3 startingPoint in startingPoints)
            {
                Vector3 addStartingPoints = startingPoint + gameObject.transform.position;
                Vector3 direction = transform.position - addStartingPoints;
                float distance = direction.magnitude;
                //Using the layer mask parameter, we seek / search for this specific type of object
                //in this context, we are interested in searching for any feature from the labyrinth
                //to validate if the avatar may pass through this node while path finding
                if (Physics.Raycast(addStartingPoints, direction,out _rayH, distance, LayerMask.GetMask("Layout")))
                {
                    //The raycast has detected something from the labyrinth, so this
                    //node has to be discarded to take part in the graph
                    MeshRenderer objRenderer = GetComponent<MeshRenderer>();
                    objRenderer.material = invalidNodeMaterial;
                    isTheNodeActive = false;
                    //We do not have to continue to explore the for, since
                    //as one raycast found something from the labyrinth, this node is already discarde
                    break; //-> it stops the foreach
                    //return; //this ends the method
                }
            }
            //TODO: Pending
        }

        #endregion

        #region Getters

        public bool theNodeIsNotActive
        {
            get { return isTheNodeActive; }
        }

        #endregion
    }
}

