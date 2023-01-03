using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteractUI
{
    private PlayerInteract         playerInteract;
    private PlayerInteractUISettings playerInteractUISettings;

    public PlayerInteractUI(PlayerInteract _playerInteract) {
        playerInteract = _playerInteract;

        playerInteract.OnStartInteractingEvent += Handle_PlayerInteract_OnStartInteractingEvent;
        playerInteract.OnEndInteractingEvent   += Handle_PlayerInteract_OnEndInteractingEvent;
    }

    ~PlayerInteractUI() {
        playerInteract.OnStartInteractingEvent -= Handle_PlayerInteract_OnStartInteractingEvent;
        playerInteract.OnEndInteractingEvent   -= Handle_PlayerInteract_OnEndInteractingEvent;
    }

    private void Handle_PlayerInteract_OnStartInteractingEvent(Interactable interactable) {
        InteractUI.Instance.OpenInteractUI(KeyCode.E, interactable.transform.name);
    }

    private void Handle_PlayerInteract_OnEndInteractingEvent(Interactable interactable) {
        InteractUI.Instance.CloseInteractUI();
    }
}

[System.Serializable]
public struct PlayerInteractUISettings {
    public InteractUI      interactUI;
}
