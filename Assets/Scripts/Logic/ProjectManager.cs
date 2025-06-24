using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using Newtonsoft.Json;
using UnityEngine;

public class ProjectManager : MonoBehaviour
{
    [Serializable]
    public struct ModifiedGroup
    {
        public CategoryType Type;
        public Modification Modification;

        public ModifiedGroup(CategoryType type, Modification modification)
        {
            Type = type;
            Modification = modification;
        }
    }

    private Car currentCar;
    private Project projectData;
    private bool isSaved = true;

    [SerializeField] private ItemsPanelController m_ItemsPanel = default;
    [SerializeField] private CarsLoader m_CarsLoader = default;
    [SerializeField] private PieceAttachMaster m_AttachMaster = default;
    [SerializeField] private DropDownMenuController m_DropDownMenu = default;
    [Header("Project data")]
    [SerializeField] private string m_ProjectName = "Имя проекта";
    [TextArea(2, 3)]
    [SerializeField] private string m_Commentary = "Комментарий от менеджера";
    [SerializeField] private List<ModifiedGroup> m_Modifications = new();

    public Car CurrentCar => currentCar;

    public List<ModifiedGroup> Modifications => m_Modifications;

    public bool IsSaved
    {
        get { return isSaved; }
        set
        {
            isSaved = value;
            m_DropDownMenu.SetSaveButtonActive(!isSaved);
        }
    }

    public event Action CarSet;

    public static ProjectManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        m_ItemsPanel.SelectedItemChange += OnSelectedItemChange;
        m_CarsLoader.CarLoaded += OnCarHasLoaded;
    }

    private void Start()
    {
        //m_DropDownMenu.OpenProjectBtnClick += OnOpenProjectClick;
        m_DropDownMenu.SubscribeSaveProjectClick(OnSaveProjectClick);
        m_DropDownMenu.SubscribeNewProjectClick(OnNewProjectClick);
    }

    private void OnDestroy()
    {
        m_ItemsPanel.SelectedItemChange -= OnSelectedItemChange;
    }

    private void OnCarHasLoaded(Car car)
    {
        m_AttachMaster = car.GetComponent<PieceAttachMaster>();
    }

    private async void OnSelectedItemChange(PieceItemData data, CategoryItemData category)
    {
        if (category == null)
        {
            currentCar = await m_CarsLoader.LoadCar(data);
            CarSet?.Invoke();
        }
        else if (category.Type == CategoryType.Colors)
        {
            currentCar.ChangeColorTo(data.Data as Data.Model.Color);

            IsSaved = false;
        }
        else
        {
            // ToDo: сделать обработку измененных деталей и ее отмену
            var isModified = Modifications.Any(m => m.Type == category.Type);
            m_AttachMaster.LoadAndAttach(data, category, isModified);
            Modifications.Add(new ModifiedGroup(category.Type, new Modification(projectData.Id, data.Id, null)));

            IsSaved = false;
        }
    }

    public async Task<bool> CreateProject(string projectName, string projectComment = "")
    {
        m_ProjectName = projectName;
        m_Commentary = projectComment;

        var payload = new ProjectDto
        {
            Name = projectName,
            Commentary = projectComment,
            AutomobileId = CurrentCar.CarId,
            ColorId = CurrentCar.ColorId,
            Modifications = new()
        };

#if UNITY_EDITOR
        Debug.Log("New project:\n" + JsonConvert.SerializeObject(payload));
#endif

        var project = await NetworkManager.PostAsync<ProjectDto, Project>("projects", payload, TokenProvider.Instance.GetToken());
        if (project == null)
            return false;

#if UNITY_EDITOR
        Debug.Log("Project created: " + JsonConvert.SerializeObject(project));
#endif

        projectData = project;
        projectData.Automobile = currentCar.Data;

        return true;
    }

    private async void OnSaveProjectClick()
    {
        var payload = new ProjectDto
        {
            Name = m_ProjectName,
            Commentary = m_Commentary,
            AutomobileId = CurrentCar.CarId,
            ColorId = CurrentCar.ColorId,
            Modifications = projectData.Modifications = Modifications.Select(s => s.Modification).ToList()
        };

        var response = await NetworkManager.PutAsync<ProjectDto, Project>($"projects/{projectData.Id}", payload, TokenProvider.Instance.GetToken());
        if (response != null)
            projectData = response;

        IsSaved = true;
        m_DropDownMenu.HidePanel();

#if UNITY_EDITOR
        Debug.Log("Project updated: " + JsonConvert.SerializeObject(response));
#endif
    }

    private void OnNewProjectClick()
    {
        if (IsSaved)
        {
            AppStateManager.Instance.State = AppStateManager.AppState.Start;
            m_DropDownMenu.HidePanel();
        }
        else
            Debug.LogWarning("Сохраните проект, чтобы не потерять наработки!");
    }

    public async Task<bool> LoadProject(int projectId)
    {
        var project = await NetworkManager.GetAsync<Project>($"projects/{projectId}", TokenProvider.Instance.GetToken());
        if (project == null)
            return false;

#if UNITY_EDITOR
        Debug.Log("Project loaded:\n" + JsonConvert.SerializeObject(project));
#endif

        return true;
    }
}