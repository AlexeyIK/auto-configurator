using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer[] m_Renderers = default;

    public void ChangeColor(Color color)
    {
        if (m_Renderers.Length > 0)
            foreach (var renderer in m_Renderers)
                renderer.sharedMaterial?.SetColor("baseColorFactor", color);
    }
}
