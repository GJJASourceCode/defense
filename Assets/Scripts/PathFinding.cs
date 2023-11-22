using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    public Tilemap ground;

    public void FindPath(int startX, int startY, int endX, int endY)
    {
        BoundsInt bounds = ground.cellBounds;
        // TileBase[] allTiles = ground.GetTilesBlock(bounds);

        // Debug.Log(allTiles.Count(s => s != null));

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = ground.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                }
                // else
                // {
                //     Debug.Log("x:" + x + " y:" + y + " tile: (null)");
                // }
            }
        }
    }

    void Start()
    {
        FindPath(0, 0, 1, 1);
    }
}
