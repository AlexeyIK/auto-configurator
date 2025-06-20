using UnityEngine;

public interface ICategoryItemData
{
    int Id { get; }
    string Caption { get; }
    string ImageUrl { get; }
    Texture2D Image { get; set; }
}