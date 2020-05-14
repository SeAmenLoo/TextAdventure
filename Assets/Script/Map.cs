using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject objMap;
    public GameObject objTip;

    public MapNode[] nodes;
    public Dictionary<int,MapNode> dicNode;
    public float width;
    RectTransform MapRect;
    // Start is called before the first frame update
    void Awake()
    {
        MapRect = objMap.GetComponent<RectTransform>();
        dicNode = new Dictionary<int, MapNode>();
        for (int i = 0; i< nodes.Length; i++)
        {
            dicNode.Add(nodes[i].stateID, nodes[i]);
        }
        RefreshMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetMapStage(int id)
    {
        if (dicNode.ContainsKey(id))
        {
            dicNode[id].SetNode(true);
            UpdateMapPosition(dicNode[id].rect);

        }

        
    }
    public void UpdateMapPosition(RectTransform rect)
    {
        float posx;
        //Debug.Log(MapRect.rect.width + "-" + Screen.width + "/2<" + rect.localPosition.x);
        if((MapRect.rect.width-Screen.width) / 2< Mathf.Abs(rect.localPosition.x))
        {
            posx=Mathf.Sign(rect.localPosition.x) * (MapRect.rect.width - Screen.width) / 2;
        }
        else
        {
            posx = rect.localPosition.x;
        }
        MapRect.localPosition = new Vector3(-posx, 0,0);
        
    }
    public void RefreshMap()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SetNode(false);
        }
        float posx =  (MapRect.rect.width - Screen.width) / 2;
        MapRect.localPosition = new Vector3(posx, 0, 0);
    }

    public void SetMapShow(bool act)
    {
        if (act)
        {
            objMap.transform.localScale = new Vector3(1, 1, 1);
            objTip.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            objMap.transform.localScale = new Vector3(0,0,0);
            objTip.transform.localScale = new Vector3(0, 0, 0);
        }
       
    }

}
