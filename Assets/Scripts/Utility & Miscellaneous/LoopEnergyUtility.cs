using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// LoopEnergyUtility.cs Class is static class handles some special functions
/// </summary>

namespace LoopEnergyClone.Utility
{
    public static class LoopEnergyUtility
    {
        /// <summary>
        /// Update alpha value of reference Image
        /// </summary>
        /// <param name="referenceImage"></param>
        /// <param name="alphaValue"></param>
        public static void UpdateAlpha(Image referenceImage, float alphaValue)
        {
            Color color = referenceImage.color;
            color.a = alphaValue;
            referenceImage.color = color;

        }

    }

}