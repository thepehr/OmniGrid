using System.Collections;
using System.Collections.Generic;
using LGrid;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton
	private static GameManager _instance;
	public static GameManager Instance { get { return _instance ? _instance : (_instance = FindObjectOfType<GameManager>()); } }
	#endregion

    public int actionPoint = 5;
    public int currentActionPoint = 5;

    public float delay;
    private float _startTime;
    public bool autorun;


    public float Progress {
        get {
            return (Time.time - _startTime)/delay;
        }
    }


    public void Tick()
    {
        foreach (var item in FindObjectsOfType<TickableMonoBehaviour>())
        {
            item.Tick();
        }
        foreach (var item in FindObjectsOfType<TickableMonoBehaviour>())
        {
            item.PostTick();
        }
        currentActionPoint = actionPoint;
    }

    public void CastObject(Position center, string name, float depth)
    {
        var open = new PriorityQueue<Position>();
        var closed = new HashSet<Position>();
        var depths = new Dictionary<Position, float>();
        open.Enqueue(center, 0);
        depths.Add(center, 0);
        Instantiate(GameAssetCollection.Instance[name], center.GetWorldPosition(), Quaternion.identity);
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
                    if (!GridManager.Instance.HasTag(item, "revealed"))
                        Instantiate(GameAssetCollection.Instance[name], item.GetWorldPosition(), Quaternion.identity);
                }
                else
                {
                    
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.time - _startTime > delay && autorun){
            _startTime = Time.time;
            foreach (var item in FindObjectsOfType<TickableMonoBehaviour>())
            {
                item.Tick();
            }
            foreach (var item in FindObjectsOfType<TickableMonoBehaviour>())
            {
                item.PostTick();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var item in FindObjectsOfType<TickableMonoBehaviour>())
            {
                item.Tick();
            }
            foreach (var item in FindObjectsOfType<TickableMonoBehaviour>())
            {
                item.PostTick();
            }
        }
        if (Input.GetMouseButton(0))
        {

            if (Input.GetKey(KeyCode.P))
            {   
                if (!GridManager.Instance.HasTag(Position.mouse, "paving"))
                    Instantiate(GameAssetCollection.Instance["paving"], Position.mouse.GetWorldPosition(), Quaternion.identity);
            }
            if (Input.GetKey(KeyCode.F))
            {
                if (!GridManager.Instance.HasTag(Position.mouse, "job"))
                    Instantiate(GameAssetCollection.Instance["farm"], Position.mouse.GetWorldPosition(), Quaternion.identity);
            }
            if (Input.GetKey(KeyCode.R))
            {
                if (!GridManager.Instance.HasTag(Position.mouse, "job"))
                    Instantiate(GameAssetCollection.Instance["rock"], Position.mouse.GetWorldPosition(), Quaternion.identity);
            }
            if (Input.GetKey(KeyCode.G))
            {
                if (!GridManager.Instance.HasTag(Position.mouse, "job"))
                    Instantiate(GameAssetCollection.Instance["gold"], Position.mouse.GetWorldPosition(), Quaternion.identity);
            }
            if (Input.GetKey(KeyCode.T))
            {
                if (!GridManager.Instance.HasTag(Position.mouse, "job"))
                    Instantiate(GameAssetCollection.Instance["tree"], Position.mouse.GetWorldPosition(), Quaternion.identity);
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                CastObject(Position.mouse, "ground", 2.5f);
                CurtainBehaviour.Cast(Position.mouse, 2.5f, new HashSet<string>(){"revealed"}, new HashSet<string>());
            }
        }
    }
}
