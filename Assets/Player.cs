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
        // ����� �����! ���� ��������� - ��� ���� ScriptableObject, ������� �������������
        // �� ���������. � ���� ��� ��� �� �����, �� �� ������������� ��� �������
        // ��� �������� ����������
        inventory.Container.Clear();
    }
}
