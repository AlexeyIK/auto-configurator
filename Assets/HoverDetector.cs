using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    private Vector2 m_CurrentPointerPos;

    [SerializeField] private Material m_HoverMaterial = default;
    [SerializeField] private MeshRenderer[] m_Meshes = default;
    [SerializeField] private List<Material> m_DefaultMaterials = default;

    private Camera m_Camera;

    public bool IsCaptured
    {
        get
        {
            Ray ray = m_Camera.ScreenPointToRay(m_CurrentPointerPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Hits collider on object: " + hit.transform.name);
                return hit.transform == transform;
            }

            return false;
        }
    }

    private void Awake()
    {
        m_Camera = Camera.main;

        var meshCounter = 0;

        foreach (var meshRenderer in m_Meshes)
        {
            m_DefaultMaterials.Add(meshRenderer.material);
            meshCounter++;
        }
    }

    private void OnDestroy()
    {
        var collider = GetComponentInChildren<Collider>();
        collider.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        for (var i = 0; i < m_Meshes.Length; i++)
            m_Meshes[i].material = m_HoverMaterial;

        Debug.Log("Hover on");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (var i = 0; i < m_Meshes.Length; i++)
            m_Meshes[i].material = m_DefaultMaterials[i];

        Debug.Log("Hover leave");
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        m_CurrentPointerPos = eventData.position;
        if (IsCaptured)
            Debug.Log("Hover on");
    }
}
