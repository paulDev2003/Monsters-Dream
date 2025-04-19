using UnityEngine;

[CreateAssetMenu(fileName = "MonsterBase", menuName = "Scriptable Objects/MonsterBase")]
public class MonsterBase : ScriptableObject
{
    public int id;
    public string monsterName;
    public GameObject prefabMonster;
    public MonsterSO monsterSO;
}
