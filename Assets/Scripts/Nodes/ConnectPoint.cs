using UnityEngine;

/// <summary>
/// ConnectPoint.cs Class, Handles the references of Connectable Points along with a Tag
/// </summary>

namespace LoopEnergyClone
{
    public class ConnectPoint : MonoBehaviour
    {

        [field: SerializeField] public ConnectPoint ConnectedToPoint { get; set; } //Connected To Point, Which is connected to this particular Connect point
        [field: SerializeField] public Node AssociatedNode { get; set; } //Node, Which is associated to this particular Connect point

    }
}
