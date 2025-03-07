using Poio.FiniteStateMachine.Agents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Poio.FiniteStateMachine
{
    [CreateAssetMenu(menuName = "Finite State Machine/New Enemy Interactive Script")]
    public class EnemyInteractiveScript_ScriptableObject : ScriptableObject
    {
        [Header("Runtime Patrol Behaviour")]
        [SerializeField] public List <PatrolScript> patrolScript;
    }
}