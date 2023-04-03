using System.Collections;
using System.Collections.Generic;
using LGrid;
using UnityEngine;

public class JobFindingTask : ITask
{
    public void Cancel(TickableMonoBehaviour tickable)
    {
        
    }

    public void OnEnd(TickableMonoBehaviour tickable)
    {
        
    }

    public void OnStart(TickableMonoBehaviour tickable)
    {
        var worker = (WorkerBehaviour)tickable;
        var finder = worker.GetComponent<GuidedTagFinder>();
        finder.origin = worker.position;
        finder.targetTag = "job";
        var res = finder.Find();
        var jObject = GridManager.GetComponent<JobComponent>(res);
        if (jObject == null)
        {
            tickable.SetTask(new IdleTask(5, new JobFindingTask()));
            return;
        }
        var job = jObject.job;
        GridManager.Instance.RemoveTag(res, "job");
        //Debug.Log(GridManager.GetComponent<JobComponent>(res).name);
        tickable.SetTask(new PathFollowingTask(res, job));
    }

    public void PostTick(TickableMonoBehaviour tickable)
    {
        
    }

    public void Tick(TickableMonoBehaviour tickable)
    {
        
    }
}
