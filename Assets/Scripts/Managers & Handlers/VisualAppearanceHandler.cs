using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VisualAppearanceHandler.cs Class, Takes care of background changes and other visual effects related actions
/// </summary>

namespace LoopEnergyClone
{
    public class VisualAppearanceHandler : GenericSingleton<VisualAppearanceHandler>
    {

        [SerializeField] Image _backgroundImage;  //Background UI Image Reference
        [SerializeField] List<VisualBackgroundData> _backgroundData = new List<VisualBackgroundData>(); //All Background materials data

        System.Random randomBackground = new System.Random(); //Randomizer for randomizing the background

        private void Start()
        {
            UpdateRandomBackground();
        }

        /// <summary>
        /// Updating Random Background
        /// </summary>
        public void UpdateRandomBackground()
        {
            _backgroundImage.material = _backgroundData[randomBackground.Next(0, _backgroundData.Count - 1)].backgroundMaterial;
        }

        /// <summary>
        /// Get Background Material by backroundKey
        /// </summary>
        /// <param name="backgroundKey"></param>
        /// <returns></returns>
        Material GetBackgroundMaterial(string backgroundKey)
        {
            return _backgroundData.Find((match) => match.backgroundKey == backgroundKey).backgroundMaterial;
        }


        /// <summary>
        /// Background Appearance Data Storage Class
        /// </summary>
        [Serializable]
        public class VisualBackgroundData
        {
            public string backgroundKey;
            public Material backgroundMaterial;
        }


    }

}