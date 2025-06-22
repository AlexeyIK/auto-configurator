using System;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CategoryItemData
{
    [SerializeField] private int m_Id = 1;
    [SerializeField] private CategoryType m_Type;
    [SerializeField] private string m_Caption = "Cathegory Name";
    [SerializeField] private string m_ImageUrl = "https://";
    [SerializeField] private Texture2D m_Image = default;

    public CategoryItemData(int id, CategoryType type, string caption, string imageUrl)
    {
        m_Id = id;
        m_Type = type;
        m_Caption = caption;
        m_ImageUrl = imageUrl;

        if (imageUrl != null)
            TryLoadImage(imageUrl);
    }

    public async void TryLoadImage(string imageUrl)
    {
        if (imageUrl.EndsWith(".png") || imageUrl.EndsWith(".jpg"))
            imageUrl = imageUrl[..imageUrl.LastIndexOf('.')];

        var request = Resources.LoadAsync(imageUrl);
        while (!request.isDone)
            await Task.Yield();

        Texture2D texture = request.asset as Texture2D;
        if (texture == null)
            Debug.LogWarning($"[LoadImageAsync] Не удалось загрузить {imageUrl}");

        m_Image = texture;
    }

    public int Id => m_Id;
    public CategoryType Type => m_Type;
    public string Caption => m_Caption;
    public string ImageUrl => m_ImageUrl;
    public Texture2D Image => m_Image;
}
