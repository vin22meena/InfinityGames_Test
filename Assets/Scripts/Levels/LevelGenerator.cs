using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// LevelGenerator.cs Class, Takes care of generating Levels JSON Data and Loading of JSONs data
/// </summary>

namespace LoopEnergyClone
{
    public class LevelGenerator : GenericSingleton<LevelGenerator>
    {

        [Header("Level JSON Generation Settings")]

        [SerializeField] string levelName; //Level Name, Which can be alter on each level generation
        [SerializeField] int levelNumber; //Level Number, Which can be alter on each level genereation
        [SerializeField] int levelCompletionBulbNodes; //Total Bulbs Required for Level Completion

        [SerializeField] GridLayoutGroup levelGridLayoutGroup;  //Level Grid Layout Group
        [SerializeField] string saveFolderName; //Saving Folder Name into Assets
        [SerializeField] Vector2Int levelGridDimension; //Rows and Columns of Grid
        LevelStructureDataClass m_levelStructureData = null; //Level JSON Data Storage Serializable Class



        [Header("Level Instantiation Settings")]
        [SerializeField] GameObject cellPrefab; //Temp Cell Prefab as a parent of node
        [SerializeField] List<NodesDataStorageClass> nodesDataStorageList = new List<NodesDataStorageClass>();  //In Game Available Nodes Reference List

        /// <summary>
        /// Member Fields Validation Method
        /// </summary>
        private void OnValidate()
        {
            if (levelNumber <= 0)
            {
                Debug.Log("Level Number Should be greater that 0");
                levelNumber = 1;
            }

            if (string.IsNullOrEmpty(levelName))
            {
                Debug.Log("Level Name Shouldn't be null or empty");
                levelName = "LoopEnergy";
            }

            if (levelCompletionBulbNodes <= 0)
            {
                Debug.Log("There should be atleast 1 bulb node in the Level");

            }


        }

        /// <summary>
        /// Generate JSON data based on current scene setup for level, By checking Placements and settings
        /// </summary>
        public void CreateLevelJSON()
        {
            if (m_levelStructureData != null)
                m_levelStructureData = null;

            if (levelCompletionBulbNodes <= 0)
            {
                Debug.Log("There should be atleast 1 bulb node in the Level, Please make atleast one bulb node");
                return;
            }


            m_levelStructureData = new LevelStructureDataClass();

            m_levelStructureData.levelName = levelName;
            m_levelStructureData.levelNumber = levelNumber;
            m_levelStructureData.levelCompletionBulbNodes = levelCompletionBulbNodes;

            m_levelStructureData.gridRows = levelGridDimension.x;
            m_levelStructureData.gridColumns = levelGridDimension.y;


            m_levelStructureData.levelGridCellConstraintCount = levelGridLayoutGroup.constraintCount;
            m_levelStructureData.levelGridConstraintType = levelGridLayoutGroup.constraint.ToString();
            m_levelStructureData.levelGridCellsCount = levelGridLayoutGroup.transform.childCount;
            m_levelStructureData.levelGridCellSize = new float[] { levelGridLayoutGroup.cellSize.x, levelGridLayoutGroup.cellSize.y };

            m_levelStructureData.gridCellAssociatedData = new GridCellAssociatedDataClass[levelGridLayoutGroup.transform.childCount];

            int countIndex = -1;

            for (int x = 0; x < levelGridDimension.x; x++)
            {
                for (int y = 0; y < levelGridDimension.y; y++)
                {
                    countIndex++;

                    Transform child = levelGridLayoutGroup.transform.GetChild(countIndex);

                    GridCellAssociatedDataClass gridCellAssociatedData = new GridCellAssociatedDataClass();

                    if (child.childCount > 0)
                    {
                        Transform nodeTransform = child.GetChild(0);


                        if (nodeTransform != null)
                        {
                            Node node = nodeTransform.GetComponent<Node>();
                            gridCellAssociatedData.associatedNodeUniqueID = node.NodeUniqueID;
                            gridCellAssociatedData.nodeZRotation = nodeTransform.eulerAngles.z;
                        }
                    }

                    gridCellAssociatedData.associatedGridCellNumber = countIndex + 1;
                    gridCellAssociatedData.xGridPosition = x;
                    gridCellAssociatedData.yGridPosition = y;
                    m_levelStructureData.gridCellAssociatedData[countIndex] = gridCellAssociatedData;
                }

            }

            string levelJSON = JsonUtility.ToJson(m_levelStructureData);

            string filePath = Path.Combine(Path.Combine(Path.Combine(Application.dataPath, "Resources"), saveFolderName), $"{levelName} {levelNumber}.json");

            File.WriteAllText(filePath, levelJSON);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }



