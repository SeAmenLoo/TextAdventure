using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject objMap;
    public MapNode[] nodes;
    public Dictionary<int,MapNode> dicNode;
    public float width;
    RectTransform MapRect;
    // Start is called before the first frame update
    void Start()
    {
        MapRect = GetComponent<RectTransform>();
        dicNode = new Dictionary<int, MapNode>();
        for (int i = 0; i< nodes.Length; i++)
        {
            dicNode.Add(nodes[i].stateID, nodes[i]);
        }
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
        if((MapRect.rect.width-Screen.width) / 2< Mathf.Abs(rect.position.x))
        {
            posx=Mathf.Sign(rect.position.x) * (MapRect.rect.width - Screen.width) / 2;
        }
        else
        {
            posx = rect.position.x;
        }
        MapRect.position-=new Vector3(posx, 0,0);
        
    }
    public void RefreshMap()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].SetNode(false);
        }

    }

    public void SetMapShow(bool act)
    {
        objMap.SetActive(act);
    }

}
