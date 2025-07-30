using System;
using UnityEngine;

namespace Sensors
{
    public class EnemySensor : MonoBehaviour
    {
        [SerializeField] private bool forgetOnExit = false;
        public Player player;

        public Action<Player> OnEnter;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            player = other.GetComponent<Player>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (!forgetOnExit) return;

            player = null;
        }
    }
}