using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private Animator animator;
    private Player player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = Player.Instance;
    }
    private void Update()
    {
        if(player != null && animator != null) 
        {
            animator.SetBool(IS_WALKING, player.IsWalking());
        }
        
    }

}
