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
		inventory = worktable.inventory;
		yOffset = 10;
        worktable.AddUIManager(this);
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

        if (worktable != null)
        {
            UI.GetComponentInChildren<UIName>().gameObject.GetComponent<Text>().text = "W O R K T A B L E";
        }

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
            CreateWorktableText();
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
        image.rectTransform.anchoredPosition += new Vector2(10 * (index % 3),-15*Mathf.FloorToInt(index/3) - yOffset);
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
        text.rectTransform.anchoredPosition += new Vector2(10 * (index % 3), -15 * Mathf.FloorToInt(index / 3) - yOffset);
    }

    private void CreateWorktableText()
    {
        Text workDone = GameObject.Instantiate(textPrefab, content.transform).GetComponent<Text>();
        Text requirements = GameObject.Instantiate(textPrefab, content.transform).GetComponent<Text>();
        Text products = GameObject.Instantiate(textPrefab, content.transform).GetComponent<Text>();

        Task task = worktable.tasks[worktable.currentTask];

        //Shows the work done/work needed to finish e.g. 267/1200
        workDone.text = "" + (task.workToComplete - task.workNeeded) + "/" + task.workToComplete;

        requirements.text = "Requirements: ";
        for (int i = 0; i < task.requiredResources.Length; i++)
        {
            requirements.text += task.requiredResources[i].Item.name;
            if (i + 1 < task.requiredResources.Length)
            {
                requirements.text += ", ";
            }
        }

        products.text = "Product: ";
        for (int i = 0; i < task.product.Length; i++)
        {
            products.text += task.product[i].Item.name;
            if (i + 1 < task.product.Length)
            {
                products.text += ", ";
            }
        }

        //Move texts so that they don't overlap etc
        workDone.alignment = TextAnchor.MiddleLeft;
        requirements.alignment = TextAnchor.MiddleLeft;
        products.alignment = TextAnchor.MiddleLeft;

        workDone.rectTransform.anchoredPosition += new Vector2(45, 20);
        requirements.rectTransform.anchoredPosition += new Vector2(45, 15);
        products.rectTransform.anchoredPosition += new Vector2(45, 10);
    }

    /// <summary>
    /// Call this to destroy the InventoryUI
    /// </summary>
    public void Delete()
    {
        GameObject.Destroy(UI);
        GameObject.Destroy(content);
        inventory.RemoveUIManager(this);
        worktable.RemoveUIManager(this);
    }
}
