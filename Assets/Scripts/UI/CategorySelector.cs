using UnityEngine;

public enum CategoryType
{
    Automobiles = 99,
    Body = 0,
    Wheels = 1,
    Tyres = 2,
    Spoilers = 3,
    Bumbers = 4,
    Exhaust = 5
}

public class CategorySelector : MonoBehaviour
{
    private CategoriesPanelController cathegoriesController;

    [SerializeField] private InteractionHandler[] interactionHandlers = default;
    [SerializeField] private CategoryType category = CategoryType.Body;

    private void Awake()
    {
        if (interactionHandlers.Length == 0)
            Debug.LogError($"Не назначен ни один InteractionHandler для категории \"{category}\" на объекте {gameObject.name}!");

        cathegoriesController = GameObject.FindFirstObjectByType<CategoriesPanelController>();

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

    public void SetActivity(bool isActive)
    {
        foreach (var interaction in interactionHandlers)
            interaction.IsActive = isActive;
    }

    private void OnPartClick(GameObject go)
    {
        cathegoriesController.SelectByType(category);
    }
}
