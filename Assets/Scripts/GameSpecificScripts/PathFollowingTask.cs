using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowingTask : ITask
{
    private WorkerBehaviour worker;
    private Position dest;
    private ITask next;
    public PathFollowingTask(Position dest, ITask next)
    {
        this.dest = dest;
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
        //Debug.Log("Path Finding");
        worker = (WorkerBehaviour)tickable;
        var search = worker.GetComponent<GuidedSearch>();
        if (!search.profile.Check(dest))
            worker.SetTask(null);
        else
        {
            search.dest = dest;
            search.start = worker.position;
            search.Find();
        }
        //Debug.Log("Following " + worker.id);
    }

    public void PostTick(TickableMonoBehaviour tickable)
    {
        
    }

    public void Tick(TickableMonoBehaviour tickable)
    {   
        var pos = worker.position;
        var search = worker.GetComponent<GuidedSearch>();
        if (pos == search.dest)
        {
            //Debug.Log("Done " + worker.id);
            worker.SetTask(next);
            return;
        }
        if (search.path != null && search.path.ContainsKey(pos))
            worker.position = worker.GetComponent<GuidedSearch>().path[worker.position];
        else
        {
            Debug.Log("BUG");
            worker.SetTask(null);
        }
            
    }
}
