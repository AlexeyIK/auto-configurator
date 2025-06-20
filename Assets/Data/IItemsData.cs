using UnityEngine;

public interface IItemsData
{
    public int Id { get; }
    public CategoryType Type { get; }
    public string Caption { get; }
    public string Subcaption { get; }
    public Texture2D Image { get; }
}