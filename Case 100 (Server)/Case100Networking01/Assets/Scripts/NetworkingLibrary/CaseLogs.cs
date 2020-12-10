using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace CaseNetworking
{
    public class CaseLogs : MonoBehaviour
    {
        public static CaseLogs instance;

        [SerializeField]
        private GameObject messagePrefab;

        [SerializeField]
        private Transform messageContainer;

        //private List<GameObject> messages = new List<GameObject>();

        private void Awake()
        {
            instance = this;
        }

        public void AddToLog(string message)
        {
            if (messagePrefab != null && messageContainer != null)
            {
                GameObject msg = Instantiate(messagePrefab, messageContainer);
                msg.GetComponentInChildren<TextMeshProUGUI>().text = AdminServerManagement.instance.timer.CurrentTimeFormat() + ":" + message;
                // messages.Add(msg);
            }
        }

        public void Clear()
        {
            // messages.Clear();
        }
    }
}