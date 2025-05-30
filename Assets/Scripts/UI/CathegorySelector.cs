using System;
using UnityEngine;

public enum CathegoryType
{
    Body = 0,
    Wheels = 1,
    Tyres = 2,
    Spoilers = 3,
    Bumbers = 4,
    Exhaust = 5
}

public class CathegorySelector : MonoBehaviour
{
    private CathegoriesController cathegoriesController;

    [SerializeField] private InteractionHandler[] interactionHandlers = default;
    [SerializeField] private CathegoryType cathegory = CathegoryType.Body;

    private void Awake()
    {
        if (interactionHandlers.Length == 0)
            Debug.LogError($"Не назначен ни один InteractionHandler для категории \"{cathegory}\" на объекте {gameObject.name}!");

        cathegoriesController = GameObject.FindFirstObjectByType<CathegoriesController>();

        if (cathegoriesController == null)
            Debug.Log("Не могу найти скрипт CathegoriesController для управления окном категорий!");

        foreach (var interaction in interactionHandlers)
            interaction.OnClick.AddListener(OnPartClick);
    }

    private void OnDestroy()
    {
        foreach (var interaction in interactionHandlers)
            interaction.OnClick.RemoveListener(OnPartClick);
    }

    private void OnPartClick(GameObject go)
    {
        cathegoriesController.SelectByType(cathegory);
    }
}
