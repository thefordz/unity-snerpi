using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    #region "Variables"
    public Rigidbody rigid;
    public float mouseSensitivity;
    public float moveSpeed;
    #endregion

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update ()
    {
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0)));
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(new Vector3(Input.GetAxis("Mouse Y") * mouseSensitivity, 0, 0)));
        rigid.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * moveSpeed) + (transform.right * Input.GetAxis("Horizontal") * moveSpeed));
    }
}