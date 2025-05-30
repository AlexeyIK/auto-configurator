using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "CathegoriesMockList", menuName = "Data Mock/Create Cathegories List", order = 1)]
public class CathegoriesMock : ScriptableObject
{
    [SerializeField] private List<CathegoryItemData> m_CathegoryItems;

    public List<CathegoryItemData> CathegoryItems => m_CathegoryItems;
}

