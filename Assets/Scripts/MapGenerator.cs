using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [Header("Node Settings")]
    public GameObject nodePrefab;
    public float horizontalSpacing = 3f;
    public float verticalSpacing = 2.5f;
    public int totalColumns = 6;

    [Header("Random Column Settings")]
    public int minNodesPerColumn = 2;
    public int maxNodesPerColumn = 4;

    [Header("Map Start Position")]
    public Vector2 startPosition = new Vector2(-10f, 0f);

    [Header("Line Settings")]
    public Material lineMaterial;

    private List<List<Transform>> nodeGrid = new List<List<Transform>>();

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        nodeGrid.Clear();

        // Nodo inicial (columna 0)
        List<Transform> startColumn = new List<Transform>();
        var startNode = Instantiate(nodePrefab, startPosition, Quaternion.identity, this.transform);
        startColumn.Add(startNode.transform);
        nodeGrid.Add(startColumn);

        // Columnas intermedias (1 to totalColumns - 1)
        for (int column = 1; column < totalColumns; column++)
        {
            List<Transform> columnNodes = new List<Transform>();

            int nodeCount = Random.Range(minNodesPerColumn, maxNodesPerColumn + 1);
            float totalHeight = (nodeCount - 1) * verticalSpacing;
            float startY = -totalHeight / 2f;

            for (int i = 0; i < nodeCount; i++)
            {
                Vector3 position = new Vector3(
                    startPosition.x + column * horizontalSpacing,
                    startPosition.y + startY + i * verticalSpacing,
                    0
                );

                var node = Instantiate(nodePrefab, position, Quaternion.identity, this.transform);
                columnNodes.Add(node.transform);
            }

            nodeGrid.Add(columnNodes);
        }

        // Nodo final (boss)
        Vector3 finalPosition = new Vector3(
            startPosition.x + totalColumns * horizontalSpacing,
            startPosition.y,
            0
        );
        List<Transform> finalColumn = new List<Transform>();
        var bossNode = Instantiate(nodePrefab, finalPosition, Quaternion.identity, this.transform);
        finalColumn.Add(bossNode.transform);
        nodeGrid.Add(finalColumn);

        ConnectNodes();
    }

    private void ConnectNodes()
    {
        for (int i = 0; i < nodeGrid.Count - 1; i++)
        {
            List<Transform> currentColumn = nodeGrid[i];
            List<Transform> nextColumn = nodeGrid[i + 1];

            // Seguimiento de conexiones
            Dictionary<Transform, bool> hasOutgoing = new Dictionary<Transform, bool>();
            Dictionary<Transform, bool> hasIncoming = new Dictionary<Transform, bool>();

            foreach (Transform node in currentColumn)
                hasOutgoing[node] = false;
            foreach (Transform node in nextColumn)
                hasIncoming[node] = false;

            foreach (Transform fromNode in currentColumn)
            {
                if (i == 0)
                {
                    // Primer nodo conecta con todos
                    foreach (Transform toNode in nextColumn)
                    {
                        DrawLine(fromNode.position, toNode.position);
                        hasOutgoing[fromNode] = true;
                        hasIncoming[toNode] = true;
                    }
                }
                else
                {
                    // Conectar al nodo más cercano que aún no tenga conexión entrante
                    Transform closest = null;
                    float minDist = float.MaxValue;

                    foreach (Transform toNode in nextColumn)
                    {
                        float dist = Vector3.Distance(fromNode.position, toNode.position);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            closest = toNode;
                        }
                    }

                    if (closest != null)
                    {
                        DrawLine(fromNode.position, closest.position);
                        hasOutgoing[fromNode] = true;
                        hasIncoming[closest] = true;
                    }
                }
            }

            // Asegurar que todos los nodos de la siguiente columna tengan entrada
            foreach (Transform toNode in nextColumn)
            {
                if (!hasIncoming[toNode])
                {
                    Transform closest = null;
                    float minDist = float.MaxValue;

                    foreach (Transform fromNode in currentColumn)
                    {
                        float dist = Vector3.Distance(fromNode.position, toNode.position);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            closest = fromNode;
                        }
                    }

                    if (closest != null)
                    {
                        DrawLine(closest.position, toNode.position);
                        hasOutgoing[closest] = true;
                        hasIncoming[toNode] = true;
                    }
                }
            }

            // Asegurar que todos los nodos actuales tengan salida
            foreach (Transform fromNode in currentColumn)
            {
                if (!hasOutgoing[fromNode])
                {
                    Transform closest = null;
                    float minDist = float.MaxValue;

                    foreach (Transform toNode in nextColumn)
                    {
                        float dist = Vector3.Distance(fromNode.position, toNode.position);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            closest = toNode;
                        }
                    }

                    if (closest != null)
                    {
                        DrawLine(fromNode.position, closest.position);
                        hasOutgoing[fromNode] = true;
                        hasIncoming[closest] = true;
                    }
                }
            }
        }
    }

    private void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("ConnectionLine");
        lineObj.transform.parent = this.transform;

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = lineMaterial;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.sortingOrder = -1; // behind nodes
    }
}
