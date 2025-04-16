using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CarsLoader : MonoBehaviour
{
    private int m_CurrentCarId;

    [SerializeField] private Car[] m_AvailableCars = default;
    [SerializeField] private GameObject m_SpawnStage = default;
    [SerializeField] private Button m_ChangeCarBtn = default;

    private void Awake()
    {
        SpawnACar(0);

        m_ChangeCarBtn.onClick.AddListener(SpawnNextCar);
    }

    private void OnDestroy()
    {
        m_ChangeCarBtn.onClick.RemoveListener(SpawnNextCar);
    }

    public void SpawnACar(string carId)
    {
        ClearSpawnPoint();

        // затем ищем авто в базе и создаем модель
        var spawnCar = m_AvailableCars.FirstOrDefault(c => carId == c.CarID);
        if (spawnCar == null)
            Debug.LogError($"Не могу найти автомобиль с ID={carId}");
        else
            GameObject.Instantiate(spawnCar, m_SpawnStage.transform);

    }

    public void SpawnACar(int carId)
    {
        ClearSpawnPoint();

        if (carId > m_AvailableCars.Length - 1)
            Debug.LogError($"Не могу найти автомобиль с ID={carId}");
        else
            GameObject.Instantiate(m_AvailableCars[carId], m_SpawnStage.transform);
    }

    private void SpawnNextCar()
    {
        if (m_AvailableCars.Length == 0)
            return;

        m_CurrentCarId++;

        if (m_CurrentCarId > m_AvailableCars.Length - 1)
            m_CurrentCarId = 0;

        SpawnACar(m_CurrentCarId);
    }

    private void ClearSpawnPoint()
    {
        // сначала удаляем предыдущую модель авто с пьедестала
        foreach (Transform child in m_SpawnStage.transform)
            GameObject.Destroy(child.gameObject);
    }
}
