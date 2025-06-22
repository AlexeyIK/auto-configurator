using UnityEngine;

public class StandRotator : MonoBehaviour
{
    private bool isRotating = false;
    private DragHandler dragHandler;
    private bool isDragging;
    private float rotateCountdown;

    private float initAngleY;

    [SerializeField] private Transform m_SpawnPoint = default;
    [SerializeField] private float m_RotateSpeed = -2f;
    [SerializeField] private float m_DragRotateSensivity = 0.5f;
    [SerializeField] private float m_RotatePause = 1f;

    public bool IsRotating
    {
        get { return isRotating; }
        set {
            isRotating = value;
        }
    }

    private void Awake()
    {
        initAngleY = m_SpawnPoint.rotation.y;

        dragHandler = GetComponent<DragHandler>();
        dragHandler.DragStart += () => isDragging = true;
        dragHandler.DragEnd += () =>
        {
            isDragging = false;
            rotateCountdown = m_RotatePause;
        };
        dragHandler.DragPerforming += (position, delta) =>
        {
            transform.Rotate(Vector3.up, -delta.x * m_DragRotateSensivity);
            m_SpawnPoint.Rotate(Vector3.up, -delta.x * m_DragRotateSensivity);
        };
    }

    private void Update()
    {
        if (isRotating && !isDragging)
        {
            if (rotateCountdown > 0)
                rotateCountdown -= Time.deltaTime;
            else
            {
                transform.Rotate(new Vector3(0, 1, 0), m_RotateSpeed * Time.deltaTime);
                m_SpawnPoint.Rotate(new Vector3(0, 1, 0), m_RotateSpeed * Time.deltaTime);
            }
        }
    }
}
