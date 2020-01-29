using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryData : MonoBehaviour
{
    Dictionary<int, SelectData> selectData;
    Dictionary<int, StageData> stageData;
    Dictionary<int, PhoneData> phoneData;
    Dictionary<int, StateData> stateData;
    // Start is called before the first frame update
    void Start()
    {
        selectData = FileReader.Instance.GetSelectData();
        stageData = FileReader.Instance.GetStageData();
        phoneData = FileReader.Instance.GetPhoneData();
        stateData = FileReader.Instance.GetStateData();
    }

    public SelectData GetSelectDataByID(int id)
    {
        if (selectData.ContainsKey(id))
        {
            return selectData[id];
        }
        else
        {
            Debug.Log("not found");
            return null;
        }
    }
    public StageData GetStageDataByID(int id)
    {
        if (stageData.ContainsKey(id))
        {
            return stageData[id];
        }
        else
        {
            Debug.Log("not found");
            return null;
        }
    }
    public PhoneData GetPhoneDataByID(int id)
    {
        if (phoneData.ContainsKey(id))
        {
            return phoneData[id];
        }
        else
        {
            Debug.Log("not found");
            return null;
        }
    }
    public StateData GetStateDataByID(int id)
    {
        if (stateData.ContainsKey(id))
        {
            return stateData[id];
        }
        else
        {
            Debug.Log("not found");
            return null;
        }
    }
    public void SetStateData(int id,bool t)
    {
        if (stateData.ContainsKey(id))
        {
            stateData[id].state = t;
            return;
        }
        else
        {
            return;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
