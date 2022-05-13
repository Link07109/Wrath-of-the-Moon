using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput playerInput;

    private Vector2 movementInput;
    private Vector2 cameraInput;

    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float cameraX;
    public float cameraY;

    public bool isRollPressed;
    // private bool isJumpPressed;

    private void Awake()
    {
        playerInput = new PlayerInput();

        // Input Callbacks
        playerInput.PlayerMovement.Camera.performed += OnCameraInput;
        
        playerInput.PlayerMovement.Move.started += OnMovementInput;
        playerInput.PlayerMovement.Move.canceled += OnMovementInput;
        playerInput.PlayerMovement.Move.performed += OnMovementInput;

        playerInput.PlayerActions.Roll.started += OnRoll;
        playerInput.PlayerActions.Roll.canceled += OnRoll;
        
        // playerInput.PlayerActions.Jump.started += OnJump;
        // playerInput.PlayerActions.Jump.canceled += OnJump;
    }
    
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
    }
    
    private void OnCameraInput(InputAction.CallbackContext context)
    {
        cameraInput = context.ReadValue<Vector2>();
        cameraX = cameraInput.x;
        cameraY = cameraInput.y;
    }

    private void OnRoll(InputAction.CallbackContext context)
    {
        isRollPressed = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        playerInput.PlayerMovement.Enable();
        playerInput.PlayerActions.Enable();
    }
    
    private void OnDisable()
    {
        playerInput.PlayerMovement.Disable();
        playerInput.PlayerActions.Disable();
    }

}

