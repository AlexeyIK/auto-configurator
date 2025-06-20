using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CategoriesMockList", menuName = "Data Mock/Create Categories List", order = 1)]
public class CategoriesMock : ScriptableObject
{
    [SerializeField] private List<CategoryItemData> m_CathegoryItems;

    public List<CategoryItemData> CathegoryItems => m_CathegoryItems;
}