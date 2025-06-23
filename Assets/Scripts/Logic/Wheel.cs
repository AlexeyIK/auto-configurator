using UnityEngine;

public class Wheel : MonoBehaviour
{
    private PieceAttachSocket attachPoint;

    private void Awake()
    {
        attachPoint = GetComponentInParent<PieceAttachSocket>();

        if (attachPoint.Orientation == PieceAttachSocket.PointOrientation.Right)
        {
            transform.Rotate(Vector3.up, 180);
        }
    }
}
