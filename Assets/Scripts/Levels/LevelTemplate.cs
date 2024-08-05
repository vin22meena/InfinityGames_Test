using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// LevelTemplate.cs Class, holds the Level Card data and performs operation accordingly
/// </summary>

namespace LoopEnergyClone
{
    public class LevelTemplate : MonoBehaviour, IPointerClickHandler
    {

        /// <summary>
        /// Level to be loaded property to Handle, Which Level to load
        /// </summary>
        string m_toBeLoadedLevel;
        public string _toBeloadedLevel
        {
            get
            {
                return m_toBeLoadedLevel;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    m_toBeLoadedLevel = value;
                    _levelNameText.text = value;
                }
            }
        }


        /// <summary>
        /// Lock Proprty, Handles which level is locked and unlocked
        /// </summary>
        bool m_isLevelLocked;
        public bool _isLevelLocked
        {
            get
            {
                return m_isLevelLocked;
            }
            set
            {
                m_isLevelLocked = value;
                _lockIconObejct?.SetActive(value);
            }
        }


        public int LevelNumber { get; set; } = 0; //Level Number

        [SerializeField] TMP_Text _levelNameText; //Level Name Text Referene
        [SerializeField] GameObject _lockIconObejct; //Lock Icon Reference


        public static Action<LevelTemplate> OnLevelSelect; //Level Selection Event

        /// <summary>
        /// On Level Card Select, Fires the Selection Event
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            OnLevelSelect?.Invoke(this);
        }
    }
}