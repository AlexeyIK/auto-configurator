using UnityEngine;

public interface ICathegoryItemData
{
    int Id { get; }
    string Caption { get; }
    string ImageUrl { get; }
    Sprite Image { get; set; }
}