using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class DropDownMenuController : PanelControllerBase
{
    private bool isOpen = false;
    private Button openButton;
    private List<Button> buttons;

    public bool IsOpen => isOpen;

    private void Start()
    {
        AppStateManager.Instance.SubscribeStateChange(OnStateChange);
        openButton.clicked += OnOpenButtonClick;

        RefreshState();
    }

    private void OnDestroy()
    {
        AppStateManager.Instance.UnsubscriveStateChange(OnStateChange);
    }

    public void SubscribeSaveProjectClick(Action action)
    {
        buttons[0].clicked += action;
    }

    public void SubscribeOpenProjectClick(Action action)
    {
        buttons[1].clicked += action;
    }

    public void SubscribeNewProjectClick(Action action)
    {
        buttons[2].clicked += action;
    }

    public void SetSaveButtonActive(bool isActive)
    {
        buttons[0].SetEnabled(isActive);
    }

    private void OnStateChange(AppStateManager.AppState state)
    {
        if (state != AppStateManager.AppState.ProjectModification)
        {
            SetSaveButtonActive(false);
        }
        else
        {
            SetSaveButtonActive(true);
        }
    }

    private void OnOpenButtonClick()
    {
        isOpen = !isOpen;
        RefreshState();
    }

    public override void HidePanel()
    {
        base.HidePanel();
        isOpen = false;
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        isOpen = true;
    }

    private void RefreshState()
    {
        if (isOpen)
            ShowPanel();
        else
            HidePanel();
    }

    protected override VisualElement GetPanelVisualElement()
    {
        openButton = document.rootVisualElement.Q<Button>("OpenMenuButton");
        var query = document.rootVisualElement.Query<Button>(className: "drop-down-item");
        buttons = query.ToList();
        return document.rootVisualElement.Q<VisualElement>("DropDownPanel");
    }
}
