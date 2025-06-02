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

    private CathegoryItemData _selectedCathegory;

    [SerializeField] private bool m_UseMockData = false;
    [SerializeField] private CathegoriesMock m_MockData = default;
    [SerializeField] private InteractionHandler m_MissClickHandler = default;

    public List<CathegoryItemData> CathegoryItems = new();
    public CathegoryItemData SelectedCathegory
    {
        get { return _selectedCathegory; }
        set
        {
            _selectedCathegory = value;
            OnSelectedCathegoryChange?.Invoke(_selectedCathegory);
        }
    }

    public event Action<CathegoryItemData> OnSelectedCathegoryChange;

    private void Awake()
    {
        _UIDocument = GetComponent<UIDocument>();

        if (m_UseMockData)
            CathegoryItems = m_MockData.CathegoryItems;
        else
            GetData();

        _cathegoriesList = _UIDocument.rootVisualElement.Q<ListView>("CathegoryList");
        _cathegoriesList.itemsSource = CathegoryItems;
        _cathegoriesList.selectionChanged += OnCathegorySelection;

        m_MissClickHandler.OnClick.AddListener(OnBackgroundClick);
    }

    public void SelectByType(CathegoryType cathegoryType)
    {
        var list = _cathegoriesList.itemsSource as List<CathegoryItemData>;
        SelectedCathegory = list.FirstOrDefault(c => c.Type == cathegoryType);
        if (SelectedCathegory != null)
        {
            _cathegoriesList.SetSelection(list.IndexOf(SelectedCathegory));
        }
    }

    private void OnCathegorySelection(IEnumerable<object> enumerable)
    {
        if (enumerable.Count() == 0)
        {
            var container = _cathegoriesList.Q<VisualElement>("unity-content-container");
            var elem = container.ElementAt(CathegoryItems.IndexOf(SelectedCathegory));
            elem.Children().First().RemoveFromClassList("list-item-selected");

            SelectedCathegory = null;
            return;
        }

        if (enumerable.FirstOrDefault() is CathegoryItemData cathegoryItem)
        {
            var container = _cathegoriesList.Q<VisualElement>("unity-content-container");

            if (SelectedCathegory != null && CathegoryItems.Count > 0)
            {
                var oldElem = container.ElementAt(_cathegoriesList.selectedIndex);
                oldElem.RemoveFromClassList("list-item-selected");
            }

            container = _cathegoriesList.Q<VisualElement>("unity-content-container");
            var elem = container.ElementAt(_cathegoriesList.selectedIndex);
            elem.Children().First().AddToClassList("list-item-selected");

            SelectedCathegory = cathegoryItem;
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
}
