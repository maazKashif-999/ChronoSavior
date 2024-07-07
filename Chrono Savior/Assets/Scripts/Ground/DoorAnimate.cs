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
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on DoorAnimate GameObject.");
        }

        myaudio = GetComponent<AudioSource>();
        if (myaudio == null)
        {
            Debug.LogError("AudioSource component is missing on DoorAnimate GameObject.");
        }
    }

    public void OpenDoor()
    {
        if (animator != null)
        {
            animator.SetBool(IS_OPEN, true);
        }
        else
        {
            Debug.LogError("Animator is null in OpenDoor method.");
        }

        if (myaudio != null && doorOpenSound != null)
        {
            myaudio.PlayOneShot(doorOpenSound);
        }
        else if (myaudio == null)
        {
            Debug.LogError("AudioSource is null in OpenDoor method.");
        }
        else if (doorOpenSound == null)
        {
            Debug.LogWarning("Door open sound not assigned in DoorAnimate.");
        }
    }

    public void CloseDoor()
    {
        if (animator != null)
        {
            animator.SetBool(IS_OPEN, false);
        }
        else
        {
            Debug.LogError("Animator is null in CloseDoor method.");
        }

        if (myaudio != null && doorCloseSound != null)
        {
            myaudio.PlayOneShot(doorCloseSound);
        }
        else if (myaudio == null)
        {
            Debug.LogError("AudioSource is null in CloseDoor method.");
        }
        else if (doorCloseSound == null)
        {
            Debug.LogWarning("Door close sound not assigned in DoorAnimate.");
        }
    }
}
