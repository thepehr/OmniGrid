using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameAssetCollection : SerializedMonoBehaviour
{
    #region singleton
	private static GameAssetCollection _instance;
	public static GameAssetCollection Instance { get { return _instance ? _instance : (_instance = FindObjectOfType<GameAssetCollection>()); } }
	#endregion

    public Dictionary<string, GameObject> assets = new Dictionary<string, GameObject>();
    public GameObject this[string name]{
        get{
            if (assets.ContainsKey(name))
                return assets[name];
            else
                return null;
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
}
