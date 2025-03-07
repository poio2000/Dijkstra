using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poio.FiniteStateMachine
{
    public class FiniteStateMachine_StateMachineBehaviour : StateMachineBehaviour
    {
        public States nextState;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            FiniteStateMachine fsm = animator.gameObject.GetComponent<FiniteStateMachine>();
            if (fsm != null)
            {
                fsm.ChangeState(nextState);
            }
        }

    }
}