using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
/// <summary>
/// Creates and manages the UI object of an inventory.
/// </summary>
public class InventoryUIManager
{
    Inventory inventory;
	WorkTableBlock worktable;

    GameObject imagePrefab;
    GameObject textPrefab;

    GameObject UI;
    Content content;
	int yOffset;

	public InventoryUIManager(WorkTableBlock worktable)
    {
		this.worktable = worktable;
		this.inventory = worktable.inventory;
		yOffset = -30;
		
		Setup();
    }
	
    public InventoryUIManager(Inventory inventory)
    {	
		this.inventory = inventory;
		yOffset = 0;
		Setup();
    }
	
	private void Setup() {
		
        UI = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/InventoryUI"));
        content = UI.GetComponentInChildren<Content>();
        UI.GetComponentInChildren<Button>().onClick.AddListener(delegate { Delete(); }); ;

        imagePrefab = Resources.Load<GameObject>("Prefabs/Image");
        textPrefab = Resources.Load<GameObject>("Prefabs/Text");
		
        Render();
        inventory.AddUIManager(this);
	}
	
    /// <summary>
    /// Renders the inventory by creating UI Images and UI Texts
    /// </summary>
    public void Render()
    {
        //Destroy old InventoryUI content
        foreach (var image in content.GetComponentsInChildren<Image>())
        {
            GameObject.Destroy(image.gameObject);
        }
        foreach (var text in content.GetComponentsInChildren<Text>())
        {
            GameObject.Destroy(text.gameObject);
        }
		
		if (worktable != null) {
			CreateWorktableUIElements();
		}

        //Create new InventoryUI content
        for (int i = 0; i < inventory.StackAmount; i++)
        {
            CreateItemImage(inventory.GetStack(i).Item, i);
            CreateItemText(inventory.GetStack(i).Size, i);
        }
    }
    /// <summary>
    /// Creates and positions the Image for an ItemStack in an Inventory
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    void CreateItemImage(Item item, int index)
    {
        Image image = GameObject.Instantiate(imagePrefab, content.transform).GetComponent<Image>();

        image.sprite = ItemSprites.GetItemSprite(item.name);
        image.rectTransform.anchoredPosition += new Vector2(10 * (index % 3),-15*Mathf.FloorToInt(index/3) + yOffset);
    }
    /// <summary>
    /// Creates and positions the Image for an ItemStack in an Inventory
    /// </summary>
    /// <param name="size"></param>
    /// <param name="index"></param>
    void CreateItemText(int size, int index)
    {
        Text text = GameObject.Instantiate(textPrefab, content.transform).GetComponent<Text>();

        text.text = "" + size;
        text.rectTransform.anchoredPosition += new Vector2(10 * (index % 3), -15 * Mathf.FloorToInt(index / 3) + yOffset);
    }

	private void CreateWorktableUIElements() {
		Text workDone = GameObject.Instantiate(textPrefab, content.transform).GetComponent<Text>();
		workDone.text = "" + worktable.tasks[worktable.currentTask].workToComplete - worktable.tasks[worktable.currentTask].workToComplete + "/" + worktable.tasks[worktable.currentTask].workNeeded;
	}
	
    /// <summary>
    /// Call this to destroy the InventoryUI
    /// </summary>
    public void Delete()
    {
        GameObject.Destroy(UI);
        GameObject.Destroy(content);
        inventory.RemoveUIManager(this);
    }
}
