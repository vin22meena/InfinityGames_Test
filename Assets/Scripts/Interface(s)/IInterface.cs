using UnityEngine.UI;

/// <summary>
/// Interface for Highlighting Image
/// </summary>

namespace LoopEnergyClone
{
    public interface IHighlightable
    {
        public Image HighlightImage { get; set; }
        public float HighlightResetValue { get; set; }

        public void OnHighlight(float alphaValue);
        public void OnReset();
    }

}