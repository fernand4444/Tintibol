using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damageAmount = 10;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerControler player = other.gameObject.GetComponent<PlayerControler>();
            if (player != null)
            {
                int amount = 0;

                bool applyKnockback = !gameObject.CompareTag("HitKill");
                bool ignoreCooldown = gameObject.CompareTag("HitKill");

                if (gameObject.CompareTag("HitKill")) 
                {
                    amount = 999999;
                }
                else if (gameObject.CompareTag("obstaculo"))
                {
                    amount = 25;
                }
                else
                {
                    amount = damageAmount;
                }

                player.TakeDamage(amount, applyKnockback, ignoreCooldown);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}