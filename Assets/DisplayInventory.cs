using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    // Inventory that we want to display
    public InventoryObject inventory;

    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEMS;
    public int Y_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMNS;
    // Dictionary for the items display
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    private void CreateDisplay()
    {
        for(int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            // Now we geet to get object in child, which is TextMeshProUGUI, and set that object to the amount
            // of that item that we have. ToString("n0") <- the argument is just for the nice formatting
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amout.ToString("n0");
            // Adding the item that gets created to the items displayed dictionary
            itemsDisplayed.Add(inventory.Container[i], obj);

        }
    }

    // The argument need to know the index of the item in the inventory
    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMNS)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS)), 0f);
    }

    private void UpdateDisplay()
    {
        for(int i = 0; i < inventory.Container.Count; i++)
        {
            // We are checking if that item is already in our inventory
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amout.ToString("n0");
            }
            else
            {
                // Same what we did in the CreateDisplay() function
                var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amout.ToString("n0");
                itemsDisplayed.Add(inventory.Container[i], obj);

            }
        }
    }
}
