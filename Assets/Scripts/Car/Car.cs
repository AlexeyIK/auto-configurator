using System;
using UnityEngine;

[RequireComponent(typeof(ColorChanger), typeof(CategorySelector))]
public class Car : MonoBehaviour
{
    private ColorChanger m_ColorChanger;
    private CategorySelector[] m_CategorySelectors;

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
        m_CategorySelectors = GetComponents<CategorySelector>();
        m_ColorChanger = GetComponent<ColorChanger>();
        if (m_ColorChanger == null)
            Debug.LogError("You need a ColorChanger component on the car");
    }

    private void Start()
    {
        AppStateManager.Instance.StateChange += OnStateChange;
        OnStateChange(AppStateManager.Instance.State);
    }

    private void OnDestroy()
    {
        AppStateManager.Instance.StateChange -= OnStateChange;
    }

    private void OnStateChange(AppStateManager.AppState state)
    {
        if (state == AppStateManager.AppState.ProjectModification)
        {
            foreach (var selector in m_CategorySelectors)
                selector.SetActivity(true);
        }
        else
        {
            foreach (var selector in m_CategorySelectors)
                selector.SetActivity(false);
        }
    }
}
