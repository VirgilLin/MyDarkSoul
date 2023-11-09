using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton
{
    public bool isPressing = false;
    public bool onPressed = false;
    public bool onReleased = false;
    public bool isExtending = false;
    public bool isDelaying = false;

    public float extendingDuration = 0.15f;
    public float delayingDuration = 0.15f;

    private bool curState = false;
    private bool lastState = false;

    private  MyTimer extTimer = new MyTimer();
    private  MyTimer delayTimer = new MyTimer();
    public void Tick(bool input)
    {
        extTimer.Tick();
        delayTimer.Tick();
        
        curState = input;
        isPressing = curState;
        
        onPressed = false;
        onReleased = false;
        if (curState != lastState)
        {
            if (curState == true)
            {
                onPressed = true;
                StartTimer(delayTimer, delayingDuration);
            }
            else
            {
                onReleased = true;
                // 连续点击判断
                StartTimer(extTimer, extendingDuration);
            }
        }
        lastState = curState;
        isExtending = extTimer.state == MyTimer.State.Run;
        isDelaying = delayTimer.state == MyTimer.State.Run;
    }

    private void StartTimer(MyTimer timer,float duration)
    {
        timer.duration = duration;
        timer.Go();
    }
}
