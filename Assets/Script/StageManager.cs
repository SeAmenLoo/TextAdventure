﻿using System.Collections;
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
        UpdataStage(stageData);
        stage.onVideoEnd += UpdateSelect;
    }
    void UpdataStage(StageData stageData)
    {

        if (!string.IsNullOrEmpty(stageData.Video))
        {
            stage.ActVideo(true);
            stage.ActImage(false);
            stage.SetVideo(stageData.Video);
            stage.PlayVideo();
        }
    }
    void UpdateSelect()
    {
        StageData stageData = storyData.GetStageDataByID(curStage);
        if (stageData.Select > 0)
        {
            SelectData selectData = storyData.GetSelectDataByID(stageData.Select);
            stage.SetSelect(selectData);
            stage.ActAside(true);
            StartCoroutine(WaitInput(selectData));
        }
        else
        {

            lastStage = curStage;
            curStage = -stageData.Select;
            UpdataStage(storyData.GetStageDataByID(curStage));
        } 
    }
    IEnumerator WaitInput(SelectData selectData)
    {
        for(int i = 0; i > selectData.items.Count; i++)
        {
            if (Input.GetKey(GetKeyCode(selectData.items[i].Button)))
            {
                lastStage = curStage;
                curStage = selectData.items[i].Next;
                UpdataStage(storyData.GetStageDataByID(curStage));
                yield break;
            }
        }
        Phone();
        yield return null;
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

    public void Phone()//判断电话输入，注意跳转stage时关闭输入判断协程
    {

    }

}
