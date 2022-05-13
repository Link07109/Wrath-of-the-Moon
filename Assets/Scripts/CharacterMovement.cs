using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private InputHandler inputHandler;
    private AnimatorHandler animatorHandler;
    private PlayerManager playerManager;
    
    private PlayerAttacker playerAttacker;
    private Inventory inventory;

    private Transform cameraObject;
    public GameObject normalCamera;
    [HideInInspector] public Transform myTransform;

    private Vector3 moveDirection;
    public new Rigidbody rigidbody;

    [Header("Ground & Air Vars")]
    [SerializeField] private float groundDetectionRayStart = 0.35f;
    [SerializeField] private float minimumDistanceNeededToFall = 1.0f;
    [SerializeField] private float groundDirectionRayDistance = 0.02f;
    private LayerMask ignoreForGroundCheck;
    public float inAirTimer;
    
    [Header("Movement Vars")]
    [SerializeField] private float movementSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 5.0f;
    [SerializeField] private float rotationSpeed = 7.0f;
    [SerializeField] private float fallSpeed = 60.0f;
    
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        animatorHandler = GetComponentInChildren<AnimatorHandler>();
        playerManager = GetComponent<PlayerManager>();
        
        playerAttacker = GetComponent<PlayerAttacker>();
        inventory = GetComponent<Inventory>();
        
        cameraObject = Camera.main.transform;
        myTransform = transform;
        
        animatorHandler.Initialize();

        playerManager.isGrounded = true;
        ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
    }

    private Vector3 normalVector;
    private Vector3 targetPosition;

    public void HandleAttacking()
    {
        if (inputHandler.isRBPressed)
            playerAttacker.HandleLightAttack(inventory.weapon);
        else if (inputHandler.isRTPressed)
            playerAttacker.HandleHeavyAttack(inventory.weapon);
    }
    
    private void HandleRotation(float delta)
    {
        var moveOverride = inputHandler.moveAmount;
        
        var targetDirection = cameraObject.forward * inputHandler.vertical;
        targetDirection += cameraObject.right * inputHandler.horizontal;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero) targetDirection = myTransform.forward;

        var rs = rotationSpeed;

        var tr = Quaternion.LookRotation(targetDirection);
        var targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

        myTransform.rotation = targetRotation;
    }
    
    public void HandleMovement(float delta)
    {
        if (playerManager.isInteracting) return;
        
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;

        var speed = movementSpeed;
        moveDirection *= speed;

        var projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0.0f);
        
        if (animatorHandler.canRotate)
        {
            HandleRotation(delta);
        }
    }

    public void HandleRollAndSprint(string animString, bool inputRoll)
    {
        if (animatorHandler.animator.GetBool(animatorHandler.IsInteracting)) return;

        var comparison = inputRoll ? inputHandler.isRollPressed : inputHandler.isSlidePressed;
        if (!comparison) return;
        
        moveDirection = cameraObject.forward * inputHandler.vertical;
        moveDirection += cameraObject.right * inputHandler.horizontal;

        if (!(inputHandler.moveAmount > 0)) return;
        
        animatorHandler.PlayTargetAnimation(animString, true);
        moveDirection.y = 0;
        var rollRotation = Quaternion.LookRotation(moveDirection);
        myTransform.rotation = rollRotation;
    }

    public void HandleFalling(float delta)
    {
        playerManager.isGrounded = false;
        var origin = myTransform.position;
        origin.y += groundDetectionRayStart;

        // if (Physics.Raycast(origin, myTransform.forward, out var hit, 0.4f))
        // {
        //     moveDirection = Vector3.zero;
        // }

        if (playerManager.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallSpeed);
            rigidbody.AddForce(moveDirection * fallSpeed / 20.0f);
        }

        // var dir = moveDirection;
        // dir.Normalize();
        // origin += dir * groundDirectionRayDistance;

        targetPosition = myTransform.position;
        
        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToFall, Color.red, 0.1f, false);
        
        if (Physics.Raycast(origin, -Vector3.up, out var hit, minimumDistanceNeededToFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            var tp = hit.point;
            playerManager.isGrounded = true;
            targetPosition.y = tp.y;

            if (playerManager.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    animatorHandler.PlayTargetAnimation("Land", true);
                    inAirTimer = 0.0f;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Movement", false);
                    inAirTimer = 0.0f;
                }

                playerManager.isInAir = false;
            }
        }
        else
        {
            if (playerManager.isGrounded)
            {
                playerManager.isGrounded = false;
            }

            if (!playerManager.isInAir)
            {
                if (!playerManager.isInteracting)
                {
                    animatorHandler.PlayTargetAnimation("Falling", true);
                }

                // var vel = rigidbody.velocity;
                // vel.Normalize();
                // rigidbody.velocity = vel * (movementSpeed / 2.0f);
                playerManager.isInAir = true;
            }
        }

        if (!playerManager.isGrounded) return;
        
        if (playerManager.isInteracting || inputHandler.moveAmount > 0.0f)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, delta);
        }
        else
        {
            myTransform.position = targetPosition;
        }

    }
    
}