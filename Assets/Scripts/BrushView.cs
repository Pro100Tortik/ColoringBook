using UnityEngine.Tilemaps;
using UnityEngine;

public class BrushView : MonoBehaviour
{
    public Tilemap paintTilemap;

    public void PlaceTile(Vector3Int cellPosition, Tile tile)
    {
        paintTilemap.SetTile(cellPosition, tile);
        paintTilemap.SetTileFlags(cellPosition, TileFlags.None);
    }

    public void PaintCell(Vector3Int cellposition, Tile brush)
    {
        var color = paintTilemap.GetColor(cellposition);

        // Do not paint with same color
        if (brush.color == color)
            return;

        paintTilemap.SetColor(cellposition, brush.color);
    }
}
