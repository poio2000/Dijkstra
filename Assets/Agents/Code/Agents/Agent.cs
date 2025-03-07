using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poio.FiniteStateMachine.Agents
{
    #region Structs

    [System.Serializable]
    public struct MaterialForFeedback
    {
        [SerializeField] public Material idleMaterial;
        [SerializeField] public Material sightedMaterial;
    }

    #endregion

    [RequireComponent(typeof(FiniteStateMachine))]
    public class Agent : MonoBehaviour
    {
        #region Paramters

        [SerializeField] public MaterialForFeedback materialForFeedback;
        #endregion

        #region References

        [SerializeField, HideInInspector] protected FiniteStateMachine _fsm;
        [SerializeField, HideInInspector] protected MeshRenderer _meshRenderer;

        #endregion

        #region UnityMethods

        private void Start()
        {
            InitializeAgent();
        }

        #endregion

        #region LocalMethods

        protected virtual void InitializeAgent()
        {
            _fsm = GetComponent<FiniteStateMachine>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetSightedMaterial()
        {
            _meshRenderer.material = materialForFeedback.sightedMaterial;
        }

        public void SetIdleMaterial()
        {
            _meshRenderer.material = materialForFeedback.idleMaterial;
        }

        #endregion

        #region PublicMethods



        #endregion
    }
}