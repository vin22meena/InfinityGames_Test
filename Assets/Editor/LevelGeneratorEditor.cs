#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// LevelGeneratorEditor.cs Editor Class helps in speeding up Level Generation process by providing Custom Inspector Setup.
/// </summary>

namespace LoopEnergyClone
{
    [CustomEditor(typeof(LevelGenerator))]
    public class LevelGeneratorEditor : Editor
    {

        public override void OnInspectorGUI()
        {

            if (SceneManager.GetActiveScene().name == "LevelGenerationScene")
            {
                base.OnInspectorGUI();
            }

            LevelGenerator generator = target as LevelGenerator;

            //Generating Button for ease of Level JSON Generation
            if (GUILayout.Button("Generate Level JSON"))
            {
                if (SceneManager.GetActiveScene().name != "LevelGenerationScene")
                {
                    Debug.Log("Cannot Perform Level Generation in Current Scene. Please Open the LevelGenerationScene First");
                }
                else
                {
                    generator.CreateLevelJSON();
                }
            }
        }
    }
}
#endif