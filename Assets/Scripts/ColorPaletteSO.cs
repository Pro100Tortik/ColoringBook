using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color Palette", menuName = "Brushes/Create color palette")]
public class ColorPaletteSO : ScriptableObject
{
    [field: SerializeField] public Color EraserColor { get; private set; } = Color.white;
    public IReadOnlyList<Color> Colors => colors;
    [SerializeField] private Color[] colors = new Color[14];
}
