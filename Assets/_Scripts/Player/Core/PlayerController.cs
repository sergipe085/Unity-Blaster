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

    [Header("UI")]
    [SerializeField] private OverheadUI overheadUI = null;

    [Header("MODEL")]
    [SerializeField] private Transform model = null;

    public override void OnNetworkSpawn() {
        SetNetworkRoleUI();
    }

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

    private void SetNetworkRoleUI() {
        overheadUI.SetNetworkRoleText(GetNetworkRole());
    }

    private string GetNetworkRole() {
        // Se for server = Authority
        // Se for client e for owned = Autonomous Proxy
        // Se for client e nao for owner = Simulated Proxy

        string networkRole;

        if (IsServer) {
            networkRole = "Authority";
        }
        else if (IsOwner) {
            networkRole = "Autonomous Proxy";
        }
        else {
            networkRole = "Simulated Proxy";
        }

        return networkRole;
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
        model.transform.forward = lookCamera.GetForwardVector();
    }    
}
