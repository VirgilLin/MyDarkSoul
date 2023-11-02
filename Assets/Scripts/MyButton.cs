using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    public bool isPressing = false;
    public bool onPressed = false;
    public bool onReleased = false;
    public bool isExtending = false;
    

    private bool curState = false;
    private bool lastState = false;
    private bool isDelaying = false;

    private  MyTimer extTimer = new MyTimer();
    public void Tick(bool input)
    {
        extTimer.Tick();
        
        curState = input;
        isPressing = curState;
        
        onPressed = false;
        onReleased = false;
        if (curState != lastState)
        {
            if (curState == true)
            {
                onPressed = true;
            }
            else
            {
                onReleased = true;
                // 连续点击判断
                StartTimer(extTimer, 1.0f);
            }
        }
        lastState = curState;
        isExtending = extTimer.state == MyTimer.State.Run;
    }

    private void StartTimer(MyTimer timer,float duration)
    {
        timer.duration = duration;
        timer.Go();
    }
}
