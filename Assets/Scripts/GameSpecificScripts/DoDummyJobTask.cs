using System.Collections;
using System.Collections.Generic;
using LGrid;
using UnityEngine;

public class DoDummyJobTask : ITask
{
    int counter;
    ITask nextJob;
    public DoDummyJobTask(int counter, ITask nextJob)
    {
        this.counter = counter;
        this.nextJob = nextJob;
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
        Debug.Log("Job");
        if (--counter <= 0)
        {
            GridManager.RemoveComponent<JobComponent>(tickable.position);
            GridManager.Instance.RemoveTag(tickable.position, "job");
            var gc = GridManager.GetComponent<GameObjectComponent>(tickable.position);
            var g = gc.gameObject;
            GridManager.RemoveComponent<GameObjectComponent>(tickable.position);
            GameObject.Destroy(g);
            tickable.SetTask(nextJob);
        }
    }
}
