using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragHandler : MonoBehaviour
{
    private InputAction m_PressAction;
    private InputAction m_PointerPositionAction;
    private InputAction m_PointerDeltaAction;

    private Vector2 m_DragStartPos;
    private Vector2 m_CurrentPointerPos;
    private Vector2 m_PointerDelta;
    private bool m_IsDragging;

    private Camera m_Camera;

    public bool IsCaptured
    {
        get
        {
            Ray ray = m_Camera.ScreenPointToRay(m_CurrentPointerPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.transform == transform;
            }

            return false;
        }
    }

    public Action DragStart;
    public Action<Vector2, Vector2> DragPerforming;
    public Action DragEnd;

    private void Awake()
    {
        m_Camera = Camera.main;

        m_PressAction = InputSystem.actions.FindAction("DragStart");
        m_PointerPositionAction = InputSystem.actions.FindAction("DragPosition");
        m_PointerDeltaAction = InputSystem.actions.FindAction("DragDelta");

        m_PressAction.Enable();
        m_PointerPositionAction.Enable();
        m_PointerDeltaAction.Enable();

        m_PointerDeltaAction.performed += OnDeltaChange;
        m_PointerPositionAction.performed += OnPointerPositionChange;
        m_PressAction.started += OnDragStart;
        m_PressAction.canceled += OnDragEnd;
    }

    private void OnDestroy()
    {
        m_PressAction.canceled -= OnDragEnd;
        m_PressAction.started -= OnDragStart;
        m_PointerPositionAction.performed -= OnPointerPositionChange;
        m_PointerDeltaAction.performed -= OnDeltaChange;

        m_PressAction = null;
        m_PointerPositionAction = null;
    }

    private void OnDeltaChange(InputAction.CallbackContext context) => m_PointerDelta = context.ReadValue<Vector2>();

    private void OnDragStart(InputAction.CallbackContext context)
    {
        if (IsCaptured && !m_IsDragging)
        {
            m_IsDragging = true;
            m_DragStartPos = m_CurrentPointerPos;
            Debug.Log($"Drag started at pos: {m_DragStartPos}");
            DragStart?.Invoke();
        }
    }

    private void OnDragEnd(InputAction.CallbackContext context)
    {
        if (m_IsDragging)
        {
            m_IsDragging = false;
            Debug.Log("Drag ended");
            DragEnd?.Invoke();
        }
    }

    private void OnPointerPositionChange(InputAction.CallbackContext context)
    {
        m_CurrentPointerPos = context.ReadValue<Vector2>();

        if (m_IsDragging)
        {
            // отправляем дельту перемещения курсора относительно старта драга
            DragPerforming?.Invoke(m_CurrentPointerPos, m_PointerDelta);
        }
    }

    private void OnDrawGizmos()
    {
        if (m_IsDragging)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, transform.localScale.magnitude / 2f);
        }
    }
}
