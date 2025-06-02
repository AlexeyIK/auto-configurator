using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsMockList", menuName = "Data Mock/Create Items List", order = 2)]
public class ItemsMock : ScriptableObject
{
    [Serializable]
    public struct CathegoryItems
    {
        public CathegoryType CathegoryType;
        public List<ItemsData> Items;
    }

    [SerializeField] private List<CathegoryItems> m_Items;

    public List<CathegoryItems> Items => m_Items;
}

