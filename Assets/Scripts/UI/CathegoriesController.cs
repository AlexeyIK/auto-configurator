using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CathegoriesController : MonoBehaviour
{
    private UIDocument m_UIDocument;
    private ListView m_CathegoriesList;
    private ListView m_ItemsList;

    public List<CathegoryItemData> m_CathegoryItems;

    private void OnEnable()
    {
        m_UIDocument = GetComponent<UIDocument>();

        m_CathegoriesList = m_UIDocument.rootVisualElement.Q<ListView>("CathegoryList");
        m_CathegoriesList.itemsSource = m_CathegoryItems;
        m_CathegoriesList.selectionChanged += OnCathegorySelection;

        m_ItemsList = m_UIDocument.rootVisualElement.Q<ListView>("ItemsList");
        m_ItemsList.parent.style.display = DisplayStyle.None;
    }

    private void OnCathegorySelection(IEnumerable<object> enumerable)
    {
        if (enumerable != null)
            m_ItemsList.parent.style.display = DisplayStyle.Flex;

        foreach (CathegoryItemData item in enumerable)
            Debug.Log($"Selected item id: {item.Id}");
    }
}
