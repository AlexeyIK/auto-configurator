using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using UnityEngine;
using UnityEngine.UI;

public class CarsLoader : MonoBehaviour
{
    [SerializeField] private List<Car> m_AvailableCars = default;
    [SerializeField] private GameObject m_SpawnStage = default;
    [SerializeField] private Button m_ChangeCarBtn = default;
    [SerializeField] private Car m_DefaultCar = default;
    [SerializeField] private ItemsPanelController m_ItemsPanel = default;

    private void Awake()
    {
        ClearSpawnPoint();
        m_ItemsPanel.SelectedItemChange += OnSelectedItemChange;
    }

    private void OnDestroy()
    {
        m_ItemsPanel.SelectedItemChange -= OnSelectedItemChange;
    }

    /// <summary>
    /// Создание автомобиля по ссылке
    /// </summary>
    /// <param name="carPath"></param>
    public async void SpawnACar(string carPath)
    {
        ClearSpawnPoint();

        // очищаем от расширения
        if (carPath.EndsWith(".asset"))
            carPath = carPath[..carPath.LastIndexOf('.')];

        var request = Resources.LoadAsync<GameObject>(carPath);
        while (!request.isDone)
            await Task.Yield();

        var spawnCar = request.asset as GameObject;
        if (spawnCar == null)
            Debug.LogError($"Не могу загрузить автомобиль по ссылке: {carPath}");
        else
            GameObject.Instantiate(spawnCar, m_SpawnStage.transform);
    }

    private async void OnSelectedItemChange(PieceItemData data, CategoryType type)
    {
        if (type != CategoryType.Automobiles)
            return;

        var automobileData = await NetworkManager.GetAsync<Automobile>($"Automobiles/{data.Id}", TokenProvider.Instance.GetToken());

        SpawnACar(automobileData.ModelUrl);
    }

    private void ClearSpawnPoint()
    {
        // сначала удаляем предыдущую модель авто с пьедестала
        foreach (Transform child in m_SpawnStage.transform)
        {
            child.gameObject.SetActive(false);
            GameObject.Destroy(child.gameObject);
        }
    }
}
