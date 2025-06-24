using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model;
using Data.ViewModel;
using UnityEngine;

public class CarsLoader : MonoBehaviour
{
    [SerializeField] private List<Car> m_AvailableCars = default;
    [SerializeField] private Car m_DefaultCar = default;
    [Header("Spawn options")]
    [SerializeField] private StandRotator m_RotatingStand = default;
    [SerializeField] private Transform m_SpawnPoint = default;

    public event Action<Car> CarLoaded;

    private void Awake()
    {
        SpawnACar(m_DefaultCar);
    }

    private void Start()
    {
        AppStateManager.Instance.SubscribeStateChange(OnAppStateChange);
    }

    private void OnDestroy()
    {
        AppStateManager.Instance.UnsubscriveStateChange(OnAppStateChange);
    }

    private void OnAppStateChange(AppStateManager.AppState state)
    {
        if (state == AppStateManager.AppState.Start)
            SpawnACar(m_DefaultCar);
    }

    /// <summary>
    /// Загрузка автомобиля из данных
    /// </summary>
    /// <param name="carData"></param>
    /// <returns></returns>
    public async Task<Car> LoadCar(PieceItemData carData)
    {
        var automobileData = await NetworkManager.GetAsync<Automobile>($"Automobiles/{carData.Id}", TokenProvider.Instance.GetToken());
        if (automobileData == null)
        {
            Debug.LogError($"Couldn't load requested Car with id={carData.Id} from server");
            return null;
        }

        return await SpawnACar(automobileData);
    }

    /// <summary>
    /// Создание автомобиля по ссылке
    /// </summary>
    /// <param name="carPath"></param>
    private async Task<Car> SpawnACar(Automobile automobileData)
    {
        var modelPath = automobileData.ModelUrl;

        // очищаем от расширения
        if (modelPath.EndsWith(".asset"))
            modelPath = modelPath[..modelPath.LastIndexOf('.')];

        var request = Resources.LoadAsync<Car>(modelPath);
        while (!request.isDone)
            await Task.Yield();

        var spawnCarPrefab = request.asset as Car;
        if (spawnCarPrefab == null)
        {
            Debug.LogError($"Couldn't load the Car model from resources: {modelPath}");
            return null;
        }

        return InstantiateCar(spawnCarPrefab, automobileData);
    }

    /// <summary>
    /// Создание автомобиля из префаба
    /// </summary>
    /// <param name="prefab"></param>
    private void SpawnACar(Car prefab)
    {
        if (m_DefaultCar == null)
            ClearSpawnPoint();
        else
            InstantiateCar(prefab, null);
    }

    private Car InstantiateCar(Car prefab, Automobile data)
    {
        ClearSpawnPoint();
        m_RotatingStand.IsRotating = true;
        var car = GameObject.Instantiate(prefab, m_SpawnPoint);
        car.SetDataContext(data);

        CarLoaded?.Invoke(car);
        return car;
    }

    private void ClearSpawnPoint()
    {
        m_RotatingStand.IsRotating = false;

        // сначала удаляем предыдущую модель авто с пьедестала
        foreach (Transform child in m_SpawnPoint)
        {
            child.gameObject.SetActive(false);
            GameObject.Destroy(child.gameObject);
        }
    }
}
