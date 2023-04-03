using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBehaviour : TickableMonoBehaviour
{
    public static int nextID;
    public int id;
    public HouseBehaviour house;
    public AnimationCurve curve;
    // Start is called before the first frame update
    void Start()
    {
        position = Position.GetPosition(transform.position);
        SetTask(new JobFindingTask());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(prePosition.GetWorldPosition(), position.GetWorldPosition(), curve.Evaluate(GameManager.Instance.Progress));
        //if (Input.GetMouseButtonDown(0))
        //{
        //    SetTask(new PathFollowingTask(Position.mouse, null));
        //}
    }

    private void Awake() {
        id = nextID++;
        GetComponent<GuidedSearch>().wildCards.Add("id"+id);
        GetComponent<GuidedTagFinder>().wildcards.Add("id"+id);
    }
}
