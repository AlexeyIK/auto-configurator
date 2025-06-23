using System.Linq;
using Data.Model;
using UnityEngine;

[RequireComponent(typeof(ColorChanger), typeof(CategorySelector))]
public class Car : MonoBehaviour
{
    private ColorChanger m_ColorChanger;
    private CategorySelector[] m_CategorySelectors;

    private Automobile data;

    [SerializeField] private int m_CarID = 1;
    [SerializeField] private int m_ColorId = default;
    [SerializeField] private bool m_OverrideColor = false;

    public Automobile Data => data;
    public int CarId => m_CarID;
    public int ColorId => m_ColorId;

    private void OnEnable()
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
        AppStateManager.Instance.SubscribeStateChange(OnStateChange);
    }

    private void OnDestroy()
    {
        AppStateManager.Instance.UnsubscriveStateChange(OnStateChange);
    }

    public async void SetDataContext(Automobile dataContext)
    {
        data = dataContext;
        m_CarID = data.Id;

        var colors = await ColorsRequest.GetColors();
        ChangeColorTo(colors.FirstOrDefault(c => c.Id == m_ColorId));
    }

    public void ChangeColorTo(Data.Model.Color colorData)
    {
        if (ColorUtility.TryParseHtmlString($"#{colorData.HexCode}", out var color))
            m_ColorChanger.ChangeColor(color);

        m_OverrideColor = true;
        m_ColorId = colorData.Id;
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
