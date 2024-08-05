using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// InputManager.cs Class, Handles all the Game Inputs
/// </summary>

namespace LoopEnergyClone
{
    public class InputManager : MonoBehaviour, IPointerClickHandler
    {

        [SerializeField]
        [Range(0, 360)]
        float rotatingAngleOnInput = 90f;  //How much a node can rotate

        [SerializeField]
        float rotatingSpeed = 0.5f; //How fast node can rotate

        Node m_currentNode; //Reference of Current Node

        private void Start()
        {
            m_currentNode = GetComponent<Node>();
        }

        /// <summary>
        /// On Pointer Click UI Event, Node click will trigger the function and performs the valid action
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_currentNode != null)
            {
                m_currentNode.OnTap(rotatingSpeed, -rotatingAngleOnInput);
            }
        }
    }

}