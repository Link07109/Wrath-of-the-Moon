using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Animator animator;
    private InputHandler inputHandler;
    private CharacterMovement characterMovement;
    private CameraHandler cameraHandler;

    private static readonly int IsInteracting = Animator.StringToHash("isInteracting");
    
    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;
    // public bool isRolling;
    // public bool isAttacking;

    private void Start()
    {
        cameraHandler = CameraHandler.Singleton;
        animator = GetComponentInChildren<Animator>();
        inputHandler = GetComponent<InputHandler>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        var delta = Time.deltaTime;
        
        isInteracting = animator.GetBool(IsInteracting);
        
        characterMovement.HandleMovement(delta);
        characterMovement.HandleRollAndSprint(delta);
        characterMovement.HandleFalling(delta);
    }

    private void FixedUpdate()
    {
        var delta = Time.fixedDeltaTime;

        if (!cameraHandler)
        {
            Debug.LogError("Camera Handler is null!");
            return;
        }
        cameraHandler.FollowTarget(delta);
        cameraHandler.HandleCameraRotation(delta, inputHandler.cameraX, inputHandler.cameraY);
    }
    
    private void LateUpdate()
    {
        var delta = Time.deltaTime;
        
        if (isInAir)
        {
            characterMovement.inAirTimer += delta;
        }
    }
    
}
