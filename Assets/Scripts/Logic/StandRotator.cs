using System;
using UnityEngine;

public class StandRotator : MonoBehaviour
{
    private DragHandler dragHandler;
    private bool isDragging;
    private float rotateCountdown;

    private Car currentCar = null;

    [SerializeField] private Transform m_SpawnPoint = default;
    [SerializeField] private float m_RotateSpeed = -2f;
    [SerializeField] private float m_DragRotateSensivity = 0.5f;
    [SerializeField] private float m_RotatePause = 1f;

    private void Awake()
    {
        dragHandler = GetComponent<DragHandler>();
        dragHandler.DragStart += () => isDragging = true;
        dragHandler.DragEnd += () =>
        {
            isDragging = false;
            rotateCountdown = m_RotatePause;
        };
        dragHandler.DragPerforming += (position, delta) =>
        {
            transform.Rotate(Vector3.up, -delta.x * m_DragRotateSensivity);
            m_SpawnPoint.Rotate(Vector3.up, -delta.x * m_DragRotateSensivity);
        };

        ClearSpawnPoint();
    }

    private void Update()
    {
        if (!isDragging && currentCar != null)
        {
            if (rotateCountdown > 0)
                rotateCountdown -= Time.deltaTime;
            else
            {
                transform.Rotate(new Vector3(0, 1, 0), m_RotateSpeed * Time.deltaTime);
                m_SpawnPoint.Rotate(new Vector3(0, 1, 0), m_RotateSpeed * Time.deltaTime);
            }
        }
    }

    private void ClearSpawnPoint()
    {
        // сначала удаляем предыдущую модель авто с пьедестала
        foreach (Transform child in m_SpawnPoint)
        {
            child.gameObject.SetActive(false);
            GameObject.Destroy(child.gameObject);
        }
    }

    public void SetACar(Car spawnCar)
    {
        if (spawnCar != null && currentCar != spawnCar)
        {
            ClearSpawnPoint();
            GameObject.Instantiate(spawnCar, m_SpawnPoint.transform);
        }

        currentCar = spawnCar;
    }
}
