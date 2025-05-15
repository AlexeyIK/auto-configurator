using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CathegoryItemData", menuName = "Data Mock/Create Cathegory Item", order = 1)]
public class CathegoryItemDataMock : ScriptableObject, ICathegoryItemData
{
    private CathegoryItemDataMock[] m_CathegoryItems;

    [SerializeField] private int m_Id = 1;
    [SerializeField] private string m_Caption = "Cathegory Name";
    [SerializeField] private string m_ImageUrl = "https://";
    [SerializeField] private Sprite m_Image = default;

    public int Id => m_Id;
    public string Caption => m_Caption;
    public string ImageUrl => m_ImageUrl;
    public Sprite Image
    {
        get { return m_Image; }
        set { m_Image = value; }
    }

    private void OnEnable()
    {
        m_CathegoryItems = Resources.FindObjectsOfTypeAll<CathegoryItemDataMock>();
        if (m_CathegoryItems.FirstOrDefault(c => c.Id == m_Id) != null)
            m_Id++;
    }
}
