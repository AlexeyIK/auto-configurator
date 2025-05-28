using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HoverDetector : MonoBehaviour
{
    private InputAction m_PointerPositionAction;
    private LayerMask m_LayerMask;
    private Camera m_Camera;

    [SerializeField] private Material m_HoverMaterial = default;
    [SerializeField] private MeshRenderer[] m_Meshes = default;
    [SerializeField] private List<Material> m_DefaultMaterials = default;

    private void Awake()
    {
        m_Camera = Camera.main;
        m_LayerMask = gameObject.layer;

        var meshCounter = 0;

        foreach (var meshRenderer in m_Meshes)
        {
            m_DefaultMaterials.Add(meshRenderer.material);
            meshCounter++;
        }

        m_PointerPositionAction = InputSystem.actions.FindAction("DragPosition");
        m_PointerPositionAction.Enable();
        m_PointerPositionAction.performed += OnPointerMove;
    }

    private void OnPointerMove(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = context.ReadValue<Vector2>();
        Ray ray = m_Camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out var hit, 10))
        {
            if (hit.transform == transform)
            {
                PointerEnter();
                return;
            }
        }

        PointerLeave();
    }

    private void OnDestroy()
    {
        m_PointerPositionAction.Disable();
        m_PointerPositionAction.performed -= OnPointerMove;

        var collider = GetComponentInChildren<Collider>();
        collider.enabled = false;
    }

    public void PointerEnter()
    {
        for (var i = 0; i < m_Meshes.Length; i++)
            m_Meshes[i].material = m_HoverMaterial;
    }

    public void PointerLeave()
    {
        for (var i = 0; i < m_Meshes.Length; i++)
            m_Meshes[i].material = m_DefaultMaterials[i];
    }
}
