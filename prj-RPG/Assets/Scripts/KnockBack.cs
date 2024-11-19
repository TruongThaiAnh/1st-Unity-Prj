using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class KnockBack : MonoBehaviour
{
    public bool getttingKnockedBack {  get; private set; }
    [SerializeField] private float knockBackTIme = .2f;
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetKnockedBack(Transform damageSource , float knockBackThrust)
    {
        getttingKnockedBack = true;
        Vector2 difference = (transform.position - damageSource.position).normalized*knockBackThrust*rb.mass;
        rb.AddForce (difference,ForceMode2D.Impulse);
        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(knockBackTIme);
        rb.velocity = Vector2.zero;
        getttingKnockedBack = false;
    }
}
