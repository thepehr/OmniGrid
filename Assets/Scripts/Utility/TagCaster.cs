using System.Collections;
using System.Collections.Generic;
using LGrid;
using Sirenix.OdinInspector;
using UnityEngine;

public class TagCaster : SerializedMonoBehaviour
{

    public HashSet<string> tags;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake() {
        foreach (var item in tags)
        {
            GridManager.Instance.AddTag(Position.GetPosition(transform.position), item);
        }
        
    }

    private void OnMouseDown() {
        foreach (var item in tags)
        {
            GridManager.Instance.RemoveTag(Position.GetPosition(transform.position), item);
        }
        Destroy(gameObject);
    }
}
