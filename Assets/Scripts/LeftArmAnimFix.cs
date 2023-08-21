using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour
{
    private Animator anim;

    public Vector3 a;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        leftLowerArm.localEulerAngles += a;
        anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm,Quaternion.Euler(leftLowerArm.localEulerAngles));
    }
}
