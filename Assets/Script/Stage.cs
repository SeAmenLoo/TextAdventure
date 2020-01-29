using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
public class Stage : MonoBehaviour
{

    //Event
    public delegate void CallEvent();//声明委托类型
    public CallEvent onVideoEnd;
    //背景音乐 视频 图片
    public AudioSource sound;
    public VideoPlayer video;
    public Image image;

    //角色对话(目前不用)
    public GameObject Objtalk;
    public Text txtTalker;
    public Text txtTalkcontent;

    //旁白
    public GameObject objAside;
    public Text txtAside;
    public GameObject listbox;
    public GameObject PrefebSelection;
    public List<SelectionText> listSelections;

    //输入号码
    public InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
#region 对话
    public void SetTalk(string ter,string tc){
        txtTalker.text=ter;
        txtTalkcontent.text=tc;
    }
    public void ActTalk(bool act){
        Objtalk.SetActive(act);
    }
#endregion
#region 视频
    public void SetVideo(string videoId){
        video.source=VideoSource.Url;
        video.playOnAwake=false;
        video.url=Application.streamingAssetsPath+videoId;
    }
    public void ActVideo(bool act){
        video.gameObject.SetActive(act);
    }
    public void PlayVideo(bool play=true){
        if(play){
            video.Play();
            
        }
        else
        {
            video.Stop();
        }
    }
    IEnumerator VideoCheck()
    {
        
        if(video.frame== (long)video.frameCount)
        {
            video.Pause();
            onVideoEnd();
            yield break;
        }
        yield return null;

    }
#endregion
#region 旁白
    public void SetAside(string asi){
        txtAside.text=asi;
    }
    public void ActAside(bool act){
        objAside.SetActive(act);
    }
#endregion
#region 配音 视频自带配音，此处仅为背景音乐
    public void SetSound(AudioClip ac){
        sound.clip=ac;
        sound.playOnAwake=false;
    }
    public void PlaySound(bool play=true){
        if(play){
            sound.Play();
        }
        else
        {
            sound.Stop();
        }
    }
#endregion
#region 配图 若无视频显示配图
    public void SetImage(Sprite s){
        image.sprite=s;
    }
    public void ActImage(bool act){
        image.gameObject.SetActive(act);
    }
    #endregion
#region 选项 
    public void SetSelect(SelectData data)
    {
        listSelections.Clear();
        txtAside.text = data.Title;
        for(int i = 0; i < data.items.Count; i++)
        {
            Transform tr = listbox.transform;
            tr.position += new Vector3(0f, i*60f, 0f);
            listSelections.Add( GameObject.Instantiate(PrefebSelection, tr).GetComponent<SelectionText>());
        }
    }
#endregion
#region 输入
    public void ActInputF(bool act=true){
        inputField.gameObject.SetActive(act);
        if(act){
            inputField.ActivateInputField();
        }
    }
#endregion
}
