using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
   public enum State
   {
      Idle,
      Run,
      Finished
   }

   public State state;
   public float duration = 1.0f;
   private float elapsedTime = 0.0f;

   public void Tick()
   {
      switch (state)
      {
         case State.Idle:
            break;
         case State.Run:
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= duration)
            {
               state = State.Finished;
            }
            break;
         case State.Finished:
            break;
         default:
            Debug.Log("Error");
            break;
      }
   }
   
   // 执行
   public void Go()
   {
      elapsedTime = 0;
      state = State.Run;
   }

}
