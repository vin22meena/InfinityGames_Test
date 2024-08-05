using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LevelManager.cs Class, Responsible for handling all Levels Lock, Unlock, Loading Actions.
/// </summary>

namespace LoopEnergyClone
{
    public class LevelManager : GenericSingleton<LevelManager>
    {

        [SerializeField] List<string> _totalLevelsFilenames;//Total Available Levels

        [SerializeField] GameObject _levelTemplatePrefab; //Level Menu's Level Template Prefab
        [SerializeField] Transform _levelTemplateParent; //Level Menu's Level Templated Parent


        int m_currentLevelIndex = 0; //Current level index

        int m_currentLevelTemplatedNumber = 0; //Current level number

        List<LevelTemplate> _levelTemplates = new List<LevelTemplate>(); //Cached list of level templates

        private void Start()
        {
            //By default loading of level 1
            PopulateLevelsData();
        }


        private void OnEnable()
        {
            LevelTemplate.OnLevelSelect += OnLevelSelect; //Level Selection Event
            PathDetectionManager.OnLevelCompletion += OnLevelCompletion; //Level Completion Event
        }

        private void OnDisable()
        {
            LevelTemplate.OnLevelSelect -= OnLevelSelect;
            PathDetectionManager.OnLevelCompletion -= OnLevelCompletion;

        }

        /// <summary>
        /// Populating Level Templates to showcase level cards in Level's Menu
        /// </summary>
        public void PopulateLevelsData()
        {

            m_currentLevelIndex = 0;

            m_currentLevelTemplatedNumber++;

            for (int i = 0; i < _totalLevelsFilenames.Count; i++)
            {
                GameObject levelTemplateGo = Instantiate(_levelTemplatePrefab, _levelTemplateParent, false);

                LevelTemplate levelTemplate = levelTemplateGo.GetComponent<LevelTemplate>();

                if (levelTemplate == null)
                    continue;

                levelTemplate._toBeloadedLevel = _totalLevelsFilenames[i];
                levelTemplate.LevelNumber = int.Parse(_totalLevelsFilenames[i].Split(' ')[1]);
                levelTemplate._isLevelLocked = true;

                _levelTemplates.Add(levelTemplate);
            }

            LevelTemplate levelTemplateToBeLoaded = GetLevel(m_currentLevelIndex);

            levelTemplateToBeLoaded._isLevelLocked = false;

            LoadLevel(levelTemplateToBeLoaded._toBeloadedLevel, null);


        }

        /// <summary>
        /// Loadind the Level by clearing out the last level data
        /// </summary>
        /// <param name="levelName"></param>
        /// <param name="SuccessAction"></param>
        public void LoadLevel(string levelName, Action SuccessAction)
        {
            LevelGenerator.Instance.LoadLevelJSON(levelName, SuccessAction);
        }


        /// <summary>
        /// Load Next Level
        /// </summary>
        public void LoadNextLevel()
        {
            m_currentLevelIndex++;

            if (m_currentLevelIndex >= _totalLevelsFilenames.Count)
            {
                m_currentLevelIndex = 0;
                m_currentLevelTemplatedNumber = 1;

                LoadLevel(GetLevel(m_currentLevelIndex)._toBeloadedLevel, null);
                UIManager.Instance.ToggleLevelsMenu(true);

                return;
            }

            LevelTemplate levelTemplate = GetLevel(m_currentLevelIndex);
            LoadLevel(levelTemplate._toBeloadedLevel, null);

        }

        /// <summary>
        /// Level Template Card Selection
        /// </summary>
        /// <param name="levelTemplate"></param>
        void OnLevelSelect(LevelTemplate levelTemplate)
        {
            m_currentLevelTemplatedNumber = levelTemplate.LevelNumber;
            m_currentLevelIndex = levelTemplate.LevelNumber - 1;

            if (!levelTemplate._isLevelLocked)
            {
                LoadLevel(levelTemplate._toBeloadedLevel, () =>
                {
                    UIManager.Instance.ToggleLevelsMenu(false);
                });

            }

        }

        /// <summary>
        /// Restart Button Click, Loads The Same Level
        /// </summary>
        public void OnRestartButtonClick()
        {
            LevelTemplate levelTemplate = GetLevel(m_currentLevelIndex);

            LoadLevel(levelTemplate._toBeloadedLevel, null);

        }

        /// <summary>
        /// On Level Completion, Unlocks next level accordingly
        /// </summary>
        void OnLevelCompletion()
        {

            LevelTemplate levelTemplate = GetLevel(m_currentLevelIndex);

            m_currentLevelTemplatedNumber = levelTemplate.LevelNumber;

            if (m_currentLevelTemplatedNumber < 1 && m_currentLevelTemplatedNumber > _levelTemplates.Count)
            {
                m_currentLevelTemplatedNumber = 1;
                return;
            }

            LevelTemplate levelTemplateToBeUnlocked = GetLevel(m_currentLevelTemplatedNumber);

            if (levelTemplateToBeUnlocked != null)
            {
                if (levelTemplateToBeUnlocked._isLevelLocked)
                    levelTemplateToBeUnlocked._isLevelLocked = false;
            }

        }


        /// <summary>
        /// Get the Valid Level from cached _levelTemplated Data List
        /// </summary>
        /// <param name="levelIndex"></param>
        /// <returns></returns>
        LevelTemplate GetLevel(int levelIndex)
        {
            if (levelIndex >= 0 && levelIndex < _levelTemplates.Count)
                return _levelTemplates[levelIndex];

            return null;
        }

    }
}
