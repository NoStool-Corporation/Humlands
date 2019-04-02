using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class InventoryUIManager
{
    Inventory inventory;

    GameObject imagePrefab;
    GameObject textPrefab;

    GameObject UI;
    Content content;

    public InventoryUIManager(Inventory inventory)
    {
        this.inventory = inventory;

        UI = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/InventoryUI"));
        content = UI.GetComponentInChildren<Content>();
        UI.GetComponentInChildren<Button>().onClick.AddListener(delegate { Delete(); }); ;

        imagePrefab = Resources.Load<GameObject>("Prefabs/Image");
        textPrefab = Resources.Load<GameObject>("Prefabs/Text");

        Render();
    }

    public void Render()
    {
        for (int i = 0; i < inventory.StackAmount; i++)
        {
            CreateImage(inventory.GetStack(i).Item, i);
            CreateText(inventory.GetStack(i).Size, i);
        }
    }

    void CreateImage(Item item, int index)
    {
        Image image = GameObject.Instantiate(imagePrefab, content.transform).GetComponent<Image>();

        image.sprite = ItemSprites.GetItemSprite(item.name);
        image.rectTransform.anchoredPosition += new Vector2(10 * (index % 3),-15*Mathf.FloorToInt(index/3));
    }

    void CreateText(int size, int index)
    {
        Text text = GameObject.Instantiate(textPrefab, content.transform).GetComponent<Text>();

        text.text = "" + size;
        text.rectTransform.anchoredPosition += new Vector2(10 * (index % 3), -15 * Mathf.FloorToInt(index / 3));
    }

    public void Delete()
    {
        GameObject.Destroy(UI);
        GameObject.Destroy(content);
        GameObject.Destroy(imagePrefab);
        GameObject.Destroy(textPrefab);
    }
}
