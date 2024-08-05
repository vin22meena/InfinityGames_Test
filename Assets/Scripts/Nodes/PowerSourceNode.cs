
/// <summary>
/// PowerSourceNode.cs Class, Inherited from Node class and Performs Specific Operation according to PowerSource Node.
/// And handles Path Detection on every rotation of other nodes.
/// </summary>

namespace LoopEnergyClone
{
    public class PowerSourceNode : Node
    {
        /// <summary>
        /// OnTap function, Handles Camera Shake Effect to show invalid tap and Plays error sound
        /// </summary>
        /// <param name="rotateSpeed"></param>
        /// <param name="rotateAngle"></param>
        public override void OnTap(float rotateSpeed = 0f, float rotateAngle = 0f)
        {
            if (IsNodeTapped)
                return;

            IsNodeTapped = true;

            AudioManager.Instance.PlaySFX("error");
            AudioManager.Instance.Vibrate();

            CameraShake.Instance.ShakeCamera(() =>
            {
                IsNodeTapped = false;
            });
        }
    }
}
