using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private Animator animator;
    private Player player;
    private AudioSource MyAudio;


    private void Start()

    {
        animator = GetComponent<Animator>();
        player = Player.Instance;
        MyAudio = GetComponent<AudioSource>();

        if (MyAudio == null)
        {
            Debug.LogError("No AudioSource found on the player object.");
        }
    }
    private void Update()
    {
        if(player != null && animator != null) 
        {
            bool isWalking = player.IsWalking();
            animator.SetBool(IS_WALKING, player.IsWalking());
            if (isWalking && !MyAudio.isPlaying)
            {
                MyAudio.Play();
            }
            else if(!isWalking && MyAudio.isPlaying)
            {
                MyAudio.Stop();
            }
        }
        


    }

}
