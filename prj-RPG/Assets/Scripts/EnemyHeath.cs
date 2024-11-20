using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;

    private int currentHealth;

    private KnockBack knockBack;

    private void Awake()
    {
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
        DetectDeath();
    }

   private void DetectDeath()
{
    Debug.Log($"DetectDeath called. Current Health: {currentHealth}");
    if (currentHealth <= 0)
    {
        Destroy(gameObject);
    }
}

}
