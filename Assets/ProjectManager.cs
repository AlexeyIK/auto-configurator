using System;
using System.Collections.Generic;
using Data.Model;
using Data.ViewModel;
using UnityEngine;

public class ProjectManager : MonoBehaviour
{
    [SerializeField] private ItemsPanelController m_ItemsPanel = default;
    [SerializeField] private CarsLoader m_CarsLoader = default;
    [Header("Project data")]
    [SerializeField] private string Name = "Project name";
    [SerializeField] private Car m_CurrentCar = default;
    [SerializeField] private List<Modification> m_Modifications = new();

    public Car CurrentCar => m_CurrentCar;

    public List<Modification> Modifications => m_Modifications;

    public event Action CarSet;

    public static ProjectManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        m_ItemsPanel.SelectedItemChange += OnSelectedItemChange;
    }

    private void OnDestroy()
    {
        m_ItemsPanel.SelectedItemChange -= OnSelectedItemChange;
    }

    private async void OnSelectedItemChange(PieceItemData data, CategoryType type)
    {
        if (type == CategoryType.Automobiles)
        {
            m_CurrentCar = await m_CarsLoader.LoadCar(data);
            CarSet?.Invoke();
        }
        else if (type == CategoryType.Colors)
        {
            if (ColorUtility.TryParseHtmlString($"#{data.Subcaption}", out var color))
                m_CurrentCar.ChangeColorTo(color);
        }
        else
        {
            // ToDo: сделать обработку измененных деталей и ее отмену
        }
    }
}
