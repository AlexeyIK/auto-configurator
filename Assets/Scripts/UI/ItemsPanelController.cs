using System;
using System.Collections.Generic;
using System.Linq;
using Data.Model;
using Data.ViewModel;
using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class ItemsPanelController : PanelControllerBase
{
    private List<PieceItemData> items = new();
    private Label titleText;
    private StackPanelView itemsList;

    private CategoryType selectedCategoryType;

    [SerializeField] private bool m_UseMockData = false;
    [SerializeField] private ItemsMock m_MockData = default;
    [SerializeField] private CategoriesPanelController m_CathegoriesController = default;
    [Header("Item Cards")]
    [SerializeField] private VisualTreeAsset CarItem = default;
    [SerializeField] private VisualTreeAsset ColorItem = default;
    [SerializeField] private VisualTreeAsset PieceItem = default;

    public List<PieceItemData> Items
    {
        get { return items; }
        set
        {
            items = value;
            itemsList.ItemsSource = Items;
        }
    }

    public event Action<PieceItemData, CategoryType> SelectedItemChange;

    protected override void Awake()
    {
        base.Awake();

        if (m_CathegoriesController = null)
            Debug.Log("Необходимо указать родительский контроллер типа <CathegoriesController>!");

        m_CathegoriesController = GetComponent<CategoriesPanelController>();

        titleText = document.rootVisualElement.Q<Label>("ItemsTitle");
        itemsList = document.rootVisualElement.Q<StackPanelView>("ItemsScrollView");
        itemsList.SelectedChange += OnItemSelectedChange;

        m_CathegoriesController.OnSelectedCathegoryChange += OnCathegoryChange;
        AppStateManager.Instance.StateChange += OnAppStateChange;
    }

    private void OnDestroy()
    {
        AppStateManager.Instance.StateChange -= OnAppStateChange;
        m_CathegoriesController.OnSelectedCathegoryChange -= OnCathegoryChange;
    }

    private void OnAppStateChange(AppStateManager.AppState state)
    {
        switch (state)
        {
            case AppStateManager.AppState.Initial:
            case AppStateManager.AppState.NetworkError:
                HidePanel();
                break;

            case AppStateManager.AppState.Start:
                selectedCategoryType = CategoryType.Automobiles;
                ShowAutomobileSelector();
                break;

            case AppStateManager.AppState.ProjectModification:
                HidePanel();
                break;

        }
    }

    private void OnCathegoryChange(CategoryItemData selectedCategory)
    {
        if (selectedCategory == null)
        {
            HidePanel();
        }
        else
        {
            selectedCategoryType = selectedCategory.Type;

            switch (selectedCategory.Type)
            {
                case CategoryType.Automobiles:
                    itemsList.itemsElement = CarItem;
                    break;

                case CategoryType.Colors:
                    itemsList.itemsElement = ColorItem;
                    break;

                default:
                    itemsList.itemsElement = PieceItem;
                    break;
            }

            if (m_UseMockData)
                Items = m_MockData.Items.FirstOrDefault(c => c.CathegoryType == selectedCategory.Type).Items;
            else
                GetCategoryData();

            ShowPanel();
            titleText.text = selectedCategory.Caption;
        }
    }

    private void ShowAutomobileSelector()
    {
        titleText.text = "Автомобили";
        itemsList.itemsElement = CarItem;

        if (m_UseMockData)
        {
            Items = m_MockData.Items.FirstOrDefault(c => c.CathegoryType == CategoryType.Automobiles).Items;
            Items.ForEach(i => i.TryLoadImage(i.ImageUrl));
        }
        else
            GetAutomobilesAsync();

        ShowPanel();
    }

    private void OnItemSelectedChange(VisualElement element)
    {
        if (element == null)
        {
            SelectedItemChange?.Invoke(null, selectedCategoryType);
            return;
        }

        if (element.dataSource is PieceItemData pieceItemData)
            SelectedItemChange?.Invoke(pieceItemData, selectedCategoryType);
    }

    private async void GetCategoryData()
    {
        Debug.Log("Requesting items for category: " + selectedCategoryType.ToString());
        if (selectedCategoryType == CategoryType.Colors)
        {
            var colors = await NetworkManager.GetAsync<List<Data.Model.Color>>("colors", TokenProvider.Instance.GetToken());
            var items = new List<PieceItemData>();
            foreach (var color in colors)
                items.Add(new PieceItemData(color.Id, CategoryType.Colors, color.Name, null, color.Image));

            Items = items;
        }
        else
        {

        }
    }

    private async void GetAutomobilesAsync()
    {
        var automobiles = await NetworkManager.GetAsync<List<Automobile>>("automobiles", TokenProvider.Instance.GetToken());

        var items = new List<PieceItemData>();
        foreach (var auto in automobiles)
            items.Add(new PieceItemData(auto.Id, CategoryType.Automobiles, auto.AutoBrand.Name, auto.Name, auto.ImageUrl));

        Items = items;
    }

    protected override void HidePanel()
    {
        base.HidePanel();
        document.rootVisualElement.Q<Label>("ItemsTitle").text = "";
    }

    protected override void ShowPanel()
    {
        base.ShowPanel();
    }

    protected override VisualElement GetPanelVisualElement()
    {
        itemsList = document.rootVisualElement.Q<StackPanelView>("ItemsScrollView");
        return itemsList.parent;
    }
}

