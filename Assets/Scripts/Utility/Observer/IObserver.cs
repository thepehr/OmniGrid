using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    void Register(IObservable observable);
    void Unregister(IObservable observable);
    void Notify(IObservable observable);
}
