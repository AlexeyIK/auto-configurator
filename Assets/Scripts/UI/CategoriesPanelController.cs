using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class CategoriesPanelController : PanelControllerBase
{
    private ListView cathegoriesList;

    private CategoryItemData _selectedCathegory;

    [SerializeField] private bool m_UseMockData = false;
    [SerializeField] private CategoriesMock m_MockData = default;
    [SerializeField] private InteractionHandler m_MissClickHandler = default;

    public List<CategoryItemData> CathegoryItems = new();
    public CategoryItemData SelectedCathegory
    {
        get { return _selectedCathegory; }
        set
        {
            _selectedCathegory = value;
            OnSelectedCathegoryChange?.Invoke(_selectedCathegory);
        }
    }

    public event Action<CategoryItemData> OnSelectedCathegoryChange;

    protected override void Awake()
    {
        base.Awake();

        if (m_UseMockData)
            CathegoryItems = m_MockData.CathegoryItems;
        else
            GetData();

        cathegoriesList.itemsSource = CathegoryItems;
        cathegoriesList.selectionChanged += OnCathegorySelection;

        m_MissClickHandler.OnClick.AddListener(OnBackgroundClick);

        AppStateManager.Instance.StateChange += OnStateChange;
    }

    private void OnDestroy()
    {
        AppStateManager.Instance.StateChange -= OnStateChange;
        m_MissClickHandler.OnClick.RemoveListener(OnBackgroundClick);
        cathegoriesList.selectionChanged -= OnCathegorySelection;
    }

    public void SelectByType(CategoryType cathegoryType)
    {
        var list = cathegoriesList.itemsSource as List<CategoryItemData>;
        SelectedCathegory = list.FirstOrDefault(c => c.Type == cathegoryType);
        if (SelectedCathegory != null)
        {
            cathegoriesList.SetSelection(list.IndexOf(SelectedCathegory));
        }
    }

    private void OnCathegorySelection(IEnumerable<object> enumerable)
    {
        if (enumerable.Count() == 0)
        {
            var container = cathegoriesList.Q<VisualElement>("unity-content-container");
            var elem = container.ElementAt(CathegoryItems.IndexOf(SelectedCathegory));
            elem.Children().First().RemoveFromClassList("list-item-selected");

            SelectedCathegory = null;
            return;
        }

        if (enumerable.FirstOrDefault() is CategoryItemData cathegoryItem)
        {
            var container = cathegoriesList.Q<VisualElement>("unity-content-container");

            if (SelectedCathegory != null && CathegoryItems.Count > 0)
            {
                var oldElem = container.ElementAt(cathegoriesList.selectedIndex);
                oldElem.RemoveFromClassList("list-item-selected");
            }

            container = cathegoriesList.Q<VisualElement>("unity-content-container");
            var elem = container.ElementAt(cathegoriesList.selectedIndex);
            elem.Children().First().AddToClassList("list-item-selected");

            SelectedCathegory = cathegoryItem;
        }

        foreach (CategoryItemData item in enumerable)
            Debug.Log($"Selected item id: {item.Id}");
    }

    private void GetData()
    {
        throw new NotImplementedException();
    }

    protected override void OnBackgroundClick(GameObject go)
    {
        base.OnBackgroundClick(go);
        cathegoriesList.ClearSelection();
    }

    protected override VisualElement GetPanelVisualElement()
    {
        cathegoriesList = document.rootVisualElement.Q<ListView>("CathegoryList");
        return cathegoriesList.parent;
    }

    private void OnStateChange(AppStateManager.AppState state)
    {
        switch (state)
        {
            case AppStateManager.AppState.Initial:
            case AppStateManager.AppState.NetworkError:
                HidePanel();
                break;

            case AppStateManager.AppState.Start:
                HidePanel();
                break;

            case AppStateManager.AppState.ProjectModification:
                ShowPanel();
                break;
        }
    }
}
