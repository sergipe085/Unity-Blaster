using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    public event Action<Vector2> OnMoveEvent = null;
    public event Action<Vector2> OnLookEvent = null;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;

    public void CheckInput() {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        OnMoveEvent?.Invoke(moveInput);

        lookInput.x = Input.GetAxis("Mouse X");
        lookInput.y = Input.GetAxis("Mouse Y");

        if (lookInput.magnitude != 0.0f) {
            OnLookEvent?.Invoke(lookInput);
        }
    }
}
