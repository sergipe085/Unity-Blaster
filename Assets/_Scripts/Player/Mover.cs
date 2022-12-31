using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mover
{
    private MoverSettings moverSettings;
    private Rigidbody rig = null;

    public Mover(Rigidbody _rig, MoverSettings _moverSettings) {
        this.rig = _rig;
        moverSettings = _moverSettings;
    }

    public void Move(Vector2 moveInput, Vector3 forward) {
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        Vector3 direction = right * moveInput.x + forward * moveInput.y;
        rig.velocity = direction.normalized * moverSettings.moveSpeed;
    }
}

[System.Serializable]
public struct MoverSettings {
    public float moveSpeed;
}
