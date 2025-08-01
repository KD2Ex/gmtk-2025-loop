using System;
using System.Collections;
using Sensors;
using UnityEngine;

namespace Entities.Drops.Coins
{
    public class Coin: MonoBehaviour
    {
        public int coinValue;
        public float moveSpeed;
        private Player player;
        private Rigidbody2D rb;
        [SerializeField] private EnemySensor senor;

        private bool exec = false;

        private void OnEnable()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            senor.OnEnter += OnPlayerEnterChase;
        }

        private void OnPlayerEnterChase(Player player)
        {
            this.player = player;
            StartCoroutine(WaitToFindPlayer(player));
        }

        IEnumerator WaitToFindPlayer(Player player)
        {
            yield return new WaitForSeconds(0.3f);
            //this.player = player;
            exec = true;
        }

        private void Update()
        {
            if (!exec) return;
            
            Chase();
        }
        
        private void Chase()
        {
            var dir = (player.transform.position - transform.position).normalized;
            rb.velocity = dir * moveSpeed;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            player.GetComponent<Inventory>().coins += coinValue;
            Destroy(gameObject);
        }
    }
}