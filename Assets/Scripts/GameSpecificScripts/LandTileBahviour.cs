using System.Collections;
using System.Collections.Generic;
using LGrid;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;

public class LandTileBahviour : SerializedMonoBehaviour
{
    public Dictionary<string, Sprite> sprites;

    public bool isMain;
    

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Refresh();
        }
    }

    private void Awake() {
        if (isMain)
            GridManager.Instance.AddTag(Position.GetPosition(transform.position), "land");
    }
}
