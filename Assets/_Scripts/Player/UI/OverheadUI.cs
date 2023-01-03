using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blaster.Player.UI
{
    public class OverheadUI : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI networkRoleText = null;

        public void SetNetworkRoleText(string networkRole) {
            networkRoleText.text = networkRole;
        }
    }
}
