using System;
using UnityEngine;

[Serializable]
public class ItemsData
{
    [SerializeField] private int m_Id = 1;
    [SerializeField] private CathegoryType m_Type;
    [SerializeField] private string m_Manufacturer = "Manufacturer";
    [SerializeField] private string m_ModelName = "Model name";
    [SerializeField] private string m_ImageUrl = "https://";
    [SerializeField] private Texture2D m_Image = default;

    public int Id => m_Id;
    public CathegoryType Type => m_Type;
    public string Caption => m_Manufacturer;
    public string ImageUrl => m_ImageUrl;
    public Texture2D Image
    {
        get { return m_Image; }
        set { m_Image = value; }
    }
}