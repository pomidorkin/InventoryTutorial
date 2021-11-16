using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;

    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<Item>();
        // If we were able to find an item (i.e. the 'other' might not be an item)
        if (item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        // Очень важно! Весь инвентарь - это один ScriptableObject, который автоматически
        // всё сохраняет. И если нам это не нужно, то мы принудительно его очищаем
        // при закрытии приложения
        inventory.Container.Clear();
    }
}
