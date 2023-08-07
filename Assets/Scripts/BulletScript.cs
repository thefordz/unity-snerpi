using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class BulletScript : MonoBehaviour
{
    private Rigidbody _rigid;
    public float bulletVelo;
    
    private bool hasHit = false;
    public float triggerDelay = 0.1f;
    [SerializeField] private GameManager _gameManager;

    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        _rigid.AddForce(transform.forward * bulletVelo);
        StartCoroutine(DestroyBullet());
 
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (!hasHit)
        {
            if (col.CompareTag("AI"))
            {
                Debug.Log("AI Hit");
                col.gameObject.GetComponent<AiControl>().HandleHitAI();
                _gameManager.ScoreChange(-350);
                StartCoroutine(DestroyBullet());
            }
            else if (col.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy");
                col.gameObject.GetComponent<EnemyAiControl>().HandleHitEnemy();
                _gameManager.ScoreChange(300);
                StartCoroutine(DestroyBullet());
            }
            else if (col.CompareTag("Crow"))
            {
                col.gameObject.SetActive(false);
                _gameManager.ScoreChange(250);
                StartCoroutine(CrowShoot());
                StartCoroutine(DestroyBullet());
            }
            else if (col.CompareTag("Cardinal"))
            {
                col.gameObject.SetActive(false);
                StartCoroutine(CardinalShoot());
                _gameManager.ScoreChange(-250);
                StartCoroutine(DestroyBullet());
            }
        }
    }
    
    IEnumerator CrowShoot()
    {
        yield return new WaitForSeconds(.5f);
        AudioManager.Instance.PlaySFX("CrowShoot");
    }
    
    IEnumerator CardinalShoot()
    {
        yield return new WaitForSeconds(.5f);
        AudioManager.Instance.PlaySFX("CardinalShoot");
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(triggerDelay);
        Destroy(gameObject);
    }
    
}


