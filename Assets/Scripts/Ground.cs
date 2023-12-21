using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Node
{
    public Node(bool _isBuilt, int _x, int _y)
    {
        isBuilt = _isBuilt;
        x = _x;
        y = _y;
    }

    public bool isBuilt;
    public Node ParentNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x,
        y,
        G,
        H;
    public int F
    {
        get { return G + H; }
    }
}

public class Ground : MonoBehaviour
{
    public List<List<Node>> GetGraph()
    {
        // 그래프 생성
        var ground = GetComponent<Tilemap>();
        BoundsInt bounds = ground.cellBounds;

        var graph = new List<List<Node>>();
        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            List<Node> temp = new List<Node>();
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                TileBase tile = ground.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    if (FindObjectOfType<SpawnManager>().isBuilt[x - bounds.xMin][y - bounds.yMin])
                    {
                        temp.Add(new Node(true, x - bounds.xMin, y - bounds.yMin));
                    }
                    else
                    {
                        temp.Add(new Node(false, x - bounds.xMin, y - bounds.yMin));
                    }
                }
                else
                {
                    temp.Add(new Node(true, x - bounds.xMin, y - bounds.yMin));
                }
            }
            graph.Add(temp);
        }

        return graph;
    }
}
