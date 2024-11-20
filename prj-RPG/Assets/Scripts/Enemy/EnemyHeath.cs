﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;
    [SerializeField] private GameObject deathVFXPrefabs; 

    private int currentHealth;

    private KnockBack knockBack;
    private Flash flash;


    private void Awake()
    {
        flash = GetComponent<Flash>();
        knockBack = GetComponent<KnockBack>();  
    }

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        knockBack.GetKnockedBack(PlayerController.Instance.transform,15f);
        StartCoroutine(flash.FlashRoutine());
    }

    public void DetectDeath()
{
    Debug.Log($"DetectDeath called. Current Health: {currentHealth}");
    if (currentHealth <= 0)
    {
        Instantiate(deathVFXPrefabs, transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}

}
