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
    public DungeonManager dungeonManager;

    public RoomType beginType;
    public RoomType bossType;
    public RoomType figthType;
    public List<RoomType> typeOptions;

    public List<RoomType> allRoomTypes;

    void Start()
    {
        MapData mapData = MapSaveSystem.LoadMap();
        if (PlayerPrefs.GetInt("MapGenerated") == 0)
        {
            GenerateMap();
        }
        else if(mapData != null)
        {
            LoadMapFromData(mapData);
        }
    }
    void GenerateMap()
    {
        nodeGrid.Clear();

        // Nodo inicial (columna 0)
        List<Transform> startColumn = new List<Transform>();
        var startNode = Instantiate(nodePrefab, startPosition, Quaternion.identity, this.transform);
        NodeRoom firstNode = startNode.GetComponent<NodeRoom>();
        firstNode.roomType = beginType;
        dungeonManager.currentNode = firstNode;
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
                NodeRoom nodeScript = node.GetComponent<NodeRoom>();
                int randomNumber = Random.Range(1, 100);
                if (randomNumber < 50)
                {
                    nodeScript.roomType = figthType;
                }
                else
                {
                    nodeScript.roomType = typeOptions[Random.Range(0, typeOptions.Count)];
                }
                nodeScript.ShowSpriteRoom();
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
        NodeRoom scriptBossRoom = bossNode.GetComponent<NodeRoom>();
        scriptBossRoom.roomType = bossType;
        scriptBossRoom.ShowSpriteRoom();
        finalColumn.Add(bossNode.transform);
        nodeGrid.Add(finalColumn);

        ConnectNodes();
        PlayerPrefs.SetInt("MapGenerated", 1);
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
                NodeRoom scriptNode = fromNode.GetComponent<NodeRoom>();
                if (i == 0)
                {
                    // Primer nodo conecta con todos
                    foreach (Transform toNode in nextColumn)
                    {
                        scriptNode.outNodes.Add(toNode.GetComponent<NodeRoom>());
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
                        scriptNode.outNodes.Add(closest.GetComponent<NodeRoom>());
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
                        NodeRoom scriptNodeIn = closest.GetComponent<NodeRoom>();
                        scriptNodeIn.outNodes.Add(toNode.GetComponent<NodeRoom>());
                        DrawLine(closest.position, toNode.position);
                        hasOutgoing[closest] = true;
                        hasIncoming[toNode] = true;
                    }
                }
            }

            // Asegurar que todos los nodos actuales tengan salida
            foreach (Transform fromNode in currentColumn)
            {
                NodeRoom nodeScript = fromNode.GetComponent<NodeRoom>();
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
                        nodeScript.outNodes.Add(closest.GetComponent<NodeRoom>());
                        DrawLine(fromNode.position, closest.position);
                        hasOutgoing[fromNode] = true;
                        hasIncoming[closest] = true;
                    }
                }
            }
        }
        dungeonManager.EnableRooms();
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

    public void SaveGeneratedMap()
    {
        MapData mapData = new MapData();
        int id = 0;
        Dictionary<Transform, int> nodeToId = new Dictionary<Transform, int>();

        foreach (var column in nodeGrid)
        {
            foreach (var node in column)
            {
                NodeRoom nr = node.GetComponent<NodeRoom>();
                var data = new NodeData
                {
                    id = id,
                    position = node.position,
                    roomType = nr.roomType.name
                };

                nodeToId[node] = id;
                mapData.allNodes.Add(data);
                id++;
            }
        }

        // Agrega las conexiones
        for (int i = 0; i < nodeGrid.Count; i++)
        {
            foreach (Transform node in nodeGrid[i])
            {
                NodeRoom nr = node.GetComponent<NodeRoom>();
                NodeData nodeData = mapData.allNodes[nodeToId[node]];

                foreach (NodeRoom connected in nr.outNodes)
                {
                    Transform target = connected.transform;
                    nodeData.connectedNodeIds.Add(nodeToId[target]);
                }
            }
        }

        // Nodo actual del jugador
        mapData.currentNodeId = nodeToId[dungeonManager.selectedRoom.transform];

        MapSaveSystem.SaveMap(mapData);
    }

    void LoadMapFromData(MapData data)
    {
        nodeGrid.Clear();
        Dictionary<int, Transform> idToNode = new Dictionary<int, Transform>();
        Dictionary<int, NodeRoom> idToScript = new Dictionary<int, NodeRoom>();

        foreach (var nodeData in data.allNodes)
        {
            var node = Instantiate(nodePrefab, nodeData.position, Quaternion.identity, this.transform);
            var script = node.GetComponent<NodeRoom>();
            script.roomType = GetRoomTypeByName(nodeData.roomType);
            script.ShowSpriteRoom();

            idToNode[nodeData.id] = node.transform;
            idToScript[nodeData.id] = script;

            // Agrupar por columnas según posición X
            int columnIndex = Mathf.RoundToInt((nodeData.position.x - startPosition.x) / horizontalSpacing);
            while (nodeGrid.Count <= columnIndex)
                nodeGrid.Add(new List<Transform>());

            nodeGrid[columnIndex].Add(node.transform);
        }

        // Conexiones
        foreach (var nodeData in data.allNodes)
        {
            var script = idToScript[nodeData.id];
            foreach (int targetId in nodeData.connectedNodeIds)
            {
                script.outNodes.Add(idToScript[targetId]);
                DrawLine(idToNode[nodeData.id].position, idToNode[targetId].position);
            }
        }

        // Nodo actual del jugador
        dungeonManager.currentNode = idToScript[data.currentNodeId];
        dungeonManager.EnableRooms();
    }

    private RoomType GetRoomTypeByName(string name)
    {
        RoomType targetRoom = null;
        foreach (var room in allRoomTypes)
        {
            if (room.name == name)
            {
                targetRoom = room;
            }
        }
        return targetRoom;
    }
}
