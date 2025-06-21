using System;
using UnityEngine.UIElements;

public class NewProjectPopup : PanelControllerBase
{
    private VisualElement popup;
    private Button startProjectButton;
    private Button cancelButton;
    private Button submitButton;

    private TextField textField;

    protected override void Awake()
    {
        base.Awake();

        HidePanel();

        startProjectButton = document.rootVisualElement.Q<Button>("StartProjectBtn");
        cancelButton = panel.Q<Button>("CancelBtn");
        submitButton = panel.Q<Button>("SubmitBtn");
        textField = panel.Q<TextField>();

        startProjectButton.clicked += OnStartProjectClick;
        cancelButton.clicked += OnCancelBtnClick;
        submitButton.clicked += OnSubmitBtnClick;
    }

    private void OnDestroy()
    {
        startProjectButton.clicked -= OnStartProjectClick;
    }

    private void OnSubmitBtnClick()
    {
        // make request
        AppStateManager.Instance.State = AppStateManager.AppState.ProjectModification;
        HidePanel();
    }

    private void OnCancelBtnClick()
    {
        //textField.Clear();
        HidePanel();
    }

    private void OnStartProjectClick()
    {
        ShowPanel();
    }

    protected override VisualElement GetPanelVisualElement()
    {
        popup = document.rootVisualElement.Q<VisualElement>("NewProjectPopup");
        return popup.parent;
    }
}
