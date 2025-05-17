using UnityEngine;

public interface ICathegoryItemData
{
    int Id { get; }
    string Caption { get; }
    string ImageUrl { get; }
    Texture2D Image { get; set; }
}