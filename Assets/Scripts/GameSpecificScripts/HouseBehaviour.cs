using System.Collections;
using System.Collections.Generic;
using LGrid;
using Sirenix.OdinInspector;
using UnityEngine;

public class HouseBehaviour : SerializedMonoBehaviour
{
    public WorkerBehaviour worker;

    private void Start() {
        GridManager.Instance.AddTag(Position.GetPosition(transform.position), "id" + worker.id.ToString());
    }
}
