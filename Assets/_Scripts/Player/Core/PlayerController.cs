using System.Collections;
using System.Collections.Generic;
using Blaster.Player.UI;
using Unity.Netcode;
using UnityEngine;

namespace Blaster.Player.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        [Header("INPUT")]
        private PlayerInput playerInput = null;

        [Header("COMPONENTS")]
        [SerializeField] private LookCamera lookCamera = null;
        [SerializeField] private Rigidbody rig = null;
        private Mover            mover            = null;
        private PlayerInteract   playerInteract   = null;
        private PlayerInteractUI playerInteractUI = null;

        [Header("SETTINGS")]
        [SerializeField] private MoverSettings moverSettings;
        [SerializeField] private PlayerInteractSettings playerInteractSettings;

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
            if (!IsOwner) {
                overheadUI.SetNetworkRoleText(GetNetworkRole());
            }
            else {
                Destroy(overheadUI.gameObject);
            }
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
            playerInteract = new PlayerInteract(playerInteractSettings);
            playerInteractUI = new PlayerInteractUI(playerInteract);
        }

        private void InitializeInput() {
            playerInput = new PlayerInput();
            playerInput.OnMoveEvent     += Handle_PlayerInput_OnMoveEvent;
            playerInput.OnLookEvent     += Handle_PlayerInput_OnLookEvent;
            playerInput.OnInteractEvent += Handle_PlayerInput_OnInteractEvent;
        }

        private void SetupCursor() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Handle_PlayerInput_OnMoveEvent(Vector2 moveInput) {
            //Move Player
            mover.Move(moveInput, lookCamera.GetMovementForwardVector());
        }  

        private void Handle_PlayerInput_OnLookEvent(Vector2 lookInput) {
            //Look Camera
            lookCamera.ApplyLook(lookInput);
            model.transform.forward = lookCamera.GetMovementForwardVector();
            playerInteract.CheckInteracting(lookCamera.GetPosition(), lookCamera.GetForwardVector());
        }    

        private void Handle_PlayerInput_OnInteractEvent() {
            playerInteract.Interact(lookCamera.GetPosition(), lookCamera.GetForwardVector());
        }
    }
}
