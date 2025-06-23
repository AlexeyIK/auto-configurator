using UnityEngine;

public class PieceAttachSocket : MonoBehaviour
{
    public enum PointOrientation
    {
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back,
        Center
    }

    [SerializeField] private PointOrientation m_Orientation = PointOrientation.Left;

    public PointOrientation Orientation => m_Orientation;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
