using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class CathegoriesController : MonoBehaviour
{
    private UIDocument _UIDocument;
    private ListView _cathegoriesList;
    private ListView _itemsList;

    [SerializeField] private bool useMockData = false;
    [SerializeField] private CathegoriesMock mockData = default;
    [SerializeField] private InteractionHandler missClickHandler = default;

    public List<CathegoryItemData> cathegoryItems;
    public CathegoryItemData selectedCathegory;

    private void OnEnable()
    {
        _UIDocument = GetComponent<UIDocument>();

        if (useMockData)
            cathegoryItems = mockData.CathegoryItems;
        else
            GetData();

        _cathegoriesList = _UIDocument.rootVisualElement.Q<ListView>("CathegoryList");
        _cathegoriesList.itemsSource = cathegoryItems;
        _cathegoriesList.selectionChanged += OnCathegorySelection;

        _itemsList = _UIDocument.rootVisualElement.Q<ListView>("ItemsList");
        _itemsList.parent.style.display = DisplayStyle.None;

        missClickHandler.OnClick.AddListener(OnBackgroundClick);
    }

    public void SelectByType(CathegoryType cathegoryType)
    {
        var list = _cathegoriesList.itemsSource as List<CathegoryItemData>;
        selectedCathegory = list.FirstOrDefault(c => c.Type == cathegoryType);
        if (selectedCathegory != null)
        {
            _cathegoriesList.SetSelection(list.IndexOf(selectedCathegory));
        }
    }

    private void OnCathegorySelection(IEnumerable<object> enumerable)
    {
        if (enumerable.Count() == 0)
        {
            var container = _cathegoriesList.Q<VisualElement>("unity-content-container");
            var elem = container.ElementAt(cathegoryItems.IndexOf(selectedCathegory));
            elem.Children().First().RemoveFromClassList("list-item-selected");
            selectedCathegory = null;
            HideItemsPanel();
            return;
        }

        if (enumerable.FirstOrDefault() is CathegoryItemData cathegoryItem)
        {
            var container = _cathegoriesList.Q<VisualElement>("unity-content-container");

            if (selectedCathegory != null && cathegoryItems.Count > 0)
            {
                var oldElem = container.ElementAt(_cathegoriesList.selectedIndex);
                oldElem.RemoveFromClassList("list-item-selected");
            }

            selectedCathegory = cathegoryItem;

            container = _cathegoriesList.Q<VisualElement>("unity-content-container");
            var elem = container.ElementAt(_cathegoriesList.selectedIndex);
            elem.Children().First().AddToClassList("list-item-selected");
            ShowItemsPanel();
        }

        foreach (CathegoryItemData item in enumerable)
            Debug.Log($"Selected item id: {item.Id}");
    }

    private void OnBackgroundClick(GameObject go)
    {
        _cathegoriesList.ClearSelection();
    }

    private void GetData()
    {
        throw new NotImplementedException();
    }

    private void HideItemsPanel()
    {
        _itemsList.parent.style.display = DisplayStyle.None;
        _UIDocument.rootVisualElement.Q<Label>("ItemsTitle").text = "";
    }

    private void ShowItemsPanel()
    {
        _itemsList.parent.style.display = DisplayStyle.Flex;
        _UIDocument.rootVisualElement.Q<Label>("ItemsTitle").text = selectedCathegory.Caption;
    }
}
