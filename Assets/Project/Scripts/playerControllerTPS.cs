
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UniRx;
using UniRx.Triggers;
using UniRx.Operators;
using System;
[RequireComponent(typeof(CharacterController))]

public class playerControllerTPS : MonoBehaviourPun
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 60.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    public Text playerName;

    [HideInInspector]
    public bool canMove = true;
    [SerializeField] Animator controller;
    public ReactiveProperty<bool> grounded = new ReactiveProperty<bool>();
    float rotationSpeed = 5f;
    void Start()
    {
        playerCameraParent = transform.GetChild(0);
        if (photonView.IsMine)
        {
            PlatformManager.control.player = transform;
            PlatformManager.control.Init();
            playerCameraParent.gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);

        }
        else
        {
            playerCameraParent.gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);

        }

        controller = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
        observeGrounded();
    }
    void observeGrounded()
    {
        grounded
            .Do(_ => controller.SetBool("Grounded", _))
            .Subscribe()
            .AddTo(this);
    }
    void Update()
    {
        if (!photonView.IsMine)
            return;

        grounded.Value = characterController.isGrounded;
        if (photonView != null)
        {
            if (!photonView.IsMine)
                return;
        }
        float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
        controller.SetFloat("SpeedX", curSpeedY, 1, 1);
        controller.SetFloat("SpeedY", curSpeedX, 1, 1);
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

          
            controller.SetFloat("MotionSpeed", 1f, 1, 1);

            if (Input.GetButton("Jump") && canMove)
            {
                moveDirection.y = jumpSpeed;
                controller.SetTrigger("Jumped");

            }

        }
       

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }
    }
    public void setRoation()
    {
        //Set a speed for the rotation
        
        //Get the position of the player.
        Vector3 currentPosition = transform.position;
        //Create a vector for the inputs (Which components you use depends on the game)
        Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        //Adding these vectors together will result in a position in the world, that is around your player.
        inputVector += currentPosition;

        /*Example:
        * The players position is at 0,0,0.
        * Input is 1,0,0.
        * The position in space would be 0,0,0 + 1,0,0 = 1,0,0.
        * This would be to the right of the player.
        */

        //Now we create a target rotation, by creating a direction vector: (This would be just be inputVector in this case).
        Quaternion targetRotation = Quaternion.LookRotation(inputVector - currentPosition);

        //Rotate smoothly to this target:
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
    }
  
}
