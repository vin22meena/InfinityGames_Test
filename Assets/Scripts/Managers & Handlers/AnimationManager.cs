using UnityEngine;


/// <summary>
/// AnimationManager.cs Class handles MenuButton simple animation.
/// </summary>

namespace LoopEnergyClone
{
    public class AnimationManager : MonoBehaviour
    {
        [SerializeField] Animator _menuButtonAnimator; //Animator Reference
        [SerializeField] GameObject _menuPanelObject; //Menu Object Reference to Enable and Disable
        bool isMenuOpen = false; //Toggling bool


        /// <summary>
        /// Animating Menu
        /// </summary>
        public void OnMenuButtonClick()
        {
            isMenuOpen = !isMenuOpen;

            AudioManager.Instance.PlaySFX("btnclick");

            _menuPanelObject?.SetActive(isMenuOpen);
            _menuButtonAnimator.SetBool("IsMenuOpened", isMenuOpen);
        }


    }

}