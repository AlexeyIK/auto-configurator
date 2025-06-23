using System;
using System.Collections.Generic;
using System.Linq;
using Data.Model;
using Data.ViewModel;
using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class CategoriesPanelController : PanelControllerBase
{
    private ListView categoriesList;
    private List<CategoryItemData> categoryItems = new();

    private CategoryItemData selectedCategory;

    [SerializeField] private bool m_UseMockData = false;
    [SerializeField] private CategoriesMock m_MockData = default;
    [SerializeField] private InteractionHandler m_MissClickHandler = default;

    public List<CategoryItemData> CategoryItems
    {
        get { return categoryItems; }
        set
        {
            categoryItems = value;
            categoriesList.itemsSource = categoryItems;
        }
    }

    public CategoryItemData SelectedCategory
    {
        get { return selectedCategory; }
        set
        {
            selectedCategory = value;
            SelectedCategoryChange?.Invoke(selectedCategory);
        }
    }

    public event Action<CategoryItemData> SelectedCategoryChange;

    protected override void Awake()
    {
        base.Awake();

        AppStateManager.Instance.StateChange += OnStateChange;
        categoriesList.selectionChanged += OnCategorySelection;
        m_MissClickHandler.OnClick.AddListener(OnBackgroundClick);

        if (m_UseMockData)
            CategoryItems = m_MockData.CathegoryItems;
    }

    private void OnDestroy()
    {
        m_MissClickHandler.OnClick.RemoveListener(OnBackgroundClick);
        categoriesList.selectionChanged -= OnCategorySelection;
        AppStateManager.Instance.StateChange -= OnStateChange;
    }

    public void SelectByType(CategoryType categoryType)
    {
        var list = categoriesList.itemsSource as List<CategoryItemData>;
        SelectedCategory = list?.FirstOrDefault(c => c.Type == categoryType);
        if (SelectedCategory != null)
        {
            categoriesList.SetSelection(list.IndexOf(SelectedCategory));
        }
    }

    private void OnCategorySelection(IEnumerable<object> enumerable)
    {
        if (enumerable.Count() == 0)
        {
            var container = categoriesList.Q<VisualElement>("unity-content-container");
            var elem = container.ElementAt(CategoryItems.IndexOf(SelectedCategory));
            elem.Children().First().RemoveFromClassList("list-item-selected");

            SelectedCategory = null;
            return;
        }

        if (enumerable.FirstOrDefault() is CategoryItemData cathegoryItem)
        {
            var container = categoriesList.Q<VisualElement>("unity-content-container");

            if (SelectedCategory != null && CategoryItems.Count > 0)
            {
                var oldElem = container.ElementAt(categoriesList.selectedIndex);
                oldElem.RemoveFromClassList("list-item-selected");
            }

            container = categoriesList.Q<VisualElement>("unity-content-container");
            var elem = container.ElementAt(categoriesList.selectedIndex);
            elem.Children().First().AddToClassList("list-item-selected");

            SelectedCategory = cathegoryItem;
        }

        foreach (CategoryItemData item in enumerable)
            Debug.Log($"Selected item id: {item.Id}");
    }

    private async void GetData()
    {
        var categories = await NetworkManager.GetAsync<List<Category>>("categories", TokenProvider.Instance.GetToken());

        var items = new List<CategoryItemData>();
        foreach (var category in categories)
            items.Add(new CategoryItemData(category.Id, category.Type, category.Name, category.Image));

        CategoryItems = items;
    }

    protected override void OnBackgroundClick(GameObject go)
    {
        base.OnBackgroundClick(go);
        categoriesList.ClearSelection();
    }

    protected override VisualElement GetPanelVisualElement()
    {
        categoriesList = document.rootVisualElement.Q<ListView>("CathegoryList");
        return categoriesList.parent;
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
                GetData();
                ShowPanel();
                break;
        }
    }
}
