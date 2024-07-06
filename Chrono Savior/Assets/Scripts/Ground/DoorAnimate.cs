using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimate : MonoBehaviour
{
    private Animator animator;
    private const string IS_OPEN = "IsOpen";
    private void Awake()
    {
        animator = GetComponent<Animator>();
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
    }
}
