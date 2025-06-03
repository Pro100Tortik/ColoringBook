using UnityEngine;

[CreateAssetMenu(fileName = "Color Palette", menuName = "Brushes/Create color palette")]
public class ColorPaletteSO : ScriptableObject
{
    public Color EraserColor = Color.white;
    public Color[] Colors = new Color[14];
}
