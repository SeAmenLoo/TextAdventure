using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Stage stage;
    public StoryData storyData;
    int curStage;
    int lastStage;
    // Start is called before the first frame update
    void Start()
    {
        storyData.LoadData();
        StartGame(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
        stage.onVideoEnd -= UpdateSelect;
    }
    void StartGame(int startStage)
    {
        curStage = startStage;
        StageData stageData= storyData.GetStageDataByID(curStage);
        stage.onVideoEnd += UpdateSelect;


        UpdataStage(stageData);
        
    }
    void UpdataStage(StageData stageData)
    {
        if (stageData.PreState != null && stageData.PreState.Count >= 0)
        {
            for (int i = 0; i < stageData.PreState.Count; i++)
            {
                if (string.IsNullOrEmpty(stageData.PreState[i])) continue;
                if(int.Parse(stageData.PreState[i]) < 0){
                    if (storyData.GetStateDataByID(-int.Parse(stageData.PreState[i])).state){
                        curStage = -storyData.GetDefNextByID(stageData.ID); //stageData.defNext;
                        lastStage = curStage;
                        UpdataStage(storyData.GetStageDataByID(curStage));
                        return;
                    }
                }
                else if(int.Parse(stageData.PreState[i]) > 0)
                {
                    if (!storyData.GetStateDataByID(int.Parse(stageData.PreState[i])).state)
                    {
                        curStage = storyData.GetDefNextByID(stageData.ID); //stageData.defNext;
                        lastStage = curStage;
                        UpdataStage(storyData.GetStageDataByID(curStage));
                        return;
                    }
                }
                
            }
        }
        if (stageData.State != null && stageData.State.Count >= 0)
        {
            for (int i = 0; i < stageData.State.Count; i++)
            {
                if (string.IsNullOrEmpty(stageData.State[i])) continue;
                if (int.Parse(stageData.State[i]) > 0)
                {
                    storyData.SetStateData(int.Parse(stageData.State[i]), true);
                }
                else if(int.Parse(stageData.State[i]) < 0)
                {
                    storyData.SetStateData(-int.Parse(stageData.State[i]), false);
                }
                
            }

        }
        stage.ActInputF(false);
        stage.ActAside(false);

        if (!string.IsNullOrEmpty(stageData.Video))
        {
            Debug.Log(stageData.ID.ToString() +'\n'+ stageData.StageName + '\n' + stageData.Video+"\n Start!");
            stage.ActVideo(true);
            stage.ActImage(false);
            stage.SetVideo(stageData.Video);
            stage.PlayVideo();
        }
    }
    void UpdateSelect()
    {
        StageData stageData = storyData.GetStageDataByID(curStage);

        Debug.Log(stageData.ID.ToString() +"\n Start!\n"+ stageData.Select);
        if (stageData.Select > 0)
        {
            SelectData selectData = storyData.GetSelectDataByID(stageData.Select);
            stage.SetSelect(selectData);
            //stage.ActAside(true);
            StartCoroutine(WaitSelect(selectData));
        }
        else if(stageData.Select<0)
        {

            lastStage = curStage;
            curStage = -stageData.Select;
            UpdataStage(storyData.GetStageDataByID(curStage));
        } 
        else if (stageData.Select == 0)
        {
            StartCoroutine(WaitPhone());
        }
    }
    IEnumerator WaitSelect(SelectData selectData)
    {
        while(true){
            if (Input.anyKeyDown)
            {
                for (int i = 0; i < selectData.items.Count; i++)
                {
                   
                    if (Input.GetKey(GetKeyCode(selectData.items[i].Button)))
                    {
          
                        lastStage = curStage;
                        curStage = -selectData.items[i].Next;
                        UpdataStage(storyData.GetStageDataByID(curStage));
                        yield break;
                    }
                }
            }
            

            yield return null;
        }

    }
    KeyCode GetKeyCode(string s)
    {
        if (s.Length > 1)
        {
            //特殊按键
        }
        else
        {
            
            //输入字符
        }
        return (KeyCode)s[0];
    }

    IEnumerator  WaitPhone()//判断电话输入，
    {
        while(true){
            if (!stage.inputField.IsActive())
            {
                stage.ActInputF(true);
            }
            if (stage.strPhoneNum.Length == 5)
            {
                StageData stageData= storyData.GetStageDataByPhone(stage.strPhoneNum, lastStage);
                if (stageData == null)
                {
                    stage.ActInputF(true);
                }
                else
                {
                    lastStage = curStage;
                    curStage = stageData.ID;
                    UpdataStage(stageData);
                    yield break;
                }
                
            }
            
            yield return null;
        }
        
    }

}
