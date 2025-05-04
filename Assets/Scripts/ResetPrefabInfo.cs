using UnityEngine;
using TMPro;

public class ResetPrefabInfo : MonoBehaviour
{
    public ManagerRunes runeManager;
    public GameObject lineToConnect;
    public GameObject prefabInfo;
    public TextMeshProUGUI txtInfoPrefab;
    public TextMeshProUGUI txtNamePrefab;
    public void ResetParameters()
    {
        runeManager.lineToConect = lineToConnect;
        runeManager.prefabInfo = prefabInfo;
        runeManager.txtInfoPrefab = txtInfoPrefab;
        runeManager.txtNamePrefab = txtNamePrefab;
    }
}
