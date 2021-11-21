using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// This class hold the item that this object is representing

/*
 * ISerializationCallbackReceiver: Everytime you make an editor change, (In the editor we can only see
 * serialized data, so we do a serialization change) it's going to fire OnAfterDeserialize and
 * OnBeforeSerialize methonds.
 */
public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
{
    public ItemObject item;

    public void OnAfterDeserialize()
    {

    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
        // This let's unity that somethind hanged on this object, so you are able to save it
        EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
#endif
    }
}
