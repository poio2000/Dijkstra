using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poio.FiniteStateMachine
{
    #region Enums

    public enum States
    {
        IDLE = 0,
        WALKING = 1,
        ROTATING = 2,
        //SIGHTING = 3 case in which the enemy RESPONDS when the avatar was sighted by him
    }

    public enum Actions //State Mechanic
    {
        ROTATE,
        WALK,
        STOP
    }

    public enum PlayerIndexes
    {
        ONE = 0,
        TWO = 1,
        THREE = 2,
        FOUR = 3
    }

    #endregion

    public class FiniteStateMachine : MonoBehaviour
    {
        //parameters which are displayed at the Inspector
        #region Knobs

        public PlayerIndexes playerIndex;
        //[SerializeField]
        //protected float value;

        #endregion

        #region References

        protected Animator _animator;
        protected Rigidbody _rigidbody;

        #endregion

        //variables which will operate during the life cycle of this script
        #region RuntimeVariables

        [SerializeField] protected States _state;
        protected Vector3 _movementInput;

        protected float _speedAngle; //includes direction and magnitude
        protected float _speedMovement;

        //protected float _dummy;

        #endregion

        #region UnityMethods

        private void Start()
        {
            //myState = States.IDLE;
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            //in which state are we?
            switch (_state)
            {
                case States.IDLE:
                    ExecutingIdleState();
                    break;
                case States.ROTATING:
                    ExecutingRotatingState();
                    break;
                case States.WALKING:
                    ExecutingWalkingState();
                    break;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {

        }

        #endregion

        #region FiniteStateMachineMethods

        #region IdleState

        virtual protected void InitializeIdleState()
        {
            //doing nothing
        }

        virtual protected void ExecutingIdleState()
        {
            //doing nothing
        }

        virtual protected void FinalizeIdleState()
        {
            //doing nothing
        }

        #endregion IdleState

        #region RotatingState

        virtual protected void InitializeRotatingState()
        {

        }

        virtual protected void ExecutingRotatingState()
        {
            _rigidbody.AddTorque(_speedAngle * Vector3.up,
                ForceMode.VelocityChange);
        }

        virtual protected void FinalizeRotatingState()
        {

        }

        #endregion RotatingState

        #region WalkingState

        protected void InitializeWalkingState()
        {

        }

        protected void ExecutingWalkingState()
        {
            //Movement Input belongs to the FSM,
            //wether the agent is an avatar or an NPC
            _rigidbody.velocity = _movementInput * _speedMovement;
        }

        protected void FinalizeWalkingState()
        {

        }

        #endregion WalkingState

        #endregion FiniteStateMachineMethods

        #region PublicMethods

        //method for the state machine to update the state in the Finite State Machine (code)
        public void ChangeState(States state)
        {
            _state = state;
            CleanAnimatorParameters();
        }

        public void StopRotationOfTheAgent()
        {
            _rigidbody.AddTorque(- _speedAngle * Vector3.up,
                ForceMode.VelocityChange);
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;
        }

        public void StateMechanic(Actions value)
        {
            _animator.SetBool(value.ToString(), true);
        }

        #endregion

        #region RuntimeMethods

        protected virtual void CleanAnimatorParameters()
        {
            if (_animator != null)
            {
                foreach (Actions action in Enum.GetValues(typeof(Actions)))
                {
                    _animator.SetBool(action.ToString(), false);
                }
            }
        }

        #endregion

        #region GettersAndSetters

        public float SetSpeedAngle
        {
            set { _speedAngle = value; }
        }

        public float SetSpeedMovement
        {
            set { _speedMovement = value; }
        }

        public Vector3 SetMovementInput
        {
            set { _movementInput = value; }
        }

        public Vector3 GetMovementInput
        {
            get { return _movementInput; }
        }

        public States GetState
        {
            get { return _state; }
        }

        #endregion
    }
}