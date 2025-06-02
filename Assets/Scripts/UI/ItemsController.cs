using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class ItemsController : MonoBehaviour
{
    private UIDocument _UIDocument;
    private ListView _itemsList;

    [SerializeField] private bool m_UseMockData = false;
    [SerializeField] private ItemsMock m_MockData = default;
    [SerializeField] private CathegoriesController m_CathegoriesController = default;

    public List<ItemsData> Items = new();

    private void Awake()
    {
        _UIDocument = GetComponent<UIDocument>();

        if (m_CathegoriesController = null)
            Debug.Log("Необходимо указать родительский контроллер типа <CathegoriesController>!");

        m_CathegoriesController = GetComponent<CathegoriesController>();

        _itemsList = _UIDocument.rootVisualElement.Q<ListView>("ItemsList");
        _itemsList.dataSource = Items;

        HidePanel();

        m_CathegoriesController.OnSelectedCathegoryChange += OnCathegoryChange;
    }

    private void OnDestroy()
    {
        m_CathegoriesController.OnSelectedCathegoryChange -= OnCathegoryChange;
    }

    private void OnCathegoryChange(CathegoryItemData selectedCathegory)
    {
        if (selectedCathegory == null)
        {
            HidePanel();
        }
        else
        {
            if (m_UseMockData)
                Items = m_MockData.Items.FirstOrDefault(c => c.CathegoryType == selectedCathegory.Type).Items;
            else
                GetData();

            ShowPanel(selectedCathegory.Caption);
        }
    }

    private void GetData()
    {
        throw new NotImplementedException();
    }

    private void HidePanel()
    {
        _itemsList.parent.style.display = DisplayStyle.None;
        _UIDocument.rootVisualElement.Q<Label>("ItemsTitle").text = "";
    }

    private void ShowPanel(string caption)
    {
        _itemsList.parent.style.display = DisplayStyle.Flex;
        _itemsList.itemsSource = Items;
        _UIDocument.rootVisualElement.Q<Label>("ItemsTitle").text = caption;
    }
}

