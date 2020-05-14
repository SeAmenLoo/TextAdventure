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
    public void LoadData()
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
            //Debug.Log("not found");
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
            //Debug.Log("not found");
            return null;
        }
    }
    public int GetDefNextByID(int id)
    {
        if (stageData.ContainsKey(id))
        {
            if (stageData[id].Select <= 0)
            {
                return -stageData[id].Select;
            }
            else
            {
                return -GetSelectDataByID(stageData[id].Select).items[0].Next;
            }
        }
        return -1;
    }
    public PhoneData GetPhoneDataByID(int id)
    {
        if (phoneData.ContainsKey(id))
        {
            return phoneData[id];
        }
        else
        {
            //Debug.Log("not found");
            return null;
        }
    }
    public StageData GetStageDataByPhone(string num,int stage)
    {
        for(int i =1; i <= phoneData.Count; i++)
        {
            if (num == phoneData[i].Num)
            {
                for(int j = 0; j < phoneData[i].PreStage.Count; j++)
                {
                    if (string.IsNullOrEmpty(phoneData[i].PreStage[j])) continue;
                    if (stage == int.Parse(phoneData[i].PreStage[j]))
                    {
                        return stageData[phoneData[i].Stage];
                    }
                }
            }
        }

        //Debug.Log("not found"+num+" on "+stage);
        return null;

    }
    public StateData GetStateDataByID(int id)
    {
        if (stateData.ContainsKey(id))
        {
            return stateData[id];
        }
        else
        {
            //Debug.Log("not found");
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
