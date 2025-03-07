using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Poio.FiniteStateMachine.Agents
{
    #region Structs

    [System.Serializable] //takes part in Unity's ecosystem (inpsector)
    public struct PatrolScript
    {
        public Actions actionToExecute;
        public float speedOrTime;
            //IDLE: Time
            //WALK: Speed
            //ROTATE: Speed
        public Vector3 destinyVector;
            //IDLE: Does not use it
            //WALK: Uses it! ;)
            //ROTATE: Uses it! ;)
    }

    #endregion


    public class EnemyNPC : Agent
    {
        #region Knobs/Parameters

        //public PatrolScript dummy;
        //SUB STATE MACHINE ;)
        public EnemyInteractiveScript_ScriptableObject soPatrolScript;

        #endregion

        #region RuntimeVariables

        #region PatrolScript

        protected int currentPatrolIndex; //= 0 //int index;
        [SerializeField] protected PatrolScript currentPatrolScript;

        #endregion PatrolScript

        #region StopPatrol

        protected float _stopCronometer = 10000000; //-= Time.deltaTime; Update()

        #endregion StopPatrol

        #region RotatePatrol

        protected float _destinyAngle;
        protected float _signAngle; //1: right or -1: left
        protected Vector3 _v3DifferenceBetweenAngles;

        #endregion RotatePatrol

        #endregion RuntimeVariables

        #region UnityMethods

        private void Start()
        {
            InitializeAgent();
            //_fsm.OnMOVE(null);
        }

        private void FixedUpdate()
        {
            switch (currentPatrolScript.actionToExecute)
            {
                case Actions.STOP:
                    ExecutingStopPatrolSubState();
                    break;
                case Actions.WALK:
                    ExecutingWalkPatrolSubState();
                    break;
                case Actions.ROTATE:
                    ExecutingRotatePatrolSubState();
                    break;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (currentPatrolScript.actionToExecute == Actions.ROTATE)
            {
                CalculateRotationAngle();
            }
        }

        #endregion

        #region LocalMethods

        protected override void InitializeAgent()
        {
            base.InitializeAgent();
            //TODO: Configuration of the elements of the enemy

            //Start the SUB STATE MACHINE (Patrol Script)
            currentPatrolScript = soPatrolScript.patrolScript[0];
            if (soPatrolScript.patrolScript.Count <= 0)
            {
                //The Level Designer didn't assign any patrols
                //to the enemy
                //So we will leave it PERMANENTLY at the
                //IDLE state (via the STOP State Mecanic)
                currentPatrolScript.actionToExecute = Actions.STOP;
                currentPatrolScript.speedOrTime = -1.0f;
            }

            //Give a little cooldown for the Finite State Machine
            //to start the IDLE state
            Invoke("StartStateMechanic", 0.5f);
            //StartStateMechanic();
            //currentPatrolIndex = 0;
        }

        protected void StartStateMechanic()
        {
            _fsm.StateMechanic(currentPatrolScript.actionToExecute);
            //Start the sub state machine state
            switch (currentPatrolScript.actionToExecute)
            {
                case Actions.STOP:
                    InitializeStopPatrolSubState();
                    break;
                case Actions.WALK:
                    InitializeWalkPatrolSubState();
                    break;
                case Actions.ROTATE:
                    InitializeRotatePatrolSubState();
                    break;
            }
        }

        protected void GoToNextPatrolSubState()
        {
            //add one to the index, to jump to the next
            //element of the script
            currentPatrolIndex++;
            if (currentPatrolIndex >= soPatrolScript.patrolScript.Count)
            {
                return;
            }
            currentPatrolScript = soPatrolScript.patrolScript[currentPatrolIndex];
            Invoke("StartStateMechanic", 0.5f);
        }

        protected void CalculateRotationAngle()
        {
            if (Vector3.SignedAngle(
                transform.forward,
                currentPatrolScript.destinyVector,
                Vector3.up) > 0)
            {
                _signAngle = 1.0f;
            }
            else
            {
                _signAngle = -1.0f;
            }
            _fsm.SetSpeedAngle = _signAngle * currentPatrolScript.speedOrTime;
        }

        #endregion

        #region PublicMethods


        #endregion

        #region SubStateMachineMethods

        #region StopPatrolSubState

        protected void InitializeStopPatrolSubState()
        {
            _stopCronometer = currentPatrolScript.speedOrTime;
        }

        protected void ExecutingStopPatrolSubState()
        {
            //Method invoked by the Update()
            _stopCronometer -= Time.deltaTime;
            if (_stopCronometer <= 0.0f)
            {
                FinalizedStopPatrolSubState();
            }
        }

        protected void FinalizedStopPatrolSubState()
        {
            _stopCronometer = 1000000.0f;
            GoToNextPatrolSubState();
        }

        #endregion StopPatrolSubState

        #region MovePatrolSubState

        protected void InitializeWalkPatrolSubState()
        {
            _fsm.SetSpeedMovement = currentPatrolScript.speedOrTime;
        }

        protected void ExecutingWalkPatrolSubState()
        {
            //transform.LookAt(currentPatrolScript.destinyVector);
            _fsm.SetMovementInput = (currentPatrolScript.destinyVector - transform.position).normalized;
            transform.forward = _fsm.GetMovementInput;

            //<>
            if (Vector3.Distance(transform.position, currentPatrolScript.destinyVector) < .3f)
            {
                FinalizedWalkPatrolSubState();
            }
        }

        protected void FinalizedWalkPatrolSubState()
        {
            _fsm.SetSpeedMovement = 0.0f;
            _fsm.SetMovementInput = Vector3.zero;
            GoToNextPatrolSubState(); 

        }

        #endregion MovePatrolSubState

        #region RotatePatrolSubState

        protected void InitializeRotatePatrolSubState()
        {
            CalculateRotationAngle();
        }

        protected void ExecutingRotatePatrolSubState()
        {
            _v3DifferenceBetweenAngles = transform.rotation.eulerAngles - currentPatrolScript.destinyVector;

            //Debug.Log(gameObject.name + " EnemyNPC - ExecutingRotatePatrolSubState(): Difference between the vectors: " +
            //    _v3DifferenceBetweenAngles);

            //Debug.Log(gameObject.name + " EnemyNPC - ExecutingRotatePatrolSubState(): Transform rotation " +
            //    transform.rotation.eulerAngles.ToString());

            //Debug.Log(gameObject.name + " EnemyNPC - ExecutingRotatePatrolSubState(): Patrol Destiny Transform " +
            //    currentPatrolScript.destinyVector.ToString());

            if ( //10° tolerance
                    Mathf.Abs(_v3DifferenceBetweenAngles.x) <= 10.0f &&
                    Mathf.Abs(_v3DifferenceBetweenAngles.y) <= 10.0f &&
                    Mathf.Abs(_v3DifferenceBetweenAngles.z) <= 10.0f
                )
            {
                //Debug.Break();
                FinalizedRotatePatrolSubState();
            }
        }

        protected void FinalizedRotatePatrolSubState()
        {
            _fsm.StopRotationOfTheAgent();
            _fsm.SetSpeedAngle = 0.0f;
            GoToNextPatrolSubState();
        }

        #endregion RotatePatrolSubState

        #endregion SubStateMachineMethods

        #region GettersAndSetters


        #endregion
    }
}