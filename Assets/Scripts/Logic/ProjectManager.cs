using System;
using System.Collections.Generic;
using System.Linq;
using Data.Model;
using Data.ViewModel;
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

    private Project projectData;

    [SerializeField] private ItemsPanelController m_ItemsPanel = default;
    [SerializeField] private CarsLoader m_CarsLoader = default;
    [SerializeField] private PieceAttachMaster m_AttachMaster = default;
    [Header("Project data")]
    [SerializeField] private string Name = "Project name";
    [SerializeField] private Car m_CurrentCar = default;
    [SerializeField] private List<ModifiedGroup> m_Modifications = new();

    public Car CurrentCar => m_CurrentCar;

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
            m_CurrentCar = await m_CarsLoader.LoadCar(data);
            CarSet?.Invoke();
        }
        else if (category.Type == CategoryType.Colors)
        {
            if (ColorUtility.TryParseHtmlString($"#{data.Subcaption}", out var color))
                m_CurrentCar.ChangeColorTo(color);
        }
        else
        {
            // ToDo: сделать обработку измененных деталей и ее отмену
            var isModified = Modifications.Any(m => m.Type == category.Type);
            m_AttachMaster.LoadAndAttach(data, category, isModified);
            Modifications.Add(new ModifiedGroup(CategoryType.Wheels, new Modification(projectData.Id, data.Id, null)));
        }
    }

    internal void SetProject(Project project)
    {
        projectData = project;
    }
}