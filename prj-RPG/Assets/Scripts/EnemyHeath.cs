using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int startingHealth = 3;

    private int currentHealth;

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth); // Kiểm tra giá trị máu hiện tại
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
