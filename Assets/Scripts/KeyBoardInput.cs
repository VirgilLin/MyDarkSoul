using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardInput : IUserInput
{
    // Variable
    [Header("===== Key Setting =====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";
    public string keyLockOn = "q";
    
    
    public string keyA; // 加速
    public string keyB; // 跳跃
    public string keyC; // 攻击
    public string keyD; // 防守

    public string keyJRight;
    public string keyJLeft;
    public string keyJUp;
    public string keyJDown;

    //  抽象按钮
    public MyButton keyRun = new MyButton();
    public MyButton keyJump = new MyButton();
    public MyButton keyAttack= new MyButton();
    public MyButton keyDefense = new MyButton();
    public MyButton keyLockOnBtn = new MyButton();

    [Header("===== Output Signal =====")] 
    public bool mouseEnable = false;
    public float mouseSensitivityX = 3.0f;
    public float mouseSensitivityY = 2.5f;
    
    // [Header("===== Output Signal =====")]
    // public float Dup;
    // public float Dright;
    // public float Dmag;
    // public Vector3 Dvec;
    // public float Jup;
    // public float JRight;
    //
    // // 1.pressing signal
    // public bool run;
    // // 2.trigger once signal
    // public bool jump;
    // public bool lastJump;
    // public bool attack;
    // public bool lastAttack;
    //
    // [Header("===== Others =====")]
    // public bool inputEnabled = true;
    //
    // private float targetDup;
    // private float targetDright;
    // private float velocityDup;
    // private float velocityDright;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        keyRun.Tick(Input.GetKey(keyA));
        keyJump.Tick(Input.GetKey(keyB));
        keyAttack.Tick(Input.GetKey(keyC));
        keyDefense.Tick(Input.GetKey(keyD));
        keyLockOnBtn.Tick(Input.GetKey(keyLockOn));
        
        // 判断是否连续点击
        // print(keyAttack.isExtending && keyAttack.isPressing);
        
        
        if (mouseEnable)
        {
            // 鼠标控制视角方向
            Jup = Input.GetAxis("Mouse Y") * mouseSensitivityY;
            JRight = Input.GetAxis("Mouse X") * mouseSensitivityX;
        }
        else
        {
            // 右边遥感控制视角方向的输入信号
            Jup = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
            JRight = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);
        }
        // 左边遥感控制移动方向的输入信号
        targetDup = (Input.GetKey(keyUp)? 1.0f:0) - (Input.GetKey(keyDown)? 1.0f:0);
        targetDright = (Input.GetKey(keyRight)? 1.0f:0) - (Input.GetKey(keyLeft)? 1.0f:0);

        if (!inputEnabled)
        {
            targetDup = 0;
            targetDright = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        // 合并两个方向
        var tempDAixs = SquareToCircle(new Vector2(Dright,Dup));

        // 移动的冲量
        Dmag = Mathf.Sqrt(tempDAixs.x * tempDAixs.x + tempDAixs.y * tempDAixs.y);
        // 移动的方向
        Dvec = tempDAixs.x * transform.right + tempDAixs.y * transform.forward;
        
        // 跑动的输入信号
        // run = Input.GetKey(keyA);
        run = (keyRun.isPressing && !keyRun.isDelaying) || keyRun.isExtending;
        
        //防守的输入信号
        // defense = Input.GetKey(keyD);
        defense = keyDefense.isPressing;

        // 跳跃的输入信号
        // var newJump = Input.GetKey(keyB);
        // 防止长按跳跃键使jump一直为true
        // if (newJump != lastJump && newJump)
        // {
        //     jump = true;
        // }
        // else
        // {
        //     jump = false;
        // }
        // lastJump = newJump;
        jump = keyJump.onPressed;
        
        // 翻滚
        roll = keyRun.onReleased && keyRun.isDelaying;
        // 攻击的输入信号
        // var newAttack = Input.GetKey(keyC);
        // 防止长按攻击键使attack一直为true
        // if (newAttack != lastAttack && newAttack)
        // {
        //     attack = true;
        // }
        // else
        // {
        //     attack = false;
        // }
        // lastAttack = newAttack;
        attack = keyAttack.onPressed;
        
        // 锁定
        lockOn = keyLockOnBtn.onPressed;
    }

    // private Vector2 SquareToCircle(Vector2 input)
    // {
    //     Vector2 output = Vector2.zero;
    //     output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
    //     output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
    //     return output;
    // }
}
