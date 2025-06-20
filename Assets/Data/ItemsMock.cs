using System;
using System.Collections.Generic;
using Data.ViewModel;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsMockList", menuName = "Data Mock/Create Items List", order = 2)]
public class ItemsMock : ScriptableObject
{
    [Serializable]
    public struct CategoryItems
    {
        public CategoryType CathegoryType;
        public List<PieceItemData> Items;
    }

    [SerializeField] private List<CategoryItems> m_Items;

    public List<CategoryItems> Items => m_Items;
}

