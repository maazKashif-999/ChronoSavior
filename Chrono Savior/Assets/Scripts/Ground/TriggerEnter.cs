using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerEnter : MonoBehaviour
{
    [SerializeField] private int index;
    public delegate void EnterTrigger(int index);
    public static event EnterTrigger onEnter;
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            onEnter?.Invoke(index);
        }
    }
}
