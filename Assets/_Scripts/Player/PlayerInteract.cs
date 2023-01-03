using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract
{
    private PlayerInteractSettings playerInteractSettings;
    private Interactable currentInteracting = null;

    public event Action<Interactable> OnStartInteractingEvent = null;
    public event Action<Interactable> OnEndInteractingEvent   = null;
    public event Action<Interactable> OnInteractEvent         = null;

    public PlayerInteract(PlayerInteractSettings _playerInteractSettings) {
        playerInteractSettings = _playerInteractSettings;
    }

    public void CheckInteracting(Vector3 position, Vector3 direction) {
        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit, playerInteractSettings.maxDistance, playerInteractSettings.interactableLayerMask)) {
            bool hasInteractable = hit.transform.TryGetComponent<Interactable>(out Interactable interactable);
            if (hasInteractable) {
                currentInteracting = interactable;
                OnStartInteractingEvent?.Invoke(currentInteracting);
                return;
            }
        }

        OnEndInteractingEvent?.Invoke(currentInteracting);
        currentInteracting = null;
    }

    public void Interact(Vector3 position, Vector3 direction) {
        if (!currentInteracting) return;

        currentInteracting.Interact();
        OnInteractEvent?.Invoke(currentInteracting);
        OnEndInteractingEvent?.Invoke(currentInteracting);
    }
}

[System.Serializable]
public struct PlayerInteractSettings {
    public float maxDistance;
    public LayerMask interactableLayerMask;
}
