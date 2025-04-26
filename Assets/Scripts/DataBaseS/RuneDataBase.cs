using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RuneDataBase", menuName = "Scriptable Objects/RuneDataBase")]
public class RuneDataBase : ScriptableObject
{
    public List<RuneBase> allRunes;

    private Dictionary<string, RuneBase> runeDict;

    public void Initialize()
    {
        runeDict = new Dictionary<string, RuneBase>();
        foreach (var rune in allRunes)
        {
            if (!runeDict.ContainsKey(rune.runeName))
                runeDict.Add(rune.runeName, rune);
        }
    }

    public RuneBase GetRuneBaseByName(string runeName)
    {
        if (runeDict == null)
            Initialize();
        return runeDict.TryGetValue(runeName, out var runeBase) ? runeBase : null;
    }
}
