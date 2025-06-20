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
    private UIDocument _UIDocument;
    private Label _titleText;
    private StackPanelView _itemsList;

    [SerializeField] private bool m_UseMockData = false;
    [SerializeField] private ItemsMock m_MockData = default;
    [SerializeField] private CategoriesPanelController m_CathegoriesController = default;
    [Header("Item Cards")]
    [SerializeField] private VisualTreeAsset CarItem = default;
    [SerializeField] private VisualTreeAsset ColorItem = default;
    [SerializeField] private VisualTreeAsset PieceItem = default;

    public List<PieceItemData> Items = new();

    protected override void Awake()
    {
        base.Awake();

        if (m_CathegoriesController = null)
            Debug.Log("Необходимо указать родительский контроллер типа <CathegoriesController>!");

        m_CathegoriesController = GetComponent<CategoriesPanelController>();

        _itemsList = _UIDocument.rootVisualElement.Q<StackPanelView>("ItemsScrollView");

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
            if (m_UseMockData)
            {
                switch (selectedCategory.Type)
                {
                    case CategoryType.Automobiles:
                        _itemsList.itemsElement = CarItem;
                        break;

                    case CategoryType.Body:
                        _itemsList.itemsElement = ColorItem;
                        break;

                    default:
                        _itemsList.itemsElement = PieceItem;
                        break;
                }

                Items = m_MockData.Items.FirstOrDefault(c => c.CathegoryType == selectedCategory.Type).Items;
            }
            else
                GetCategoryData();

            _titleText.text = selectedCategory.Caption;
        }
    }

    private void ShowAutomobileSelector()
    {
        _titleText.text = "Автомобили";
        GetAutomobilesAsync();
        ShowPanel();
    }

    private void GetCategoryData()
    {
        throw new NotImplementedException();
    }

    private async void GetAutomobilesAsync()
    {
        var automobiles = await NetworkManager.GetAsync<List<Automobile>>("automobiles", TokenProvider.Instance.GetToken());

        var items = new List<PieceItemData>();
        foreach (var auto in automobiles)
            items.Add(new PieceItemData(auto.Id, CategoryType.Automobiles, auto.AutoBrand.Name, auto.Name, auto.ImageUrl));

        Items = items;
        _itemsList.itemsElement = CarItem;
        _itemsList.ItemsSource = Items;
    }

    protected override void HidePanel()
    {
        base.HidePanel();
        _UIDocument.rootVisualElement.Q<Label>("ItemsTitle").text = "";
    }

    protected override void ShowPanel()
    {
        base.ShowPanel();
    }

    protected override VisualElement GetPanelVisualElement()
    {
        _UIDocument = GetComponent<UIDocument>();
        _titleText = _UIDocument.rootVisualElement.Q<Label>("ItemsTitle");
        return _UIDocument.rootVisualElement.Q("ItemsScrollView");
    }

    protected override void OnBackgroundClick(GameObject go)
    {
        throw new NotImplementedException();
    }
}

