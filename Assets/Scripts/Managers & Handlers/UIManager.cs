using TMPro;
using UnityEngine;

/// <summary>
/// UIManager.cs Class, Takes care of in game UI events like enabling disabling popups etc.
/// </summary>

namespace LoopEnergyClone
{
    public class UIManager : GenericSingleton<UIManager>
    {
        [SerializeField] GameObject _levelsMenu; //Levels Menu Object Reference
        [SerializeField] TMP_Text _gameScoreText; //GameScore Text Reference
        [SerializeField] GameObject _levelCompletionPanel; //Level Complete Popup Object Reference
        [SerializeField] TMP_Text _levelCompletionScoreText; //Score to be shown on Level Complete Text Reference

        private void OnEnable()
        {
            PathDetectionManager.OnLevelCompletion += OnLevelCompletion;
        }

        private void OnDisable()
        {
            PathDetectionManager.OnLevelCompletion -= OnLevelCompletion;
        }


        /// <summary>
        /// On Level Completion, Poping Up the Level Completion Popup after some time delay for User Experience
        /// </summary>
        void OnLevelCompletion()
        {
            Invoke("InvokeLevelCompletionPopup", 0.5f);
        }

        /// <summary>
        /// Level Completion Popup Invocation
        /// </summary>
        void InvokeLevelCompletionPopup()
        {
            _levelCompletionScoreText.text = ScoreManager.Instance._currentScore.ToString();
            _levelCompletionPanel?.SetActive(true);
        }

        /// <summary>
        /// Toggling Levels Menu on button click
        /// </summary>
        /// <param name="isActive"></param>
        public void ToggleLevelsMenu(bool isActive)
        {
            AudioManager.Instance.PlaySFX("btnclick");
            _levelsMenu?.SetActive(isActive);
        }

        /// <summary>
        /// Update Score UI
        /// </summary>
        /// <param name="score"></param>
        public void UpdateScore(int score)
        {
            _gameScoreText.text = score.ToString();
        }

        /// <summary>
        /// Application Quit Method
        /// </summary>
        public void ApplicationQuit()
        {
            AudioManager.Instance.PlaySFX("btnclick");
            Application.Quit();
        }

        /// <summary>
        /// Restart Button click, Enabling and Disabling some UI Component before restarting the level
        /// </summary>
        public void OnRestartButtonClick()
        {

            AudioManager.Instance.PlaySFX("btnclick");

            _levelCompletionPanel?.SetActive(false);

            VisualAppearanceHandler.Instance.UpdateRandomBackground();

            LevelManager.Instance.OnRestartButtonClick();
        }

        /// <summary>
        /// Next Level Load Button click, Enabling and Disabling some UI Component before Loading the next level
        /// </summary>
        public void OnNextLevelLoadButtonClick()
        {
            AudioManager.Instance.PlaySFX("btnclick");

            _levelCompletionPanel?.SetActive(false);


            VisualAppearanceHandler.Instance.UpdateRandomBackground();


            LevelManager.Instance.LoadNextLevel();
        }

        /// <summary>
        /// Toggle Sound Mute Unmute
        /// </summary>
        public void OnMuteButtonClick()
        {
            AudioManager.Instance.PlaySFX("btnclick");
            AudioManager.Instance.ToggleMute();
        }

        /// <summary>
        /// Toggle Vibration
        /// </summary>
        public void OnVibrationToggleButtonClick()
        {
            AudioManager.Instance.PlaySFX("btnclick");
            AudioManager.Instance.VibrateToggle();
        }


    }

}