using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEntity
{
    private Position _position;
    public Position position {
        get {
            return _position;
        }
        set {
            if (_position != value){
                
            }
            _position = value;
        }
    }
    public Dictionary<Type, DataComponent> components = new Dictionary<Type, DataComponent>();
    public T GetComponent<T>() where T : DataComponent{
        return (components[typeof(T)] as T);
    }

    public T AddComponent<T>() where T : DataComponent, new(){
        if (components.ContainsKey(typeof(T))){
            return components[typeof(T)] as T;
        }
        var t = new T();
        t.entity = this;
        t.Init();
        components.Add(typeof(T), t);
        return t;
    }

    public void RemoveComponent<T>() where T : DataComponent{
        if (components.ContainsKey(typeof(T))){
            components[typeof(T)].Remove();
            components.Remove(typeof(T));
        }
    }

    public bool HasComponent<T>() where T : DataComponent{
        return components.ContainsKey(typeof(T));
    }
}
