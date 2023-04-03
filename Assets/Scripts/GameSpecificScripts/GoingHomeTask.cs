using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoingHomeTask : ITask
{
    public void Cancel(TickableMonoBehaviour tickable)
    {
        
    }

    public void OnEnd(TickableMonoBehaviour tickable)
    {
        
    }

    public void OnStart(TickableMonoBehaviour tickable)
    {
        //Debug.Log("Going home!");
        var worker = (WorkerBehaviour)tickable;
        tickable.SetTask(new PathFollowingTask(Position.GetPosition(worker.house.transform.position), new IdleTask(5, new JobFindingTask())));
    }   

    public void PostTick(TickableMonoBehaviour tickable)
    {
        
    }

    public void Tick(TickableMonoBehaviour tickable)
    {
        
    }
}
