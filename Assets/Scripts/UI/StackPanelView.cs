using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class StackPanelView : ScrollView
{
    public enum Direction
    {
        Horizontal,
        Vertical
    }

    private IList m_itemsSource;

    public static readonly BindingId itemsSourceProperty = nameof(ItemsSource);

    [UxmlAttribute] public Direction direction { get; set; } = Direction.Horizontal;
    [UxmlAttribute] public VisualTreeAsset itemsElement { get; set; } = null;
    [UxmlAttribute] public float gap { get; set; } = 0;

    [CreateProperty]
    public IList ItemsSource
    {
        get { return m_itemsSource; }
        set
        {
            if (m_itemsSource == value)
                return;

            m_itemsSource = value;
            NotifyPropertyChanged(itemsSourceProperty);
            ArrangeList();
        }
    }

    public StackPanelView() : this(null) { }

    public StackPanelView(IList items)
    {
        ItemsSource = items;
        RegisterCallback<AttachToPanelEvent>(OnAttachEvent);
    }

    private void ArrangeList()
    {
        Clear();

        if (ItemsSource?.Count > 0)
        {
            Debug.Log("Items in data source: " + ItemsSource.Count);

            if (itemsElement == null)
            {
                Debug.LogError("<itemsElement> reference must be assigned!");
                return;
            }

            for (var i = 0; i < ItemsSource.Count; i++)
            {
                var elem = itemsElement.Instantiate();
                elem.dataSource = ItemsSource[i];
                Add(elem);

                if (gap > 0 && i < ItemsSource.Count - 1)
                {
                    var gapElement = new VisualElement();
                    if (direction == Direction.Horizontal)
                    {
                        gapElement.style.width = gap;
                        gapElement.style.height = contentRect.height;
                    }
                    else
                    {
                        gapElement.style.width = contentRect.width;
                        gapElement.style.height = gap;
                    }

                    Add(gapElement);
                }
            }
        }
    }

    private void OnAttachEvent(AttachToPanelEvent evt)
    {
        ArrangeList();
    }
}