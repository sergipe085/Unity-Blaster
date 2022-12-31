using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    [Header("INPUT")]
    private PlayerInput playerInput = null;

    [Header("COMPONENTS")]
    [SerializeField] private LookCamera lookCamera = null;
    [SerializeField] private Rigidbody rig = null;
    private Mover mover = null;

    [Header("SETTINGS")]
    [SerializeField] private MoverSettings moverSettings;

    private void Start() {
        if (IsOwner) {
            InitializeInput();
        }

        SetupCursor();
        InitializeComponents();
    }

    private void Update() {
        if (!IsOwner) return;

        playerInput?.CheckInput();
    }

    private void InitializeComponents() {
        mover = new Mover(rig, moverSettings);
    }

    private void InitializeInput() {
        playerInput = new PlayerInput();
        playerInput.OnMoveEvent += Handle_PlayerInput_OnMoveEvent;
        playerInput.OnLookEvent += Handle_PlayerInput_OnLookEvent;
    }

    private void SetupCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Handle_PlayerInput_OnMoveEvent(Vector2 moveInput) {
        //Move Player
        Debug.Log(moveInput);
        mover.Move(moveInput, lookCamera.GetForwardVector());
    }  

    private void Handle_PlayerInput_OnLookEvent(Vector2 lookInput) {
        //Look Camera
        lookCamera.ApplyLook(lookInput);
    }    
}