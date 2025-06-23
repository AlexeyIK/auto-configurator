using System.Collections.Generic;
using UnityEngine.UIElements;

public class DropDownMenuController : PanelControllerBase
{
    private bool isOpen = false;
    private Button openButton;
    private List<Button> buttons;

    private void Start()
    {
        AppStateManager.Instance.SubscribeStateChange(OnStateChange);
        openButton.clicked += OnOpenButtonClick;

        RefreshState();
    }

    private void OnDestroy()
    {
        openButton.clicked -= OnOpenButtonClick;
        AppStateManager.Instance.UnsubscriveStateChange(OnStateChange);
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
