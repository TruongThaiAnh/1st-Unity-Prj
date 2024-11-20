using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whiteFlashMat;
    [SerializeField] private float restoreDefaulTime = .2f;

    private Material defaulMat;
    private SpriteRenderer spriteRenderer;
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        defaulMat = spriteRenderer.material;
        enemyHealth = GetComponent<EnemyHealth>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator FlashRoutine()
    {
       spriteRenderer.material = whiteFlashMat;
        yield return new WaitForSeconds(restoreDefaulTime);
        spriteRenderer.material = defaulMat;
        enemyHealth.DetectDeath();
    }
}
