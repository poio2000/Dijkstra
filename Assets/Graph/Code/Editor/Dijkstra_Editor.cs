//using SotomaYorch.Dijkstra;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Poio.Graph
{
    [CustomEditor(typeof(Dijkstra))]

    public class Dijkstra_Editor : Editor
    {
        #region RuntimeVariables

        protected Dijkstra _dijkstra;

        #endregion

        #region UnityMethods

        public override void OnInspectorGUI()
        {
            if (_dijkstra == null)
            {
                _dijkstra = (Dijkstra)target;
            }
            DrawDefaultInspector();

            if(GUILayout.Button("1 Probe nodes"))
            {
                _dijkstra.ProbeNodes();
            }
            if(GUILayout.Button("2 Create Graph (by connecting the nodes)"))
            {
                _dijkstra.ConnectNodes();
            }
            if (GUILayout.Button("3 Calculate all routes (and the best route to destiny)"))
            {
                _dijkstra.CalculateallRoutes();
            }
            if (GUILayout.Button("4 Calculate All Dijkstra steps"))
            {
                _dijkstra.CalculateAllDijkstraSteps();
            }
            if (GUILayout.Button("5 Clean all previous calculations"))
            {
                _dijkstra.CleanAllPreviousCalculations();
            }
        }

        #endregion
    }
}

