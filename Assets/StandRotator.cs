using UnityEngine;

public class StandRotator : MonoBehaviour
{
    private DragHandler m_DragHandler;
    private bool m_IsDragging;

    [SerializeField] private float m_RotateSpeed = 0.1f;
    [SerializeField] private float m_DragRotateSensevity = 0.2f;

    private void Awake()
    {
        m_DragHandler = GetComponent<DragHandler>();
        m_DragHandler.DragStart += () => m_IsDragging = true;
        m_DragHandler.DragEnd += () => m_IsDragging = false;
        m_DragHandler.DragPerforming += (position, delta) => transform.Rotate(Vector3.up, -delta.x * m_DragRotateSensevity);
    }

    private void FixedUpdate()
    {
        if (!m_IsDragging)
            transform.Rotate(new Vector3(0, 1, 0), m_RotateSpeed);
    }
}
