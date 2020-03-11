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
        CheckPause();
    }
    private void OnDisable()
    {
        stage.onVideoEnd -= UpdateNext;
        stage.onCanSelect -= CanSelect;
    }

    void StartGame(int startStage)
    {
        curStage = startStage;
        StageData stageData= storyData.GetStageDataByID(curStage);
        stage.onVideoEnd += UpdateNext;
        stage.onCanSelect += CanSelect;

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
            if (stageData.Video.Contains(".mp4"))
            {
                stage.ActVideo(true);
                stage.ActImage(false);
                stage.SetVideo(stageData.Video);
                stage.PlayVideo();
            }
            else if (stageData.Video.Contains(".jpg"))
            {
                stage.ActVideo(false);
                stage.ActImage(true);
                stage.SetImage(stageData.Video);
                
            }
            
        }
        if (!string.IsNullOrEmpty(stageData.Audio))
        {
            stage.SetSound(stageData.Audio);
            stage.PlaySound();
        }
    }
    void UpdateNext()
    {
        StageData stageData = storyData.GetStageDataByID(curStage);

        Debug.Log(stageData.ID.ToString() +"\n Start!\n"+ stageData.Select);
        if (stageData.Select > 0)
        {
            //SelectData selectData = storyData.GetSelectDataByID(stageData.Select);
            //stage.SetSelect(selectData);
            //stage.ActAside(true);
            //StartCoroutine(WaitSelect(selectData));
            curStage = storyData.GetDefNextByID(stageData.ID); //stageData.defNext;
            lastStage = curStage;
            StopCoroutine("WaitSelect");
            UpdataStage(storyData.GetStageDataByID(curStage));
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
    void CanSelect()
    {
        
        StageData stageData = storyData.GetStageDataByID(curStage);

        Debug.Log("can select" + curStage);
        if (stageData.Select > 0)
        {
            SelectData selectData = storyData.GetSelectDataByID(stageData.Select);
            stage.SetSelect(selectData);
            //stage.ActAside(true);
            StartCoroutine(WaitSelect(selectData));
        }
    }
    IEnumerator WaitSelect(SelectData selectData)
    {
        while(true){
            if (Input.anyKeyDown)
            {
                for (int i = 0; i < selectData.items.Count; i++)
                {
                   
                    if (Input.GetKey(GetKeyCode(selectData.items[i].Button)))//todo输入按键
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
                    stage.ErrorPhone();
                    Debug.Log("error phone");
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
    public bool isPause=false;
    void CheckPause()
    {
        if (Input.GetKey(KeyCode.P))//todo暂停按键
        {
            if (isPause)
            {
                stage.PauseVideo(false);
                stage.SetPauseTip(false);
                isPause = !isPause;
            }
            else
            {
                stage.PauseVideo(true);
                stage.SetPauseTip(true);
                isPause = !isPause;
            }
            
        }
    }

}
