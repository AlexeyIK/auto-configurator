using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Material m_HoverMaterial = default;
    [SerializeField] private MeshRenderer[] m_Meshes = default;
    [SerializeField] private List<Material> m_DefaultMaterials = default;

    private void Awake()
    {
        var meshCounter = 0;

        foreach (var meshRenderer in m_Meshes)
        {
            m_DefaultMaterials.Add(meshRenderer.material);
            meshCounter++;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (var i = 0; i < m_Meshes.Length; i++)
            m_Meshes[i].material = m_HoverMaterial;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        for (var i = 0; i < m_Meshes.Length; i++)
            m_Meshes[i].material = m_DefaultMaterials[i];
    }
}
