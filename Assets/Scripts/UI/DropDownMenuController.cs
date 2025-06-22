using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

public class DropDownMenuController : PanelControllerBase
{
    private bool isOpen = false;
    private Button openButton;
    private List<Button> buttons;

    private void Start()
    {
        AppStateManager.Instance.StateChange += OnStateChange;
        openButton.clicked += OnOpenButtonClick;

        RefreshState();
    }

    private void OnDestroy()
    {
        openButton.clicked -= OnOpenButtonClick;
        AppStateManager.Instance.StateChange -= OnStateChange;
    }

    private void OnOpenButtonClick()
    {
        isOpen = !isOpen;
        RefreshState();
    }

    protected override VisualElement GetPanelVisualElement()
    {
        openButton = document.rootVisualElement.Q<Button>("OpenMenuButton");
        var query = document.rootVisualElement.Query<Button>(className: "drop-down-item");
        buttons = query.ToList();
        return document.rootVisualElement.Q<VisualElement>("DropDownPanel");
    }

    private void OnStateChange(AppStateManager.AppState state)
    {
        if (state != AppStateManager.AppState.ProjectModification)
        {
            buttons[1].SetEnabled(false);
        }
        else
        {
            buttons[1].SetEnabled(true);
        }
    }

    private void RefreshState()
    {
        if (isOpen)
            ShowPanel();
        else
            HidePanel();
    }
}
