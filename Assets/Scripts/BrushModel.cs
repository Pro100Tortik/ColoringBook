using UnityEngine.Tilemaps;
using UnityEngine;

public class BrushModel
{
    public Tile CurrentBrush { get; private set; }

    public BrushModel(Sprite sprite, Color standartColor)
    {
        CurrentBrush = ScriptableObject.CreateInstance<Tile>();

        CurrentBrush.sprite = sprite;
        CurrentBrush.color = standartColor;
    }

    public void SetBrushColor(Color color)
    {
        CurrentBrush.color = color;
    }
}
