using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAiControl : MonoBehaviour
{
    private GameObject[] goal;
    private NavMeshAgent AI;
    private Animator anim;
    private float timeBetweenRandomGoals = 7f; 
    private float timer = 0f;
    
    void Start()
    {
        AI = this.GetComponent<NavMeshAgent>();
        goal = GameObject.FindGameObjectsWithTag("goal");
        int i = Random.Range(0, goal.Length);
        anim = this.GetComponent<Animator>();
        anim.SetTrigger("Walk");
        ResetAgent();
        StartCoroutine(RandomGoals());
    }
    
    IEnumerator RandomGoals()
    {
        while (AI.enabled == false) 
        {
            yield break;
        }
        if (AI.remainingDistance < 1)
        {
            int i = Random.Range(0, goal.Length);
            AI.SetDestination(goal[i].transform.position);
        }
        yield return new WaitForSeconds(timeBetweenRandomGoals); 
    }
    
    void ResetAgent()
    {
        if (AI.enabled == false)
        {
            return;
        }
        float sm = Random.Range(0.5f, 2);
        AI.speed = sm;
        anim.SetTrigger("Walk");
        AI.angularSpeed = 120;
        AI.ResetPath();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (AI.enabled == false)
        {
            return;
        }
        if (AI.remainingDistance < 1)
        {
            if (timer >= timeBetweenRandomGoals)
            {
                int i = Random.Range(0, goal.Length);
                AI.SetDestination(goal[i].transform.position);
                timer = 0f;
            }
        }
    }
    public void HandleHitEnemy()
    {
        Debug.Log("Enemy Hit");
        anim.SetTrigger("Die");
        AudioManager.Instance.PlaySFX("Death");
        StartCoroutine(HitTarget());
        AI.isStopped = true;
        AI.enabled = false;
        GetComponent<Collider>().enabled = false;
        /*StartCoroutine(DestroyEnemy());*/
    }
    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    IEnumerator HitTarget()
    {
        yield return new WaitForSeconds(.5f);
        AudioManager.Instance.PlaySFX("HitTarget");
    }
    
}
