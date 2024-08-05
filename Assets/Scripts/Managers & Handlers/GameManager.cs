using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameManager.cs Class, Setting up and Resetting the GamePlay Current Session Data
/// </summary>

namespace LoopEnergyClone
{
    public class GameManager : GenericSingleton<GameManager>
    {
        GameObject[,] m_currentGameLevelGrid; //Current Level's Game Grid
        int m_currentGameLevel_Rows; //Current Level's Rows Count
        int m_currentGameLevel_Columns; //Current Level's Columns Count

        int m_levelCompletionBulbNodes; //Current Level's Available Bulbs  Count

        List<Node> m_currentGameSessionNodes = new List<Node>(); // Total Available Nodes In Current Game Level

        List<BulbSourceNode> m_currentGameSessionTotalBulbsConnected = new List<BulbSourceNode>(); // Reference of Captured Connected Bulbs


        /// <summary>
        /// Setting Up Current Level Nodes
        /// </summary>
        /// <param name="nodes"></param>
        public void SetCurrentGameSessionNodes(IEnumerable<Node> nodes)
        {
            m_currentGameSessionNodes.Clear();
            m_currentGameSessionNodes.AddRange(nodes);
        }

        /// <summary>
        /// Get Current Game Level Nodes
        /// </summary>
        /// <returns></returns>
        public List<Node> GetCurrentGameSessionNodes()
        {
            return m_currentGameSessionNodes;
        }

        /// <summary>
        /// Setting Up Current Level Grid along with Rows, Columns and Winning Condition
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        /// <param name="gameLevelGrid"></param>
        /// <param name="levelCompletionBulbNodes"></param>
        public void SetCurrentGameLevelGrid(int rows, int columns, GameObject[,] gameLevelGrid, int levelCompletionBulbNodes)
        {
            m_currentGameLevel_Rows = rows;
            m_currentGameLevel_Columns = columns;
            m_currentGameLevelGrid = gameLevelGrid;
            m_levelCompletionBulbNodes = levelCompletionBulbNodes;
        }


        /// <summary>
        /// Get Total Count of Winning Bulbs
        /// </summary>
        /// <returns></returns>
        public int GetLevelCompletionBulbNodesCount()
        {
            return m_levelCompletionBulbNodes;
        }

        /// <summary>
        /// Setting up Connected Bulbs in Current Session
        /// </summary>
        /// <param name="bulb"></param>
        public void SetConnectedBulb(BulbSourceNode bulb)
        {
            m_currentGameSessionTotalBulbsConnected.Add(bulb);
        }

        /// <summary>
        /// Remove Connected Bulb
        /// </summary>
        /// <param name="bulb"></param>
        public void RemoveConnectedBulb(BulbSourceNode bulb)
        {
            m_currentGameSessionTotalBulbsConnected.Remove(bulb);
        }

        /// <summary>
        /// Get Connected Bulbs
        /// </summary>
        /// <returns></returns>
        public List<BulbSourceNode> GetConnectedBulbsList()
        {
            return m_currentGameSessionTotalBulbsConnected;
        }

        /// <summary>
        /// Get Current Level Grid
        /// </summary>
        /// <returns></returns>
        public GameObject[,] GetCurrentGameLevelGrid()
        {
            return m_currentGameLevelGrid;
        }

        /// <summary>
        /// Setting Up Nearby Neighbours of a Node
        /// </summary>
        public void SetupNodesNeighbours()
        {
            for (int x = 0; x < m_currentGameLevel_Rows; x++)
            {
                for (int y = 0; y < m_currentGameLevel_Columns; y++)
                {
                    Transform cell = m_currentGameLevelGrid[x, y].transform;

                    if (cell.childCount > 0)
                    {
                        Node node = cell.GetChild(0).GetComponent<Node>();
                        node.UpdateTotalNeighboursConnectPoints(GetNeighboursConnectPoints(node));
                    }
                }
            }
        }


        /// <summary>
        /// Extracting Connecting Points of Nearby Neighbours
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        List<ConnectPoint> GetNeighboursConnectPoints(Node node)
        {

            List<ConnectPoint> neighboursConnectPointList = new List<ConnectPoint>();

            for (int x = -1; x <= 1; ++x)
            {
                for (int y = -1; y <= 1; ++y)
                {

                    if (x == y)
                        continue;

                    int newXIndex = node.XGridPosition + x;
                    int newYIndex = node.YGridPosition + y;

                    if (IsValidIndex(newXIndex, newYIndex))
                    {
                        GameObject cell = m_currentGameLevelGrid[newXIndex, newYIndex];

                        if (cell.transform.childCount > 0)
                        {
                            neighboursConnectPointList.AddRange(cell.transform.GetChild(0).GetComponent<Node>()._connectingPoints);
                        }
                    }

                }
            }
            return neighboursConnectPointList;
        }


        /// <summary>
        /// Index Validation
        /// </summary>
        /// <param name="xIndex"></param>
        /// <param name="yIndex"></param>
        /// <returns></returns>
        bool IsValidIndex(int xIndex, int yIndex)
        {
            return xIndex >= 0 && xIndex < m_currentGameLevelGrid.GetLength(0) &&
                yIndex >= 0 && yIndex < m_currentGameLevelGrid.GetLength(1);
        }

        /// <summary>
        /// Resetting Current Game Level
        /// </summary>
        /// <returns></returns>
        public bool DisposeCurrentLevel()
        {
            bool result = true;

            try
            {

                for (int x = 0; x < m_currentGameLevel_Rows; x++)
                {
                    for (int y = 0; y < m_currentGameLevel_Columns; y++)
                    {
                        if (m_currentGameLevelGrid[x, y] != null)
                        {
                            Destroy(m_currentGameLevelGrid[x, y]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                result = false;
            }
            finally
            {
                m_currentGameLevelGrid = null;
                m_currentGameLevel_Columns = 0;
                m_currentGameLevel_Rows = 0;
                m_levelCompletionBulbNodes = 0;

                m_currentGameSessionNodes.Clear();
                m_currentGameSessionTotalBulbsConnected.Clear();
            }

            return result;

        }



    }

}