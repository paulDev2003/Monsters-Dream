using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NodeData
{
    public int id;
    public Vector2 position;
    public string roomType;
    public List<int> connectedNodeIds = new List<int>();
}