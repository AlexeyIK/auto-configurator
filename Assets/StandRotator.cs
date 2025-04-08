using UnityEngine;

public class StandRotator : MonoBehaviour
{
    private Transform m_Transform;

    [SerializeField]
    private float m_RotateSpeed = 0.1f;

    void Start()
    {
        m_Transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        m_Transform.Rotate(new Vector3(0, 1, 0), m_RotateSpeed);
    }
}
