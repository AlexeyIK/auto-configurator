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
    }

    private void OnDestroy()
    {
        textField.UnregisterValueChangedCallback(OnProjectNameChange);
        ProjectManager.Instance.CarSet -= OnCarSet;
        submitButton.clicked -= OnSubmitBtnClick;
        cancelButton.clicked -= OnCancelBtnClick;
        startProjectButton.clicked -= OnStartProjectClick;
    }

    private void OnProjectNameChange(ChangeEvent<string> evt) => submitButton.SetEnabled(!String.IsNullOrWhiteSpace(evt.newValue));

    private void OnSubmitBtnClick()
    {
        // ToDo: make creation request


        AppStateManager.Instance.State = AppStateManager.AppState.ProjectModification;
        startProjectButton.style.display = DisplayStyle.None;
        HidePanel();
    }

    private void OnCarSet()
    {
        startProjectButton.SetEnabled(true);
    }

    private void OnCancelBtnClick()
    {
        //textField.Clear();
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
