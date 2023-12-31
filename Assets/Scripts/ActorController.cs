using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public CameraController camCon;
    public IUserInput pi;
    public float walkSpeed = 2.4f;
    public float runMultiplier = 2.5f;
    public float jumpVelocity = 3.0f;
    public float rollVelocity = 3.0f;

    [Header("=======Friction Setting======")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;
    
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    private Vector3 thrustVec;
    private bool canAttack;

    private bool lockPlanar = false;
    private bool trackDirection = false;
    private CapsuleCollider col;
    private float lerpTarget;
    private Vector3 deltaPos;

    // Start is called before the first frame update
    void Awake()
    {
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if (input.enabled == true)
            {
                pi = input;
                break;
            }
        }
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        //col.material
    }

    // Update is called once per frame
    void Update()
    {
        if (pi.lockOn)
        {
            camCon.LockUnlock();
        }

        if (camCon.lockState == false)
        {
            anim.SetFloat("forward",pi.Dmag* Mathf.Lerp(anim.GetFloat("forward"),((pi.run)?2.0f:1.0f),0.5f));
            anim.SetFloat("right",0);
        }
        else
        {
            Vector3 localDVec = transform.InverseTransformVector(pi.Dvec);
            anim.SetFloat("forward",localDVec.z * ((pi.run)?2.0f:1.0f));
            anim.SetFloat("right",localDVec.x * ((pi.run)?2.0f:1.0f));
        }
        
        // 防守
        anim.SetBool("defense",pi.defense);
        // 前滚
        // if (rigid.velocity.magnitude > 0.0f)
        // {
        //     anim.SetTrigger("roll");
        // }
        if (pi.roll || rigid.velocity.magnitude > 7f)
        {
            anim.SetTrigger("roll");
            canAttack = false;
        }

        if (pi.jump)
        {
            anim.SetTrigger("jump");
            canAttack = false;
        }
        if (pi.attack && CheckState("ground") && canAttack)
        {
            anim.SetTrigger("attack");
        }

        if (camCon.lockState == false)
        {
            if (pi.Dmag > 0.1f)
            {
                model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.3f);
            }

            // 当跳跃动作触发时，
            if (lockPlanar == false)
            {
                planarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run)?runMultiplier:1.0f);
            }
        }
        else
        {
            if (trackDirection == false)
            {
                model.transform.forward = transform.forward;
            }
            else
            {
                model.transform.forward = planarVec.normalized;
            }
            if (lockPlanar == false)
            {
                planarVec = pi.Dvec * walkSpeed * ((pi.run)?runMultiplier:1.0f);   
            }
        }
        
    }

    private void FixedUpdate()
    {
        rigid.position += deltaPos;
        // rigid.position += planarVec * Time.fixedDeltaTime;
        rigid.velocity = new Vector3(planarVec.x,rigid.velocity.y,planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    }

    /// <summary>
    /// Message processing block
    /// </summary>

    private bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }

    public void OnJumpEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0,jumpVelocity,0);
        trackDirection = true;
    }
    
    public void OnJumpExit()
    {
        // pi.inputEnabled = true;
        // lockPlanar = false;
    }

    public void IsGround()
    {
        //print("is ground");
        anim.SetBool("isGround",true);
    }
    
    public void IsNotGround()
    {
        //print("is not ground");
        anim.SetBool("isGround",false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
        canAttack = true;
        col.material = frictionOne;
        trackDirection = false;
    }
    
    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }
    
    public void OnRollEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0,rollVelocity,0);
        trackDirection = true;
    }
    
    public void OnJabEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }
    
    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }

    public void OnAttack1hAEnter()
    {
        pi.inputEnabled = false;
        //lockPlanar = true;
        lerpTarget = 1.0f;
    }

    public void OnAttack1hAUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity");
        float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.3f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"),currentWeight);
    }

    public void OnAttackIdleEnter()
    {
        pi.inputEnabled = true;
        //lockPlanar = false;
        //anim.SetLayerWeight(anim.GetLayerIndex("Attack"),0);
        lerpTarget = 0f;
    }

    public void OnAttackIdleUpdate()
    {
        float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("Attack"));
        currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.3f);
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"),currentWeight);
    }

    public void OnUpdateRM(object _deltaPos)
    {
        //print(_deltaPos);
        if (CheckState("attack1C", "Attack"))
        {
            print(_deltaPos);
            deltaPos += (0.2f * deltaPos + 0.8f * (Vector3)_deltaPos);
        }

        
    }
}
