using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y)
    {
        isWall = _isWall;
        x = _x;
        y = _y;
    }

    public bool isWall;
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

public class PathFinding : MonoBehaviour
{
    public Tilemap ground;
    public TowerSpawner towerSpawner;

    private List<List<Node>> graph;

    public List<Node> FindPath(int startX, int startY, int targetX, int targetY)
    {
        // 그래프 생성
        BoundsInt bounds = ground.cellBounds;

        graph = new List<List<Node>>();
        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            List<Node> temp = new List<Node>();
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                TileBase tile = ground.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    if (towerSpawner.isBuilt[x - bounds.xMin][y - bounds.yMin])
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

        var startNode = graph[startX - bounds.xMin][startY - bounds.yMin];
        var targetNode = graph[targetX - bounds.xMin][targetY - bounds.yMin];

        var openList = new List<Node>() { startNode };
        var closedList = new List<Node>();
        var finalNodeList = new List<Node>();

        while (openList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            var currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
                if (openList[i].F <= currentNode.F && openList[i].H < currentNode.H)
                    currentNode = openList[i];

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                Node targetCurrentNode = targetNode;
                while (targetCurrentNode != startNode)
                {
                    finalNodeList.Add(targetCurrentNode);
                    targetCurrentNode = targetCurrentNode.ParentNode;
                }
                finalNodeList.Add(startNode);
                finalNodeList.Reverse();

                for (int i = 0; i < finalNodeList.Count; i++)
                {
                    finalNodeList[i].x = finalNodeList[i].x + bounds.xMin;
                    finalNodeList[i].y = finalNodeList[i].y + bounds.yMin;
                    // towerSpawner.SpawnTower(
                    //     new Vector3Int(finalNodeList[i].x, finalNodeList[i].y, 0)
                    // );

                    // print(i + "번째는 " + finalNodeList[i].x + ", " + finalNodeList[i].y);
                }

                return finalNodeList;
            }

            OpenListAdd(
                ref currentNode,
                ref targetNode,
                currentNode.x + 1,
                currentNode.y,
                bounds.size.x,
                bounds.size.y,
                ref openList,
                ref closedList
            );
            OpenListAdd(
                ref currentNode,
                ref targetNode,
                currentNode.x - 1,
                currentNode.y,
                bounds.size.x,
                bounds.size.y,
                ref openList,
                ref closedList
            );
            OpenListAdd(
                ref currentNode,
                ref targetNode,
                currentNode.x,
                currentNode.y + 1,
                bounds.size.x,
                bounds.size.y,
                ref openList,
                ref closedList
            );
            OpenListAdd(
                ref currentNode,
                ref targetNode,
                currentNode.x,
                currentNode.y - 1,
                bounds.size.x,
                bounds.size.y,
                ref openList,
                ref closedList
            );
        }

        return null;
    }

    void OpenListAdd(
        ref Node currentNode,
        ref Node targetNode,
        int checkX,
        int checkY,
        int maxX,
        int maxY,
        ref List<Node> openList,
        ref List<Node> closedList
    )
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (
            checkX >= 0
            && checkX <= maxX
            && checkY >= 0
            && checkY <= maxY
            && !graph[checkX][checkY].isWall
            && !closedList.Contains(graph[checkX][checkY])
        )
        {
            Node neighborNode = graph[checkX][checkY];
            int MoveCost = currentNode.G + 1;

            // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
            if (MoveCost < neighborNode.G || !openList.Contains(neighborNode))
            {
                neighborNode.G = MoveCost;
                neighborNode.H =
                    (
                        Mathf.Abs(neighborNode.x - targetNode.x)
                        + Mathf.Abs(neighborNode.y - targetNode.y)
                    ) * 10;
                neighborNode.ParentNode = currentNode;

                openList.Add(neighborNode);
            }
        }
    }

    // void Start()
    // {
    // FindPath(-12, 0, -2, 7);
    // }
}
