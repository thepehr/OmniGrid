using System.Collections;
using System.Collections.Generic;
using LGrid;
using Sirenix.OdinInspector;
using UnityEngine;

public class DummyJobBehaviour : SerializedMonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake() {
        var c = GridManager.AddComponent<JobComponent>(Position.GetPosition(transform.position));
        c.job = new DoDummyJobTask(5, new GoingHomeTask());
        c.name = "JOB";
        var g = GridManager.AddComponent<GameObjectComponent>(Position.GetPosition(transform.position));
        g.gameObject = gameObject;
    }
}
