using System;
using UnityEngine;

[Serializable]
public class CategoryItemData : ICategoryItemData
{
    [SerializeField] private int m_Id = 1;
    [SerializeField] private CategoryType m_Type;
    [SerializeField] private string m_Caption = "Cathegory Name";
    [SerializeField] private string m_ImageUrl = "https://";
    [SerializeField] private Texture2D m_Image = default;

    public int Id => m_Id;
    public CategoryType Type => m_Type;
    public string Caption => m_Caption;
    public string ImageUrl => m_ImageUrl;
    public Texture2D Image
    {
        get { return m_Image; }
        set { m_Image = value; }
    }
}
