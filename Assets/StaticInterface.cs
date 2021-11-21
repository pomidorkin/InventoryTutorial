using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    // 4) And we are gonna take these inventory slot prefabs, link the to the same slot
    // inside the database
    public GameObject[] slots;
    public override void CreateSlots()
    {
        // Making sure there is no liks between our equipment database and our equipment display
        // 1) We created a new dictionary of our items displayed, confirming that we don't have
        // an old dictionary
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        // 2) We are loopoing through or equipment inside of our datebase (which is the
        // scriptable object)
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            // 3) Then we are creating an object that liks to our array, that would link to
            // the actual GameObjects in our scene, the interface that we created in the editor
            var obj = slots[i];

            // https://youtu.be/0NG_dXsPXg0?t=960
            // 5) Add pointer events to them 
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            // 5) and then add them to the actual itemsDisplayed, so they will be linked
            slotsOnInterface.Add(obj, inventory.Container.Items[i]);
        }
    }
}
