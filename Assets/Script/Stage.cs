using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Networking;
public class Stage : MonoBehaviour
{
    
    //Event
    public delegate void CallEvent();//声明委托类型
    public CallEvent onVideoEnd;
    public CallEvent onCanSelect;
    public CallEvent onMaskEnd;
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
    public string strPhoneNum;
    public GameObject objErrorTip;
    //选择进度条 
    public int timeRange;
    public Slider slider;
    //默认选项帧数
    public int defFrame=150;
    public GameObject objPause;

    public int maskFrame ;
    public Image mask;
    // Start is called before the first frame update
    void Start()
    {
        inputField.onValueChanged.AddListener(Changed_Value);

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
    
        video.url=Application.streamingAssetsPath+'/'+videoId;
        video.controlledAudioTrackCount = 1;
        //video.audioOutputMode = VideoAudioOutputMode.Direct;
    }
    public void ActVideo(bool act){
        video.gameObject.SetActive(act);

    }
    IEnumerator videoCheck;
    public void PlayVideo(){



        ClearVideo();
        videoCheck = VideoCheck();
        StartCoroutine(videoCheck);
            
        
    }
    public void ClearVideo()
    {
        if (videoCheck != null)
        {
            StopCoroutine(videoCheck);
            video.Stop();
        }
        
       
    }
    public bool isPause;
    public void PauseVideo(bool pause)
    {
        isPause = pause;
        if (!pause)
        {
            video.Play();
        }
        else
        {
            video.Pause();
        }
    }
    
    IEnumerator VideoCheck()
    {

    
        video.Prepare();
        video.EnableAudioTrack(0, true);
        while (!video.isPrepared)
        {
            yield return null;
        }
        video.Play();
        isPause = false;
        int i = 0;
        int frame = (int)video.frameCount - defFrame;
        //Debug.Log(video.url);
        bool canselect = false;
        while (true){

            i++;
            slider.value = i / (float)video.frameCount;
            // Debug.Log(video.isPlaying);
            //Debug.Log(video.frame);
            //Debug.Log(video.frameCount);
            //if (!video.isPlaying)
            //{
            //    Debug.Log(video.frame);
            //    Debug.Log(video.frameCount);
            //}
            if (video.frame>= frame && !canselect)
            {
                canselect = true;
                onCanSelect();
            }
            if(!video.isPlaying&&!isPause) //video.frame>= (long)video.frameCount
            {
                video.Stop();
                onVideoEnd();
                yield break;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                video.Stop();
                onVideoEnd();
                yield break;
            }
            yield return null;
        }
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
    public AudioClip[] clips;
    string urlMp3;
    IEnumerator LoadMp3()
    {


        using(var uwr = UnityWebRequestMultimedia.GetAudioClip(urlMp3, AudioType.OGGVORBIS))
{
            yield return uwr.SendWebRequest();
            
            AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
            // use audio clip
            sound.clip = clip;
            sound.Play();
        }

    }
    public void SetSound(string audioID){
        //urlMp3 = "file://" + Application.streamingAssetsPath + audioID;
        //AudioClip cl = Resources.Load<AudioClip>(urlMp3);
        //sound.clip = cl;
        //PlaySound();

        //StartCoroutine(LoadMp3());
        //DownloadHandlerAudioClip a = new DownloadHandlerAudioClip(urlMp3, AudioType.MPEG);

        //sound.clip = a.audioClip;
        //sound.playOnAwake = false;
        foreach(AudioClip clip in clips)
        {
            if(clip.name== audioID)
            {
                sound.clip = clip;
            }
        }
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

    IEnumerator TimeCount()
    {
        slider.gameObject.SetActive(false);
        int i = 0;
        while (true)
        {
            
            i++;
            slider.value = i / timeRange;
            //if(video.frame>= (long)video.frameCount)
            if (i>= timeRange)
            {
                slider.gameObject.SetActive(true);
                onVideoEnd();
                yield break;
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                slider.gameObject.SetActive(true);
                onVideoEnd();
                
                yield break;
            }
            yield return null;
        }
    }
    public void SetImage(string imageID){
        Sprite s = Resources.Load<Sprite>(Application.streamingAssetsPath + '/' + imageID);
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
            inputField.text = "";
            inputField.ActivateInputField();
         
        }
        
    }
    public void Changed_Value(string inp)
    {
        if (inp.EndsWith("#")) //todo输入井号判断
        {
            inp = inp.Substring(0, inp.Length - 2);
            inputField.text = inp;
            inputField.ActivateInputField();
        }
        strPhoneNum = inp;
    }
    public void ErrorPhone()
    {
        objErrorTip.SetActive(true);
        StartCoroutine(ErrorTip());
    }
    public Text texError;
    IEnumerator ErrorTip()
    {
        float i = 0;
        texError.color=new Color(1f,1f,1f,i);
        
        while (true)
        {
            i += 0.1f;
            texError.color = new Color(1f, 1f, 1f, i);
            
            if (i >= 1)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        while (true)
        {
            i -= 0.1f;
            texError.color = new Color(1f, 1f, 1f, i);

            if (i <=0)
            {
                
                objErrorTip.SetActive(false);
                yield break;
            }
            yield return null;
        }
       

    }
    #endregion
    #region 暂停
    public void SetPauseTip(bool t = true)
    {
        objPause.SetActive(t);
        if (t)
        {
            mask.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
        else
        {
            mask.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
    }
    public void SelectMask()
    {
        StartCoroutine(SelectEnd());
    }
    [HideInInspector]
    public bool isMap;
    public int delayFrame=10;
    IEnumerator SelectEnd()
    {
        int i = 0;
        bool flag=false;
        bool dark = true ;
        while (true)
        {
            if (!flag)
            {
                i++;
            }
            else
            {
                i--;
            }
            mask.color = new Color(0.0f, 0.0f, 0.0f, (float)i / maskFrame);
            if (i > maskFrame&&dark) {
                //mask.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                dark = false;
                if (!isMap)
                {
                    ClearVideo();
                }
                
                onMaskEnd();
               
            }
            if (i > maskFrame + delayFrame)
            {
                flag = true;
            }
            else if (i < 0 && flag)
            {
                
                yield break;
            }
            yield return null;
        }
    }
    #endregion
}
