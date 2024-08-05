using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// PathDetectionManager.cs Class, Handles connected paths with the PowerSource Node by iterating and backtracking.
/// It uses DFS approach
/// </summary>
namespace LoopEnergyClone
{
    [RequireComponent(typeof(PowerSourceNode))]
    public class PathDetectionManager : MonoBehaviour
    {

        PowerSourceNode m_powerSourceNode = null; //PowerSource Node Reference
        GameManager m_gameManager; //GameManager Class Reference

        List<Node> m_connectedPath = new List<Node>(); //Cached Connected Path List

        public static Action OnLevelCompletion; //Level Completion Event

        public static bool IsDetectionRunning = false; //Validation, If nodes send multiple detection request


        private void Start()
        {
            m_powerSourceNode = GetComponent<PowerSourceNode>();
            m_gameManager = GameManager.Instance;

        }


        private void OnEnable()
        {
            Node.OnPathDetectionInvoke += OnPathDetectionInvoke;
        }


        private void OnDisable()
        {
            Node.OnPathDetectionInvoke -= OnPathDetectionInvoke;

        }

        /// <summary>
        /// Path Detection Invocation Method With Some Validations
        /// </summary>
        void OnPathDetectionInvoke()
        {
            if (IsDetectionRunning)
                return;

            IsDetectionRunning = true;

            //If PowerSource Node has multiple connecting points, So it'll run the seperate detection on each connect point
            foreach (ConnectPoint connectPoint in m_powerSourceNode._connectingPoints)
            {
                CalculateConnectedPath(connectPoint);
            }
        }

        /// <summary>
        /// Path Detection Logic
        /// </summary>
        /// <param name="startConnectPoint"></param>
        void CalculateConnectedPath(ConnectPoint startConnectPoint)
        {
            m_connectedPath.Clear();

            Queue<ConnectPoint> connectPointsQueue = new Queue<ConnectPoint>();
            connectPointsQueue.Enqueue(startConnectPoint);

            while (connectPointsQueue.Count > 0)
            {
                ConnectPoint connectPoint = connectPointsQueue.Dequeue();

                if (connectPoint.ConnectedToPoint != null)
                {
                    Node connectPointAssociatedNode = connectPoint.ConnectedToPoint.AssociatedNode;

                    foreach (var validConnectPoint in connectPointAssociatedNode._connectingPoints)
                    {
                        if (validConnectPoint == connectPoint.ConnectedToPoint)
                        {
                            continue;
                        }

                        connectPointsQueue.Enqueue(validConnectPoint);
                    }

                    if (!m_connectedPath.Contains(connectPointAssociatedNode))
                    {
                        m_connectedPath.Add(connectPointAssociatedNode);

                        //Bulb Sound Playing Logic
                        if (connectPointAssociatedNode is BulbSourceNode)
                        {
                            if (!m_gameManager.GetConnectedBulbsList().Contains(connectPointAssociatedNode))
                            {
                                AudioManager.Instance.PlaySFX("bulbglow");
                                m_gameManager.SetConnectedBulb(connectPointAssociatedNode as BulbSourceNode);
                            }
                        }

                    }

                }
            }


            var distinctNodes = m_gameManager.GetCurrentGameSessionNodes().Except(m_connectedPath);

            //Resetting highlighted nodes on disconnection by extracting distinct nodes
            foreach (var distinct in distinctNodes)
            {
                IHighlightable highlight = distinct.GetComponent<IHighlightable>();

                if (highlight != null)
                {
                    highlight.OnReset();
                }

                if (distinct is BulbSourceNode)
                {
                    if (m_gameManager.GetConnectedBulbsList().Contains(distinct))
                    {
                        m_gameManager.RemoveConnectedBulb(distinct as BulbSourceNode);
                    }
                }
            }



            //Setting highlight effect on nodes on connection
            foreach (var path in m_connectedPath)
            {
                IHighlightable highlight = path.GetComponent<IHighlightable>();
                if (highlight != null)
                {
                    highlight.OnHighlight(1f);
                }
            }


            //Calculating winning condition by checking the count of connected BulbSource Node
            int totalBulbNodes = m_connectedPath.Count((node) => node is BulbSourceNode);

            //Level Completion Event Invocation
            if (totalBulbNodes == m_gameManager.GetLevelCompletionBulbNodesCount())
            {
                OnLevelCompletion?.Invoke();
            }

            //Resetting Detection Validation
            IsDetectionRunning = false;

        }

    }
}
