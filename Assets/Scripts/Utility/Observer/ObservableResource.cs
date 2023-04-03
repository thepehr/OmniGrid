using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableResource : SerializedMonoBehaviour, IObservable
{
    private HashSet<IObserver> observers = new HashSet<IObserver>();
    public string resourceName;
    [SerializeField, HideInInspector]
    private int _value;
    [ShowInInspector]
    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            if (!_value.Equals(value))
            {
                _value = value;
                Notify();
            }
        }
    }

    public void Notify()
    {
        foreach (var item in observers)
        {
            item.Notify(this);
        }
    }

    public void Register(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Unregister(IObserver observer)
    {
        observers.Remove(observer);
    }

    private void Start()
    {
        Notify();
    }
}
