using Data.Model;
using UnityEngine;

[RequireComponent(typeof(ColorChanger), typeof(CategorySelector))]
public class Car : MonoBehaviour
{
    private ColorChanger m_ColorChanger;
    private CategorySelector[] m_CategorySelectors;
    private Automobile data;

    [SerializeField] private int m_CarID = 1;
    [SerializeField] private bool m_OverrideColor = false;
    [SerializeField] private UnityEngine.Color m_Color = default;

    public Automobile Data => data;
    public int CarId => m_CarID;
    public bool OverrideColor => m_OverrideColor;

    private void OnValidate()
    {
        var cars = Resources.FindObjectsOfTypeAll<Car>();
        foreach (var car in cars)
            if (car.CarId > m_CarID)
                m_CarID = car.CarId + 1;
    }

    private void Awake()
    {
        m_CategorySelectors = GetComponents<CategorySelector>();
        m_ColorChanger = GetComponent<ColorChanger>();
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

    public void SetDataContext(Automobile dataContext)
    {
        data = dataContext;
        m_CarID = data.Id;
    }

    public void ChangeColorTo(UnityEngine.Color color)
    {
        m_ColorChanger.ChangeColor(color);
        m_Color = color;
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
