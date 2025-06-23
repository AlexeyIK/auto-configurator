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

    [SerializeField] private ItemsPanelController m_ItemsPanel = default;
    [SerializeField] private CarsLoader m_CarsLoader = default;
    [SerializeField] private PieceAttachMaster m_AttachMaster = default;
    [Header("Project data")]
    [SerializeField] private string Name = "Project name";
    [SerializeField] private List<ModifiedGroup> m_Modifications = new();

    public Car CurrentCar => currentCar;

    public List<ModifiedGroup> Modifications => m_Modifications;

    public event Action CarSet;

    public static ProjectManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        m_ItemsPanel.SelectedItemChange += OnSelectedItemChange;
        m_CarsLoader.CarLoaded += OnCarHasLoaded;
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
        }
        else
        {
            // ToDo: сделать обработку измененных деталей и ее отмену
            var isModified = Modifications.Any(m => m.Type == category.Type);
            m_AttachMaster.LoadAndAttach(data, category, isModified);
            Modifications.Add(new ModifiedGroup(CategoryType.Wheels, new Modification(projectData.Id, data.Id, null)));
        }
    }

    public async Task<bool> CreateProject(string projectName, string projectComment = "")
    {
        var payload = new ProjectDto
        {
            Name = projectName,
            Commentary = projectComment,
            AutomobileId = CurrentCar.CarId,
            ColorId = CurrentCar.ColorId,
            Modifications = new()
        };

        Debug.Log("New project:\n" + JsonConvert.SerializeObject(payload));

        var project = await NetworkManager.PostAsync<ProjectDto, Project>("projects", payload, TokenProvider.Instance.GetToken());
        if (project == null)
            return false;

        Debug.Log("Project created:\n" + JsonConvert.SerializeObject(project));

        projectData = project;
        projectData.Automobile = currentCar.Data;

        return true;
    }

    public void LoadProject(int projectId)
    {

    }
}