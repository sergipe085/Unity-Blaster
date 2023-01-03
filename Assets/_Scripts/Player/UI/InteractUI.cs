using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractUI : Singleton<InteractUI>
{
    [SerializeField] private TextMeshProUGUI interactText = null;

    private void Start() {
        CloseInteractUI();
    }

    public void OpenInteractUI(KeyCode interactInputKey, string interactingName) {
        this.gameObject.SetActive(true);
        interactText.text = $"[{interactInputKey.ToString()}] {interactingName}";
    }

    public void CloseInteractUI() {
        this.gameObject.SetActive(false);
        interactText.text = "";
    }
}