        /// <summary>
        /// Load JSON Data into a Level, Populate all the required component for a Level and Setup all the level settings. Also disposes the last levels data.
        /// </summary>
        /// <param name="levelName"></param>
        /// <param name="SuccessAction"></param>
        public void LoadLevelJSON(string levelName, Action SuccessAction)
        {

            if (m_levelStructureData != null)
                m_levelStructureData = null;


            TextAsset levelTextFile = Resources.Load<TextAsset>($"GameLevels/{levelName}");

            if(levelTextFile==null)
            {
                    Debug.Log("Level Not Available!");
                    return;
            }


            if (!GameManager.Instance.DisposeCurrentLevel())
            {
                return;
            }



            m_levelStructureData = JsonUtility.FromJson<LevelStructureDataClass>(levelTextFile.text);

            levelGridLayoutGroup.constraint = Enum.Parse<GridLayoutGroup.Constraint>(m_levelStructureData.levelGridConstraintType);
            levelGridLayoutGroup.constraintCount = m_levelStructureData.levelGridCellConstraintCount;
            levelGridLayoutGroup.cellSize = new Vector2(m_levelStructureData.levelGridCellSize[0], m_levelStructureData.levelGridCellSize[1]);

            int countIndex = -1;
            GameObject[,] levelCurrentGrid = new GameObject[m_levelStructureData.gridRows, m_levelStructureData.gridColumns];
            List<Node> levelCurrentNodes = new List<Node>();


            for (int x = 0; x < m_levelStructureData.gridRows; x++)
            {
                for (int y = 0; y < m_levelStructureData.gridColumns; y++)
                {
                    countIndex++;

                    GameObject cell = Instantiate(cellPrefab, levelGridLayoutGroup.transform, false);

                    GridCellAssociatedDataClass gridCellAssociatedDataClass = m_levelStructureData.gridCellAssociatedData[countIndex];


                    if (!string.IsNullOrEmpty(gridCellAssociatedDataClass.associatedNodeUniqueID))
                    {

                        GameObject nodePrefab = GetNodePrefab(gridCellAssociatedDataClass.associatedNodeUniqueID);

                        if (nodePrefab != null)
                        {
                            GameObject node = Instantiate(nodePrefab, cell.transform, false);

                            var nodeRotation = node.transform.eulerAngles;
                            nodeRotation.z = gridCellAssociatedDataClass.nodeZRotation;
                            node.transform.eulerAngles = nodeRotation;

                            Node nodeDataClass = node.GetComponent<Node>();

                            nodeDataClass.GridCellAssociatedNumber = gridCellAssociatedDataClass.associatedGridCellNumber;
                            nodeDataClass.XGridPosition = x;
                            nodeDataClass.YGridPosition = y;



                            levelCurrentNodes.Add(nodeDataClass);

                        }
                    }

                    levelCurrentGrid[x, y] = cell;
                }
            }



            GameManager.Instance.SetCurrentGameLevelGrid(m_levelStructureData.gridRows, m_levelStructureData.gridColumns, levelCurrentGrid, m_levelStructureData.levelCompletionBulbNodes);
            GameManager.Instance.SetupNodesNeighbours();
            GameManager.Instance.SetCurrentGameSessionNodes(levelCurrentNodes);

            SuccessAction?.Invoke();
        }


        /// <summary>
        /// Returns Specific Node Prefab based on Unique ID
        /// </summary>
        /// <param name="nodeUniqueID"></param>
        /// <returns></returns>
        GameObject GetNodePrefab(string nodeUniqueID)
        {
            return nodesDataStorageList.Find((match) => match.nodesUniqueID.Equals(nodeUniqueID)).nodePrefab;
        }





        #region JSON LEVEL DATA CLASSES REGION

        /// <summary>
        /// Level JSON Structure Data Class, Stores Basic Data
        /// </summary>
        [Serializable]
        public class LevelStructureDataClass
        {
            public string levelName = string.Empty;
            public int levelNumber = 0;
            public int levelCompletionBulbNodes = 0;

            public int gridRows = 0;
            public int gridColumns = 0;

            public int levelGridCellsCount = 0;
            public float[] levelGridCellSize;
            public string levelGridConstraintType = string.Empty;
            public int levelGridCellConstraintCount = 0;
            public GridCellAssociatedDataClass[] gridCellAssociatedData;

        }

        /// <summary>
        /// Level JSON Structure Data Class, Stores Nodes related data
        /// </summary>
        [Serializable]
        public class GridCellAssociatedDataClass
        {
            public int associatedGridCellNumber = -1;
            public string associatedNodeUniqueID = string.Empty;
            public int xGridPosition = -1;
            public int yGridPosition = -1;
            public float nodeZRotation = 0f;
        }
        #endregion

        #region NODES DATA MANAGE CLASS REGION

        /// <summary>
        /// Nodes Prefab Storage Class
        /// </summary>
        [Serializable]
        public class NodesDataStorageClass
        {
            public string nodesUniqueID = string.Empty;
            public GameObject nodePrefab;
        }



        #endregion

    }
}


