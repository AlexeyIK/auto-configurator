using System;
using System.Collections.Generic;
using System.Linq;
using Data.Model;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectsMenuController : PanelControllerBase
{
    public class ProjectItem
    {
        public int Id;
        public string Name;
        public string Commentary;
        public string CreatedAt;
        public string UpdatedAt;
    }

    private Button loadButton;
    private Button cancelButton;
    private StackPanelView stackPanel;

    private List<ProjectItem> projectsList;
    private ProjectItem selectedProject;

    public List<ProjectItem> ProjectsList
    {
        get { return projectsList; }
        set
        {
            projectsList = value;
            stackPanel.ItemsSource = projectsList;
        }
    }

    public event Action<Project> ProjectSelected;

    [SerializeField] private DropDownMenuController m_DropdownMenuController = default;

    protected override void Awake()
    {
        base.Awake();

        cancelButton = panel.Q<Button>("CancelBtn");
        loadButton = panel.Q<Button>("LoadBtn");
        stackPanel = panel.Q<StackPanelView>("ProjectsList");
        stackPanel.SelectedChange += OnSelectedProjectChange;

        cancelButton.clicked += OnCancelButtonClick;
        loadButton.clicked += OnLoadButtonClick;

        HidePanel();
    }

    private void Start()
    {
        m_DropdownMenuController.SubscribeOpenProjectClick(OnPanelOpen);
    }

    private void OnDestroy()
    {
        loadButton.clicked -= OnLoadButtonClick;
        cancelButton.clicked -= OnCancelButtonClick;
        stackPanel.SelectedChange -= OnSelectedProjectChange;
    }

    private async void OnPanelOpen()
    {
        m_DropdownMenuController.HidePanel();

        //var projects = new List<Project>()
        //{
        //    new Project { Id =1, Name = "123", Commentary = "rjvdfsf"},
        //    new Project { Id =2, Name = "123", Commentary = "rjvdfsf"},
        //    new Project { Id =3, Name = "123", Commentary = "rjvdfsf"},
        //    new Project { Id =4, Name = "123", Commentary = "rjvdfsf"},
        //    new Project { Id =5, Name = "123", Commentary = "rjvdfsf"},
        //};

        var projects = await NetworkManager.GetAsync<List<ProjectListDto>>("projects", TokenProvider.Instance.GetToken());
        if (projects == null)
        {
            HidePanel();
            return;
        }

        var projectItems = projects.Select(p => new ProjectItem
        {
            Id = p.Id,
            Name = p.Name,
            Commentary = p.Commentary,
            CreatedAt = p.CreatedAt.ToLocalTime().ToString("HH:mm dd:MM:yyyy"),
            UpdatedAt = p.UpdatedAt?.ToLocalTime().ToString("HH:mm dd:MM:yyyy") ?? "--"
        }).ToList();

        ProjectsList = projectItems;
        selectedProject = null;
        ShowPanel();
    }

    private async void OnLoadButtonClick()
    {
        HidePanel();

        if (await ProjectManager.Instance.LoadProject(selectedProject.Id))
            AppStateManager.Instance.State = AppStateManager.AppState.ProjectModification;

        ProjectsList = null;
    }

    private void OnCancelButtonClick()
    {
        ProjectsList = null;
        HidePanel();
    }

    private void OnSelectedProjectChange(VisualElement element)
    {
        loadButton.SetEnabled(element != null);

        if (selectedProject != element.dataSource)
            selectedProject = element.dataSource as ProjectItem;
    }

    protected override VisualElement GetPanelVisualElement()
    {
        return document.rootVisualElement.Q<VisualElement>("MyProjectsPopup").parent;
    }
}
