using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    public Animator animator;
    private PlayerManager playerManager;
    private CharacterMovement characterMovement;

    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    public readonly int IsInteracting = Animator.StringToHash("isInteracting");
    
    public bool canRotate;

    public void Initialize()
    {
        animator = GetComponent<Animator>();
        playerManager = GetComponentInParent<PlayerManager>();
        characterMovement = GetComponentInParent<CharacterMovement>();
    }

    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
    {
        var v = UpdateValuesHelper(verticalMovement);
        var h = UpdateValuesHelper(horizontalMovement);
        
        var delta = Time.deltaTime;
        animator.SetFloat(Vertical, v, 0.1f, delta);
        animator.SetFloat(Horizontal, h, 0.1f, delta);
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        animator.applyRootMotion = isInteracting;
        animator.SetBool(IsInteracting, isInteracting);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    private void OnAnimatorMove()
    {
        if (!playerManager.isInteracting) return;

        var delta = Time.deltaTime;
        characterMovement.rigidbody.drag = 0;
        
        var deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;

        var velocity = deltaPosition / delta;
        characterMovement.rigidbody.velocity = velocity;
    }

    public void SetCanRotate(bool tf)
    {
        canRotate = tf;
    }

    private static float UpdateValuesHelper(float movement)
    {
        float output;
        
        if (movement > 0 && movement < 0.55f)
        {
            output = 0.5f;
        }
        else if (movement > 0.55f)
        {
            output = 1;
        }
        else if (movement < 0 && movement > -0.55f)
        {
            output = -0.5f;
        }
        else if (movement < -0.55f)
        {
            output = -1;
        }
        else
        {
            output = 0;
        }
        
        return output;
    }
    
}
