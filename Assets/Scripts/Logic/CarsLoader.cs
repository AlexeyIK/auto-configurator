using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using UnityEngine;
using UnityEngine.UI;

public class CarsLoader : MonoBehaviour
{
    [SerializeField] private List<Car> m_AvailableCars = default;
    [SerializeField] private StandRotator m_SpawnStand = default;
    [SerializeField] private Button m_ChangeCarBtn = default;
    [SerializeField] private Car m_DefaultCar = default;
    [SerializeField] private ItemsPanelController m_ItemsPanel = default;

    private void Awake()
    {
        if (m_DefaultCar != null)
            SpawnACar(m_DefaultCar);

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
        // очищаем от расширения
        if (carPath.EndsWith(".asset"))
            carPath = carPath[..carPath.LastIndexOf('.')];

        var request = Resources.LoadAsync<Car>(carPath);
        while (!request.isDone)
            await Task.Yield();

        var spawnCar = request.asset as Car;
        if (spawnCar == null)
        {
            Debug.LogError($"Не могу загрузить автомобиль по ссылке: {carPath}");
            return;
        }

        m_SpawnStand.SetACar(spawnCar);
    }

    public void SpawnACar(Car prefab)
    {
        m_SpawnStand.SetACar(prefab);
    }

    private async void OnSelectedItemChange(PieceItemData data, CategoryType type)
    {
        if (type != CategoryType.Automobiles)
            return;

        var automobileData = await NetworkManager.GetAsync<Automobile>($"Automobiles/{data.Id}", TokenProvider.Instance.GetToken());

        SpawnACar(automobileData.ModelUrl);
    }
}
