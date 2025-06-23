using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Data.ViewModel
{
    [Serializable]
    public class PieceItemData
    {
        private object data = null;

        [SerializeField] private int m_Id = 1;
        [SerializeField] private CategoryType m_Type;
        [SerializeField] private string m_Manufacturer = "Manufacturer";
        [SerializeField] private string m_ModelName = "Model name";
        [SerializeField] private string m_ImageUrl = "https://";
        [SerializeField] private Texture2D m_Image = default;

        public PieceItemData(int id, CategoryType type, string manufacturer, string modelName, string imageUrl, object data)
        {
            m_Id = id;
            m_Type = type;
            m_Manufacturer = manufacturer;
            m_ModelName = modelName;
            this.data = data;

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
        public string Caption => m_Manufacturer;
        public string Subcaption => m_ModelName;
        public string ImageUrl => m_ImageUrl;
        public Texture2D Image => m_Image;

        public object Data => data;
    }
}