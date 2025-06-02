using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using TMPro;

public class EggPanel : MonoBehaviour
{
    public Bestiary bestiary;
    public MonstersHouse monstersHouse;
    public List<Image> imgsBtns = new List<Image>();
    public List<BtnEggProgress> btnEggProgress = new List<BtnEggProgress>();
    public List<Image> imgsitems = new List<Image>();
    public List<TextMeshProUGUI> txtCounts = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> txtTotals = new List<TextMeshProUGUI>();
    public UnityEvent ClosePanel;
    public List<Egg> eggs = new List<Egg>();
    public Image superiorBar;
    public GameObject btnHatch;
    private int i;
    private float totalPoints = 0;
    private bool onPanel = false;

    public void ShowEgg()
    {
        i = 0;
        onPanel = true;
        Egg scriptEgg = bestiary.eggInvoked;
        superiorBar.fillAmount = scriptEgg.eggData.currentPoints / (float)scriptEgg.eggSO.totalPoints;
        if (scriptEgg.eggData.currentPoints >= scriptEgg.eggSO.totalPoints)
        {
            btnHatch.SetActive(true);
            return;
        }
        if (scriptEgg.eggData.currentPoints == 0)
        {
            superiorBar.fillAmount = 0.01f;
        }
        foreach (var item in bestiary.eggInvoked.eggSO.typeItems)
        {
            imgsBtns[i].enabled = true;
            btnEggProgress[i].savedItem = item;
            imgsitems[i].enabled = true;
            imgsitems[i].sprite = item.sprite;
            txtCounts[i].enabled = true;
            txtCounts[i].text = scriptEgg.eggData.itemProgress[i].ToString();
            txtTotals[i].enabled = true;
            txtTotals[i].text = $"/ {bestiary.eggInvoked.eggSO.amountItems[i].ToString()}";
            btnEggProgress[i].current = scriptEgg.eggData.itemProgress[i];
            btnEggProgress[i].total = bestiary.eggInvoked.eggSO.amountItems[i];
            i++;
        }
    }

    public void AddProgress(BtnEggProgress eggProgress)
    {
        if (eggProgress.total <= eggProgress.current)
        {
            return;
        }
        eggProgress.current++;
        Egg scriptEgg = bestiary.eggInvoked;
        int total = scriptEgg.eggSO.totalPoints;
        float pointsPerCollection = total / i;
        float pointsObtained = pointsPerCollection / eggProgress.total;
        scriptEgg.eggData.currentPoints += pointsObtained;
        superiorBar.fillAmount = scriptEgg.eggData.currentPoints / (float)total;
        scriptEgg.eggSpot.imgSuperiorBar.fillAmount = superiorBar.fillAmount;
        txtCounts[eggProgress.valueI].text = eggProgress.current.ToString();
        scriptEgg.eggData.itemProgress[eggProgress.valueI] = eggProgress.current;
        if (total <= scriptEgg.eggData.currentPoints)
        {
            btnHatch.SetActive(true);
            DesactiveItems();
        }
    }

    public void SaveEggs()
    {
        List<EggData> listEggData = new List<EggData>();
        foreach (var egg in eggs)
        {
            listEggData.Add(egg.eggData);
        }
        monstersHouse.eggs = listEggData;
    }

    private void Update()
    {
        if (onPanel && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel.Invoke();
            onPanel = true;
        }
    }

    private void DesactiveItems()
    {
        for (int e = 0; e <= i ; e++)
        {
            imgsBtns[e].enabled = false;
            imgsitems[e].enabled = false;
            txtCounts[e].enabled = false;
            txtTotals[e].enabled = false;
        }
    }
}
