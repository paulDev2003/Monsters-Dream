using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class MapData
{
    public List<NodeData> allNodes = new List<NodeData>();
    public int currentNodeId;
}