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
        textField.RegisterValueChangedCallback(OnProjectNameChange);

        startProjectButton.clicked += OnStartProjectClick;
        cancelButton.clicked += OnCancelBtnClick;
        submitButton.clicked += OnSubmitBtnClick;

        startProjectButton.SetEnabled(false);
    }

    private void Start()
    {
        ProjectManager.Instance.CarSet += OnCarSet;
        AppStateManager.Instance.SubscribeStateChange(OnAppStateChange);
    }

    private void OnDestroy()
    {
        AppStateManager.Instance.UnsubscriveStateChange(OnAppStateChange);
        textField.UnregisterValueChangedCallback(OnProjectNameChange);
        ProjectManager.Instance.CarSet -= OnCarSet;

        submitButton.clicked -= OnSubmitBtnClick;
        cancelButton.clicked -= OnCancelBtnClick;
        startProjectButton.clicked -= OnStartProjectClick;
    }

    private void OnAppStateChange(AppStateManager.AppState state)
    {
        switch (state)
        {
            case AppStateManager.AppState.Start:
                startProjectButton.style.display = DisplayStyle.Flex;
                startProjectButton.SetEnabled(false);
                break;

            case AppStateManager.AppState.ProjectModification:
            case AppStateManager.AppState.NetworkError:
                startProjectButton.style.display = DisplayStyle.None;
                textField.Clear();
                HidePanel();
                break;
        }
    }

    private void OnProjectNameChange(ChangeEvent<string> evt) => submitButton.SetEnabled(!String.IsNullOrWhiteSpace(evt.newValue));

    private async void OnSubmitBtnClick()
    {
        var hasCreated = await ProjectManager.Instance.CreateProject(textField.value);
        if (!hasCreated)
        {
            AppStateManager.Instance.State = AppStateManager.AppState.NetworkError;
            return;
        }

        AppStateManager.Instance.State = AppStateManager.AppState.ProjectModification;
    }

    private void OnCarSet()
    {
        startProjectButton.SetEnabled(true);
    }

    private void OnCancelBtnClick()
    {
        HidePanel();
    }

    private void OnStartProjectClick()
    {
        ShowPanel();
        textField.value = $"Мой {ProjectManager.Instance.CurrentCar.Data.AutoBrand.Name} {ProjectManager.Instance.CurrentCar.Data.Name}";
    }

    protected override VisualElement GetPanelVisualElement()
    {
        popup = document.rootVisualElement.Q<VisualElement>("NewProjectPopup");
        return popup.parent;
    }
}
