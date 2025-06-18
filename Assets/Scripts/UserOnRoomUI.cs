using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    public class UserOnRoomUI : MonoBehaviour
    {

        public TextMeshProUGUI id;
        private void Start()
        {
            id = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}