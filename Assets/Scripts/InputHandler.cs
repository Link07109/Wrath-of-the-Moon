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
    public bool isSlidePressed;
    // private bool isJumpPressed;
    public bool isRBPressed;
    public bool isRTPressed;

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
        
        playerInput.PlayerActions.Slide.started += OnSlide;
        playerInput.PlayerActions.Slide.canceled += OnSlide;
        
        // playerInput.PlayerActions.Jump.started += OnJump;
        // playerInput.PlayerActions.Jump.canceled += OnJump;

        playerInput.PlayerActions.RB.started += OnRBInput;
        playerInput.PlayerActions.RB.canceled += OnRBInput;
        
        playerInput.PlayerActions.RT.started += OnRTInput;
        playerInput.PlayerActions.RT.canceled += OnRTInput;
    }

    private void OnRBInput(InputAction.CallbackContext context)
    {
        isRBPressed = context.ReadValueAsButton();
    }
    
    private void OnRTInput(InputAction.CallbackContext context)
    {
        isRTPressed = context.ReadValueAsButton();
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
    
    private void OnSlide(InputAction.CallbackContext context)
    {
        isSlidePressed = context.ReadValueAsButton();
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

