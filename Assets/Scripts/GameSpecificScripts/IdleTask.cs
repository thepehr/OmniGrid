using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTask : ITask
{

    public int ticks;
    public ITask next;
    public IdleTask(int ticks, ITask next)
    {
        this.ticks = ticks;
        this.next = next;
    }
    public void Cancel(TickableMonoBehaviour tickable)
    {
        
    }

    public void OnEnd(TickableMonoBehaviour tickable)
    {
        
    }

    public void OnStart(TickableMonoBehaviour tickable)
    {
        
    }

    public void PostTick(TickableMonoBehaviour tickable)
    {
        
    }

    public void Tick(TickableMonoBehaviour tickable)
    {
        if (--ticks < 0)
        {
            tickable.SetTask(next);
        }
    }
}
