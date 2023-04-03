using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedAreaRenderer : MonoBehaviour
{
    #region singleton
	private static SelectedAreaRenderer _instance;
	public static SelectedAreaRenderer Instance { get { return _instance ? _instance : (_instance = FindObjectOfType<SelectedAreaRenderer>()); } }
	#endregion
    public Image image;
    public RectTransform rect;
    public void Render(Position p1, Position p2, Color color){
        image.color = color;
        image.enabled = true;
        Position min = new Position(Mathf.Min(p1.x, p2.x), Mathf.Min(p1.y, p2.y));
        Position max = new Position(Mathf.Max(p1.x, p2.x), Mathf.Max(p1.y, p2.y));
        var pos = Camera.main.WorldToScreenPoint(min.GetWorldPosition() - new Vector3(0.5f, 0.5f, 0));
        var dest = Camera.main.WorldToScreenPoint(max.GetWorldPosition() + new Vector3(0.5f, 0.5f, 0));
        var width = Mathf.Abs(dest.x - pos.x);
        var height = Mathf.Abs(dest.y - pos.y);
        rect.sizeDelta = new Vector2(width, height);
        rect.anchoredPosition = pos + new Vector3(width/2, height/2);
    }

    public void Hide(){
        image.enabled = false;
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
        
    }
}
