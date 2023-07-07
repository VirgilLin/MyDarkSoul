using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        Vector3 temp = anim.deltaPosition;
        // print(temp.x);
        SendMessageUpwards("OnUpdateRM",anim.deltaPosition);
    }
}
