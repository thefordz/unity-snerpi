using System;
using System.Collections;
using UnityEngine;
using Image = UnityEngine.UI.Image;

[RequireComponent(typeof(CharacterController))]

public class SC_FPSController : MonoBehaviour
{
    #region Spawns region
    [SerializeField] private string currentLocation;

    [SerializeField] private Transform Spawn_1;
    [SerializeField] private Transform Spawn_2;
    [SerializeField] private Transform Spawn_3;
    #endregion
    
    #region Sniper Scope region
    [SerializeField] private Camera _camera;
    private float _defaultFOV = 60;
    private float _zoomFOV = 20;
    [SerializeField] public UnityEngine.UI.Image sniperScopeImage;
    #endregion
   
    #region bullet region
    [SerializeField] private GameObject bullet;
    private Transform bulletSpawnPoint;
    #endregion

    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    public float shootDelay = 2.0f;
    public bool canShoot = true;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    public Image imageCooldown;

    void Start()
    {
        _defaultFOV = _camera.fieldOfView;
        characterController = GetComponent<CharacterController>();
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentLocation = "Spawn_1";
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        //AudioManager.Instance.PlaySFX("Walk");

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            
        }

        //Shoot
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
        {
            StartCoroutine(Shoot());
        }

        //Zoom
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!sniperScopeImage.enabled)
            {
                sniperScopeImage.enabled = true;
                AudioManager.Instance.PlaySFX("Scope");
                StartCoroutine(SniperZoom());
            }
            else if (sniperScopeImage.enabled && _camera.fieldOfView == _zoomFOV)
            {
                sniperScopeImage.enabled = false;
                AudioManager.Instance.PlaySFX("Scope");
                StopCoroutine(SniperZoom());
                SniperUnZoom();
                //_camera.fieldOfView = _defaultFOV;
            }
        }

        if (!canShoot)
        {
            imageCooldown.fillAmount -= 1 / shootDelay * Time.deltaTime;
            if (imageCooldown.fillAmount<=0)
            {
                
                    imageCooldown.fillAmount =1 ;
            }
        }
    }

    private void FixedUpdate()
    {
        //////////////////////////////////////////// Teleport For Nin ////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentLocation == "Spawn_1" && Spawn_2 != null)
            {
                currentLocation = "Spawn_2";
                transform.position = Spawn_2.position;
            }
            else if (currentLocation == "Spawn_1" && Spawn_3 != null)
            {
                currentLocation = "Spawn_3";
                transform.position = Spawn_3.position;
            }

            else if (currentLocation == "Spawn_2" && Spawn_3 != null)
            {
                currentLocation = "Spawn_3";
                transform.position = Spawn_3.position;
            }
            else if (currentLocation == "Spawn_2" && Spawn_1 != null)
            {
                currentLocation = "Spawn_1";
                transform.position = Spawn_1.position;
            }

            else if (currentLocation == "Spawn_3" && Spawn_1 != null)
            {
                currentLocation = "Spawn_1";
                transform.position = Spawn_1.position;
            }
            else if (currentLocation == "Spawn_3" && Spawn_2 != null)
            {
                currentLocation = "Spawn_2";
                transform.position = Spawn_2.position;
            }
        }
    }

    public IEnumerator Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
        {
            bulletSpawnPoint = GameObject.Find("Bullet Spawn Point").transform;
            Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            AudioManager.Instance.PlaySFX("Shoot");
            canShoot = false;
            yield return new WaitForSeconds(shootDelay);
            canShoot = true;
        }
    }

    public IEnumerator SniperZoom()
    {
        while (_camera.fieldOfView > _zoomFOV)
        {
            yield return new WaitForSeconds(0.01f);
            _camera.fieldOfView -= 1f;
        }
    }

    public void SniperUnZoom()
    {
        _camera.fieldOfView = _defaultFOV;
    }
}