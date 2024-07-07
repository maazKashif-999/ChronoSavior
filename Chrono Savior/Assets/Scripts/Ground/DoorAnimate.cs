using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimate : MonoBehaviour
{
    private AudioSource myaudio;
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private AudioClip doorCloseSound;

    private Animator animator;
    private const string IS_OPEN = "IsOpen";
    private void Awake()
    {
        animator = GetComponent<Animator>();
        myaudio = GetComponent<AudioSource>();
    }
    public void OpenDoor()
    {
        if(animator != null)
        {
            animator.SetBool(IS_OPEN,true);
        }
        else
        {
            Debug.LogError("Animator is null");
        }

        if(myaudio!=null && doorOpenSound!=null)
        {
            myaudio.PlayOneShot(doorOpenSound);
        }

        else
        {
            Debug.Log("door opening sound not found");
        }
    }

    public void CloseDoor()
    {
        if(animator != null)
        {
            animator.SetBool(IS_OPEN,false);
        }
        else
        {
            Debug.LogError("Animator is null");
        }

        if (myaudio != null && doorCloseSound != null)
        {
            myaudio.PlayOneShot(doorCloseSound);
        }

        else
        {
            Debug.Log("door closing sound not found");
        }
    }
}
