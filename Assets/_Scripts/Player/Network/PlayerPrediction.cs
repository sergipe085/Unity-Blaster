using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Blaster.Player.Network 
{
    public class PlayerPrediction : NetworkBehaviour
    {
        private CharacterController characterController = null;

        //CLIENT CODE
        #region Client

        [SerializeField] private int updateServerTimesPerSecond = 4;
        private float time = 0.0f;

        public List<PlayerMoveInput> moveInputsToSendToServer = new List<PlayerMoveInput>();
        public List<PlayerMoveInput> unproceccedMoveInput     = new List<PlayerMoveInput>();

        private int currentRequestId = 0;

        private Vector2 currentMoveInput = Vector2.zero;

        private void Start() {
            characterController = GetComponent<CharacterController>();
        }

        private void Update() {
            if (!IsOwner) {
                return;
            }


            time += Time.deltaTime;
            if (time >= 1f / updateServerTimesPerSecond) {
                SendInputsToServer();
                time = 0.0f;
            }

            currentMoveInput = GetMoveInput();

            currentRequestId++;
        }

        private void FixedUpdate() {
            if (!IsOwner) return;

            PlayerMoveInput playerMoveInput = new PlayerMoveInput() {
                requestId = currentRequestId,
                inputDirection = currentMoveInput,
                jump = false
            };

            if (!IsServer) {
                CaptureInputToSendToServer(playerMoveInput);
            }
            else {
                ProcessInput(playerMoveInput);
            }
        }
        
        private void SendInputsToServer() {
            ReceiveInputsFromClientServerRpc(moveInputsToSendToServer.ToArray());
            moveInputsToSendToServer.Clear();
        }

        private void CaptureInputToSendToServer(PlayerMoveInput playerMoveInput) {
            if (playerMoveInput.inputDirection == Vector2.zero) return;

            moveInputsToSendToServer.Add(playerMoveInput);
            unproceccedMoveInput.Add(playerMoveInput);

            ProcessInput(playerMoveInput);

        }

        private Vector2 GetMoveInput() {
            float xMove = Input.GetAxisRaw("Horizontal");
            float yMove = Input.GetAxisRaw("Vertical");
            Vector2 moveInput = new Vector2(xMove, yMove);
            return moveInput;
        }

        [ClientRpc]
        private void ReceiveInputsProceccedFromServerClientRpc(int[] proceccedInputIds, Vector3 position) {
            foreach(int proceccedInputId in proceccedInputIds) {
                unproceccedMoveInput.Remove(unproceccedMoveInput.Find((item) => item.requestId == proceccedInputId));
            }

            transform.position = position;

            Debug.Log(unproceccedMoveInput.Count);
            ProcessInputs(unproceccedMoveInput);
        }

        private void ProcessInput(PlayerMoveInput playerMoveInput) {
            Vector3 direction = new Vector3(playerMoveInput.inputDirection.x, 0f, playerMoveInput.inputDirection.y);
            characterController.Move(direction.normalized * 0.3f);
        }

        private void ProcessInputs(IEnumerable playerMoveInputs) {
            foreach(PlayerMoveInput playerMoveInput in playerMoveInputs) {
                Vector3 direction = new Vector3(playerMoveInput.inputDirection.x, 0f, playerMoveInput.inputDirection.y);
                characterController.Move(direction.normalized * 0.3f);
            }
        }

        private int[] ProcessInputsAndGetIds(IEnumerable playerMoveInputs) {
            List<int> proceccedIds = new List<int>();

            foreach(PlayerMoveInput playerMoveInput in playerMoveInputs) {
                Vector3 direction = new Vector3(playerMoveInput.inputDirection.x, 0f, playerMoveInput.inputDirection.y);
                characterController.Move(direction.normalized * 0.3f);
                proceccedIds.Add(playerMoveInput.requestId);
            }

            return proceccedIds.ToArray();
        }


        //Capture input Array
        //Send input to server (x times per second)
        //Clear input array
        // --- Server process inputs -- 
        // Receive new position input and the inputs procecceds
        // Update position do server position and do the unprocecced inputs


        #endregion
    
        //SERVER CODE
        #region Server

        [ServerRpc]
        private void ReceiveInputsFromClientServerRpc(PlayerMoveInput[] playerMoveInputs) {
            int[] ids = ProcessInputsAndGetIds(playerMoveInputs);
            ReceiveInputsProceccedFromServerClientRpc(ids, transform.position);
        }

        #endregion
    }
}
