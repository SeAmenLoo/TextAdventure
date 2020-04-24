using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapNode : MonoBehaviour
{
    public int stateID;
    public Image image;
    public void SetNode(bool act)
    {
        if (!act)
        {
            image.color=new Color(1,1,1,0);
        }
        else
        {
            image.color = new Color(1, 1, 1, 1);
        }
    }
    public RectTransform rect;
    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
