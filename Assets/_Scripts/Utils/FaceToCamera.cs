using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    private Camera camToLook = null;

    private void Start() {
        camToLook = Camera.main;
    }

    private void Update() {
        transform.LookAt(camToLook.transform);
    }
}
