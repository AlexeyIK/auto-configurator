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

    //private CategoryItemData selectedCategory;

    [SerializeField] private bool m_UseMockData = false;
    [SerializeField] private ItemsMock m_MockData = default;
    [SerializeField] private CategoriesPanelController m_CategoriesController = default;
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

    public event Action<PieceItemData, CategoryItemData> SelectedItemChange;

    protected override void Awake()
    {
        base.Awake();

        if (m_CategoriesController = null)
            Debug.Log("Необходимо указать родительский контроллер типа <CathegoriesController>!");

        m_CategoriesController = GetComponent<CategoriesPanelController>();

        titleText = document.rootVisualElement.Q<Label>("ItemsTitle");
        itemsList = document.rootVisualElement.Q<StackPanelView>("ItemsScrollView");
        itemsList.SelectedChange += OnItemSelectedChange;

        m_CategoriesController.SelectedCategoryChange += OnCategoryChange;
        AppStateManager.Instance.StateChange += OnAppStateChange;
    }

    private void OnDestroy()
    {
        AppStateManager.Instance.StateChange -= OnAppStateChange;
        m_CategoriesController.SelectedCategoryChange -= OnCategoryChange;
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
                ShowAutomobileSelector();
                break;

            case AppStateManager.AppState.ProjectModification:
                HidePanel();
                break;

        }
    }

    private void OnCategoryChange(CategoryItemData selectedCategory)
    {
        if (selectedCategory == null)
        {
            HidePanel();
        }
        else
        {

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
            SelectedItemChange?.Invoke(null, m_CategoriesController.SelectedCategory);
            return;
        }

        if (element.dataSource is PieceItemData pieceItemData)
            SelectedItemChange?.Invoke(pieceItemData, m_CategoriesController.SelectedCategory);
    }

    private async void GetCategoryData()
    {
        Debug.Log("Requesting items for category: " + m_CategoriesController.SelectedCategory.Type.ToString());
        if (m_CategoriesController.SelectedCategory.Type == CategoryType.Colors)
        {
            var colors = await NetworkManager.GetAsync<List<Data.Model.Color>>("colors", TokenProvider.Instance.GetToken());
            var items = new List<PieceItemData>();
            foreach (var color in colors)
                items.Add(new PieceItemData(color.Id, CategoryType.Colors, color.Name, color.HexCode, color.Image));

            Items = items;
        }
        else
        {
            var pieces = await NetworkManager.GetAsync<List<Piece>>($"pieces?categoryId={m_CategoriesController.SelectedCategory.Id}" +
                                                                    $"&automobileId={ProjectManager.Instance.CurrentCar.CarId}",
                                                                    TokenProvider.Instance.GetToken());
            var items = new List<PieceItemData>();
            foreach (var piece in pieces)
                items.Add(new PieceItemData(piece.Id, m_CategoriesController.SelectedCategory.Type, piece.Name, piece.Manufacturer.Name, piece.Image));

            Items = items;
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
        Items = new List<PieceItemData>();
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

