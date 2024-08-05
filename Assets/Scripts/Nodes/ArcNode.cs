using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using LoopEnergyClone.Utility;

/// <summary>
/// ArcNode.cs Class, Inherited from Node class and Performs Specific Operation according to Arc Node.
/// </summary>

namespace LoopEnergyClone
{
    public class ArcNode : Node, IHighlightable
    {
        [field: SerializeField] public Image HighlightImage { get; set; } //Highlight Image, Which can be assigned from inspector
        [field: SerializeField] public float HighlightResetValue { get; set; } //Reset Alpha Value when Disconnecting, Which can be assigned from inspector


        /// <summary>
        /// Highlight Image Function
        /// </summary>
        /// <param name="alphaValue"></param>
        public void OnHighlight(float alphaValue)
        {
            LoopEnergyUtility.UpdateAlpha(HighlightImage, alphaValue);
        }

        /// <summary>
        /// Reset Highlight Value to Original
        /// </summary>
        public void OnReset()
        {
            LoopEnergyUtility.UpdateAlpha(HighlightImage, HighlightResetValue);
        }


        /// <summary>
        /// OnTap function, Handles all the rotation and validation logic
        /// </summary>
        /// <param name="rotateSpeed"></param>
        /// <param name="rotateAngle"></param>
        public override void OnTap(float rotateSpeed = 0f, float rotateAngle = 0f)
        {

            if (PathDetectionManager.IsDetectionRunning)
                return;

            if (IsNodeTapped)
                return;

            IsNodeTapped = true;

            AudioManager.Instance.PlaySFX("connectnodes");

            transform.DORotate(new Vector3(0f, 0f, rotateAngle), rotateSpeed, RotateMode.LocalAxisAdd).OnComplete(() =>
            {
                IsNodeTapped = false;

                Invoke("InvokePathDetection", 0.15f);
            });
        }


        /// <summary>
        /// Invocation Delay Method
        /// </summary>
        void InvokePathDetection()
        {
            OnPathDetectionInvoke?.Invoke();
        }

    }
}