using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    private Animator anim;

    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    public void ResetTrigger(string triggerName)
    {
        anim.ResetTrigger(triggerName);
    }
}
