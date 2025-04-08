using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField]
    private Material mBaseMaterial = default;
    [SerializeField]
    private Color mColor = default;

    private void OnValidate()
    {
        if (mBaseMaterial != null)
        {
            Debug.Log(mBaseMaterial.GetColor("baseColorFactor"));
            mBaseMaterial.SetColor("baseColorFactor", mColor);
        }
    }
}
