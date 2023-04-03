using System.Collections;
using System.Collections.Generic;
using LGrid;
using Sirenix.OdinInspector;
using UnityEngine;

public class CurtainBehaviour : SerializedMonoBehaviour
{

    [ShowInInspector]
    public static Dictionary<Position, HashSet<CurtainBehaviour>> curtains = new Dictionary<Position, HashSet<CurtainBehaviour>>();
    private CurtainMode _mode;
    public CurtainMode Mode
    {
        get
        {
            return _mode;
        }
        set
        {
            _mode = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake() {
        var pos = Position.GetPosition(transform.position);
        if (curtains == null)
            curtains = new Dictionary<Position, HashSet<CurtainBehaviour>>();
        if (curtains.ContainsKey(pos))
            curtains[pos].Add(this);
        else
            curtains.Add(Position.GetPosition(transform.position), new HashSet<CurtainBehaviour>(){this});
        Refresh();
    }

    public void Refresh()
    {
        var pos = Position.GetPosition(transform.position);
        if (GridManager.Instance.HasTag(pos, "revealed"))
        {
            var c = GetComponent<SpriteRenderer>().color;
            c.a = 1;
            GetComponent<SpriteRenderer>().color = c;
        }
        else
        {
            var c = GetComponent<SpriteRenderer>().color;
            c.a = 0;
            GetComponent<SpriteRenderer>().color = c;
        }
        if (GridManager.Instance.HasTag(pos, "twilight"))
        {
            var c = GetComponent<SpriteRenderer>().color;
            c.a = 0.3f;
            GetComponent<SpriteRenderer>().color = c;
        }
        if (GridManager.Instance.HasTag(pos, "hidden"))
        {
            var c = GetComponent<SpriteRenderer>().color;
            c.a = 0;
            GetComponent<SpriteRenderer>().color = c;
        }
    }

    private void OnDestroy() {
        curtains.Remove(Position.GetPosition(transform.position));
    }


    public static void Cast(Position center, float depth, HashSet<string> include, HashSet<string> exclude)
    {
        var open = new PriorityQueue<Position>();
        var closed = new HashSet<Position>();
        var depths = new Dictionary<Position, float>();
        open.Enqueue(center, 0);
        depths.Add(center, 0);
        GridManager.Instance.AddTags(center, include);
        GridManager.Instance.RemoveTags(center, exclude);
        if (curtains.ContainsKey(center))
        {
            foreach (var item2 in curtains[center])
            {
                item2.Refresh();
            }
        }
        while (open.Count > 0)
        {
            var next = open.Dequeue();
            foreach (var item in next.GetAllNeighbors())
            {
                if (depths.ContainsKey(item))
                    continue;
                var d = depths[next] + (next - item).GetWorldPosition().magnitude;
                if (d <= depth)
                {
                    open.Enqueue(item, d);
                    depths.Add(item, d);
                    GridManager.Instance.AddTags(item, include);
                    GridManager.Instance.RemoveTags(item, exclude);
                    if (curtains.ContainsKey(item))
                    {
                        foreach (var item2 in curtains[item])
                        {
                            item2.Refresh();
                        }
                        
                    }
                }
            }
        }
    }
}





public enum CurtainMode
{
    Revealed,
    Hidden,
    Twilight,
}