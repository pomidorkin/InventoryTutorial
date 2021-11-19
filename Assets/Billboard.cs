using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera _camera;
    // The reason we are using LateUpdate is because we want this to happen after
    // everything else has happened so that we avoid any glitches
    private void LateUpdate()
    {
        transform.forward = _camera.transform.forward;
    }
}
