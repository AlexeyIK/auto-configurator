using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CathegoriesController : MonoBehaviour
{
    private UIDocument m_UIDocument;

    public List<CathegoryItemData> m_CathegoryItems;

    private void OnEnable()
    {
        m_UIDocument = GetComponent<UIDocument>();

        var listView = m_UIDocument.rootVisualElement.Q<ListView>("CathegoryList");
        listView.itemsSource = m_CathegoryItems;
    }
}
