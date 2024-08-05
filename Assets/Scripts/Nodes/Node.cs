using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Node.cs Class is responsible for all the Nodes related operation from connecting two nodes to updating all the valid lists.
/// </summary>

namespace LoopEnergyClone
{

    [RequireComponent(typeof(InputManager))]
    public abstract class Node : MonoBehaviour
    {
        [field: SerializeField] public string NodeUniqueID { get; set; } //Node Unique ID for Seperating Out Nodes
        public int GridCellAssociatedNumber { get; set; } // Grid Cell Associated Number 
        public int XGridPosition { get; set; } = -1; // X Position in Grid
        public int YGridPosition { get; set; } = -1; //Y Position in Grid

        public List<ConnectPoint> _connectingPoints = new List<ConnectPoint>(); // Connecting Points of a Node

        protected List<ConnectPoint> _validNeighboursConnectPointsList = new List<ConnectPoint>();  //Valid Nearby Neighbour Connecting Points 

        public static Action OnPathDetectionInvoke = null; // Path Detection Invocation Event On Each Tap

        protected bool IsNodeTapped { get; set; } = false; //Validation, A Node Can't be tapped multiple times at a single time.


        private void Update()
        {

            if (_connectingPoints.Count > 0 && _validNeighboursConnectPointsList.Count > 0)
                UpdateConnectedPoint();
        }

        /// <summary>
        /// Method to Detect the Two Nodes Connection
        /// </summary>
        protected void UpdateConnectedPoint()
        {

            for (int i = 0; i < _connectingPoints.Count; i++)
            {
                Vector2 nodeConnectPointLocation = _connectingPoints[i].transform.position;

                for (int j = 0; j < _validNeighboursConnectPointsList.Count; j++)
                {
                    Vector2 neighbourConnectPointLocation = _validNeighboursConnectPointsList[j].transform.position;


                    //Main Connection Logic by checking whether the distance is near and were facing each other
                    if ((Vector2.Distance(nodeConnectPointLocation, neighbourConnectPointLocation) <= 0.1f) &&
                    Vector2.Dot(nodeConnectPointLocation, neighbourConnectPointLocation) > 0)
                    {
                        _connectingPoints[i].ConnectedToPoint = _validNeighboursConnectPointsList[j];

                        _validNeighboursConnectPointsList[j].ConnectedToPoint = _connectingPoints[i];
                        break;
                    }
                    else
                    {

                        if (_connectingPoints[i].ConnectedToPoint != null)
                        {
                            _connectingPoints[i].ConnectedToPoint.ConnectedToPoint = null;
                            _connectingPoints[i].ConnectedToPoint = null;
                        }
                    }
                }

            }


        }

        /// <summary>
        /// Updating Nearby Valid Neighbour Connecting Points List
        /// </summary>
        /// <param name="connectPoints"></param>
        public void UpdateTotalNeighboursConnectPoints(IEnumerable<ConnectPoint> connectPoints)
        {
            _validNeighboursConnectPointsList.Clear();
            _validNeighboursConnectPointsList.AddRange(connectPoints);
        }

        /// <summary>
        /// Method for Performing Common Operation 
        /// </summary>
        /// <param name="rotateSpeed"></param>
        /// <param name="rotateAngle"></param>
        public abstract void OnTap(float rotateSpeed = 0f, float rotateAngle = 0f);

    }
}
