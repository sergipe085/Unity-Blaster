using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    public event Action<Vector2> OnMoveEvent     = null;
    public event Action<Vector2> OnLookEvent     = null;
    public event Action          OnInteractEvent = null;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;

    // KEYS
    private KeyCode interactKey = KeyCode.E;

    public void CheckInput() {
        // MOVE INPUT
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        OnMoveEvent?.Invoke(moveInput);

        // LOOK INPUT
        lookInput.x = Input.GetAxis("Mouse X");
        lookInput.y = Input.GetAxis("Mouse Y");
            OnLookEvent?.Invoke(lookInput);
        if (lookInput.magnitude != 0.0f) {
        }

        // INTERACT INPUT
        if (Input.GetKeyDown(interactKey)) {
            OnInteractEvent?.Invoke();
        }
    }
}
