using UnityEngine;

public class StandRotator : MonoBehaviour
{
    private DragHandler m_DragHandler;
    private bool m_IsDragging;
    private float m_RotateCountdown;

    [SerializeField] private Transform m_SpawnPoint = default;
    [SerializeField] private float m_RotateSpeed = -2f;
    [SerializeField] private float m_DragRotateSensivity = 0.5f;
    [SerializeField] private float m_RotatePause = 1f;

    private void Awake()
    {
        m_DragHandler = GetComponent<DragHandler>();
        m_DragHandler.DragStart += () => m_IsDragging = true;
        m_DragHandler.DragEnd += () =>
        {
            m_IsDragging = false;
            m_RotateCountdown = m_RotatePause;
        };
        m_DragHandler.DragPerforming += (position, delta) =>
        {
            transform.Rotate(Vector3.up, -delta.x * m_DragRotateSensivity);
            m_SpawnPoint.Rotate(Vector3.up, -delta.x * m_DragRotateSensivity);
        };
    }

    private void Update()
    {
        if (!m_IsDragging)
        {
            if (m_RotateCountdown > 0)
                m_RotateCountdown -= Time.deltaTime;
            else
            {
                transform.Rotate(new Vector3(0, 1, 0), m_RotateSpeed * Time.deltaTime);
                m_SpawnPoint.Rotate(new Vector3(0, 1, 0), m_RotateSpeed * Time.deltaTime);
            }
        }
    }
}
