using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITask
{
    void OnStart(TickableMonoBehaviour tickable);
    void Tick(TickableMonoBehaviour tickable);
    void PostTick(TickableMonoBehaviour tickable);
    void OnEnd(TickableMonoBehaviour tickable);
    void Cancel(TickableMonoBehaviour tickable);
}
