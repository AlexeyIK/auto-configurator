using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Camera m_Camera;
    private InputAction m_ScrollWheel;
    private Vector3 m_PointToPosition;

    public float m_Distance;

    [SerializeField] private float m_MinDistance = 0.75f;
    [SerializeField] private float m_MaxDistance = 3f;
    [SerializeField] private float m_Sensivity = 0.01f;
    [SerializeField] private Transform m_PointToObject = default;
    [SerializeField] private float m_Height = 0.3f;

    private void Awake()
    {
        m_PointToPosition = m_PointToObject.position + Vector3.up * m_Height;

        m_Camera = GetComponent<Camera>();
        m_Camera.transform.LookAt(m_PointToPosition);
        m_Distance = (transform.position - m_PointToPosition).magnitude;

        m_ScrollWheel = InputSystem.actions.FindAction("ScrollWheel");
        m_ScrollWheel.Enable();

        m_ScrollWheel.performed += OnMouseScroll;

        PointCamera();
    }

    private void OnMouseScroll(InputAction.CallbackContext context)
    {
        //Debug.Log("Mouse wheel: " + context.ReadValue<Vector2>() * m_Sensivity);
        m_Distance = Mathf.Clamp(m_Distance - context.ReadValue<Vector2>().y * m_Sensivity, m_MinDistance, m_MaxDistance);
        PointCamera();
    }

    private void PointCamera()
    {
        transform.position = m_PointToPosition + transform.TransformDirection(Vector3.back) * m_Distance;
    }
}
