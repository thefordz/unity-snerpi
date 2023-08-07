using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AiControl : MonoBehaviour
{
    private GameObject[] goal;
    private NavMeshAgent AI;
    private Animator anim;
    private float timeBetweenRandomGoals = 7f; 
    private float timer = 0f;
    private GameObject bullet;

    void Start()
    {
        AI = this.GetComponent<NavMeshAgent>();
        goal = GameObject.FindGameObjectsWithTag("goal");
        int i = Random.Range(0, goal.Length);
        anim = this.GetComponent<Animator>();
        anim.SetTrigger("Walk");
        ResetAgent();
        StartCoroutine(RandomGoals());
        //anim.SetFloat("offset",Random.Range(0.0f,1.0f));
    }
    
    public void HandleHitAI()
    {
        Debug.Log("AI Hit");
        anim.SetTrigger("Die");
        AudioManager.Instance.PlaySFX("Death");
        StartCoroutine(WrongTarget());
        AI.isStopped = true;
        AI.enabled = false; 
        StartCoroutine(DestroyAI());
        GetComponent<Collider>().enabled = false;
    }
    
    IEnumerator WrongTarget()
    {
        yield return new WaitForSeconds(.5f);
        AudioManager.Instance.PlaySFX("WrongTarget");
    }

    //Jobb game

    IEnumerator DestroyAI()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
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
    
    private void OnTriggerStay(Collider other)
    {   
        Debug.Log("Stay");
        if (other.CompareTag("Detection Area"))
        {
            Debug.Log("Detect");
            anim.SetTrigger("Run");
            AI.speed = 5;
            AI.angularSpeed = 500;
            /*StartCoroutine(ResetAfterDelay());*/
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        if (other.CompareTag("Detection Area"))
        {
            StartCoroutine(ResetAfterDelay());
        }
    }
    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(10f);
        ResetAgent();
    }

}
