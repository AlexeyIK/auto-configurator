using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] m_Renderers = default;
    [SerializeField] private string m_ColorPropertyName = "baseColorFactor";

    public void ChangeColor(Color color)
    {
        if (m_Renderers.Length > 0)
            foreach (var renderer in m_Renderers)
            {
                Debug.Log("Material color: " + renderer.sharedMaterial?.GetColor(m_ColorPropertyName));
                renderer.sharedMaterial?.SetColor(m_ColorPropertyName, color);
            }
    }
}
