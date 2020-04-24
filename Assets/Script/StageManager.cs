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
        CheckMapVisable();
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
                        curStage = storyData.GetDefNextByID(stageData.ID); //stageData.defNext;
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
        canPause = true;
        stage.ActInputF(false);
        stage.ActAside(false);
        stageMap.SetMapStage(stageData.ID);
        //stage.ClearVideo();
        if (!string.IsNullOrEmpty(stageData.Video))
        {
           
            Debug.Log(stageData.ID.ToString() +" Start");
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
            if (stageData.Audio == "clear")
            {
                stage.PlaySound(false);
            }
            else
            {
                stage.SetSound(stageData.Audio);
                stage.PlaySound();
            }

        }
        
    }
    IEnumerator waitSelect;
    void UpdateNext()
    {
        StageData stageData = storyData.GetStageDataByID(curStage);

        Debug.Log(stageData.ID.ToString() +" end");
        Debug.Log("last:"+lastStage);
        if (stageData.Select > 0)
        {
            //SelectData selectData = storyData.GetSelectDataByID(stageData.Select);
            //stage.SetSelect(selectData);
            //stage.ActAside(true);
            //StartCoroutine(WaitSelect(selectData));
            lastStage = curStage;
            curStage = storyData.GetDefNextByID(stageData.ID); //stageData.defNext;
            
            if (waitSelect != null) 
                StopCoroutine(waitSelect);
            waitSelect = null;
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
            //StartCoroutine(WaitPhone());
        }
    }
    void CanSelect()
    {
        
        StageData stageData = storyData.GetStageDataByID(curStage);

        
        if (stageData.Select > 0)
        {
            canPause = false;
            Debug.Log("can select" + curStage);
            SelectData selectData = storyData.GetSelectDataByID(stageData.Select);
            stage.SetSelect(selectData);
            //stage.ActAside(true);
            if (waitSelect != null)
            {
                StopCoroutine(waitSelect);
                waitSelect = null;
        

            }
            waitSelect = WaitSelect(selectData);
            StartCoroutine(waitSelect);
        
        }
        if (stageData.Select == 0)
        {
            canPause = false;
            StartCoroutine(WaitPhone());
        }
    }
    IEnumerator WaitSelect(SelectData selectData)
    {
        Debug.Log("Waiting");
        while (true){
            
            if (Input.anyKeyDown)
            {
                for (int i = 0; i < selectData.items.Count; i++)
                {
                   
                    if (Input.GetKey(GetKeyCode(selectData.items[i].Button)))//todo输入按键
                    {
          
                        lastStage = curStage;
                        curStage = -selectData.items[i].Next;
                        stage.onMaskEnd = Select;
                        stage.SelectMask();
                        Debug.Log("endWait");
                        yield break;
                    }
                }
            }
            

            yield return null;
        }

    }

    void Select()
    {
        UpdataStage(storyData.GetStageDataByID(curStage));
    }
    KeyCode GetKeyCode(string s)
    {
        if (s.Length > 1)
        {
            
            //特殊按键
        }
        else
        {
     
            if (s[0] == '0') return KeyCode.Keypad0;
            else if(s[0] == '1') return KeyCode.Keypad1;
            else if (s[0] == '2') return KeyCode.Keypad2;
            else if (s[0] == '3') return KeyCode.Keypad3;
            else if (s[0] == '4') return KeyCode.Keypad4;
            else if (s[0] == '5') return KeyCode.Keypad5;
            else if (s[0] == '6') return KeyCode.Keypad6;
            else if (s[0] == '7') return KeyCode.Keypad7;
            else if (s[0] == '8') return KeyCode.Keypad8;
            else if (s[0] == '9') return KeyCode.Keypad9;
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
                yield return new WaitForSeconds(0.5f);
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

                    stage.onMaskEnd = Select;
                    stage.SelectMask();
                    yield break;
                }
                
            }
            
            yield return null;
        }
        
    }

    //暂停
    bool isPause=false;
    bool canPause=true;
    void CheckPause()
    {
        
        if (Input.GetKeyDown(KeyCode.P)&&canPause)//todo暂停按键
        {
            Debug.Log("pause");
            if (isPause)
            {
                SetPause(false);


            }
            else
            {
                SetPause(true);


            }
        }
    }
    void SetPause(bool p)
    {
        stage.PauseVideo(p);
        stage.SetPauseTip(p);
        isPause = !isPause;
    }


    public Map stageMap;
    bool isMapShow = false;
    bool canShowMap=true;
    void CheckMapVisable()
    {
        if (Input.GetKeyDown(KeyCode.O) && canShowMap)//todo菜单按键
        {
            Debug.Log("showMap");
            if (isMapShow)
            {
                SetPause(false);
                stageMap.SetMapShow(false);
            }
            else
            {
                SetPause(true);
                stageMap.SetMapShow(true); 
            }
            isMapShow = !isMapShow;

        }
    }
    
    
}
