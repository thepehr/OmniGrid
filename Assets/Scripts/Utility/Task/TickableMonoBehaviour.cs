using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TickableMonoBehaviour : SerializedMonoBehaviour
{
    public Position position;
    public Position prePosition;
    [HideInInspector]
    public ITask task;
    public List<ITask> tasksStack = new List<ITask>();

    public void Push(ITask task)
    {
        tasksStack.Insert(0, task);
    }
    public ITask Top()
    {
        return tasksStack.Count > 0 ? tasksStack[0] : null;
    }
    public ITask Pop()
    {
        var res = tasksStack.Count > 0 ? tasksStack[0] : null;
        if (res != null)
            tasksStack.RemoveAt(0);
        return res;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void PostTick()
    {
        if (task != null)
            task.PostTick(this);
        
    }

    public virtual void Tick(){
        prePosition = position;
        if (task != null)
            task.Tick(this);

    }

    public void SetTask(ITask task)
    {
        if (this.task != null)
            this.task.OnEnd(this);
        this.task = task;
        if (this.task != null)
            this.task.OnStart(this);
    }
}
