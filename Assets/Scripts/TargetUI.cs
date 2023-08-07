using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetUI : MonoBehaviour
{
    public Animator animator;
    public float timer;
    public bool isOpen;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Open();
        }
    }


    public void Open()
    {
        isOpen = animator.GetBool("IsOpen");
        animator.SetBool("IsOpen", !isOpen);
    }
}
