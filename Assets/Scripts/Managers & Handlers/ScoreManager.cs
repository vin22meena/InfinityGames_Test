using UnityEngine;

/// <summary>
/// ScoreManager.cs Class, Takes care of scoring logic on each level completion
/// </summary>

namespace LoopEnergyClone
{
    public class ScoreManager : GenericSingleton<ScoreManager>
    {

        /// <summary>
        /// Current Score Property
        /// </summary>
        int m_currentScore;
        public int _currentScore
        {
            get
            {
                return m_currentScore;
            }
            set
            {
                m_currentScore = value;

                UIManager.Instance.UpdateScore(value);
            }
        }


        [SerializeField] int _levelCompletionRewardScore; //Variable Score, Which can be given after successful completion of levels


        private void Start()
        {
            //Score Saving into inbuilt Unity's PlayerPrefs and fetching dynamically
            if (PlayerPrefs.HasKey("Loop Energy Score"))
            {
                _currentScore = PlayerPrefs.GetInt("Loop Energy Score", 0);
            }
        }


        private void OnEnable()
        {
            PathDetectionManager.OnLevelCompletion += OnLevelCompletion;
        }

        private void OnDisable()
        {
            PathDetectionManager.OnLevelCompletion -= OnLevelCompletion;
        }

        /// <summary>
        /// Score Updation On Successful Level Completion
        /// </summary>
        void OnLevelCompletion()
        {
            _currentScore += _levelCompletionRewardScore;

            //Saving score data into PlayerPrefs
            PlayerPrefs.SetInt("Loop Energy Score", _currentScore);
        }

    }

}