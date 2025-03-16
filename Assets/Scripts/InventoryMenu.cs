using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TMPro;

public class InventoryMenu : MonoBehaviour
{
    public List<Image> listImages;
    public List<TextMeshProUGUI> listTexts;
    private Inventory inventory;
    [SerializeField] private Image image;

    public enum typeList
    {
        Molecules,
        Capturables
    }
    public typeList type;

    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
    }

    public void UpdateItems()
    {
        image.enabled = true;
        int i = 0;
        switch (type)
        {
            case typeList.Molecules:
                foreach (var item in inventory.moleculeInventory)
                {
                    int amount = inventory.countMolecules[item.Key];
                    ActivateView(i, amount, item.Value.sprite);
                    i++;
                }
                break;
            case typeList.Capturables:
                foreach (var item in inventory.capturableInventory)
                {
                    int amount = inventory.countCapturables[item.Key];
                    ActivateView(i, amount, item.Value.sprite);
                    i++;
                }
                break;
        }
    }

    private void ActivateView(int i, int amount, Sprite sprite)
    {
        listImages[i].sprite = (sprite);
        listImages[i].enabled = true;
        listTexts[i].text = amount.ToString();
    }

    
}
