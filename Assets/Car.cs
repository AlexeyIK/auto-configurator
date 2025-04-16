using System;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] public string CarID;
    [SerializeField] private Guid m_ID = Guid.NewGuid();
    [SerializeField] private bool m_OverrideColor = false;
    [SerializeField] private Color m_Color = default;

    public bool OverrideColor => m_OverrideColor;

    private void OnValidate()
    {
        if (String.IsNullOrWhiteSpace(CarID))
            CarID = m_ID.ToString();
    }

    private void Awake()
    {
        var colorChanger = GetComponentInChildren<ColorChanger>();
        if (colorChanger == null)
            Debug.LogError("You need a ColorChanger component on the car");

        if (m_OverrideColor)
            colorChanger.ChangeColor(m_Color);

        gameObject.name += $"({CarID})";
    }
}
