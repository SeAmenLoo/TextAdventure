using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class FileReader : MonoBehaviour
{
    static FileReader fileReader =null;

    public static FileReader Instance
    {
        get
        {
            if (fileReader == null)
            {
                fileReader = new FileReader();
            }
            return fileReader;
        }

        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<string>  ReadCSV(string path)
    {
        List<string> data = new List<string>();
        StreamReader sr = null;
        try
        {
            string file_url = path ;    //根据路径打开csv文件
            sr = File.OpenText(file_url);
            //Debug.Log("File Find in " + file_url);
        }
        catch
        {
            //Debug.Log("File cannot find ! ");
            return null;
        }
        string line;
    
        while ((line = sr.ReadLine()) != null)   //按行读取
        {
            data.Add(line);

        }
        sr.Close();
        sr.Dispose();
        return data;
    }
    public Dictionary<int, StageData> GetStageData()
    {
        Dictionary<int, StageData> stageDataList = new Dictionary<int, StageData>();
        string path = Application.streamingAssetsPath + "/Config/Stage.csv";
        List<string> data = ReadCSV(path);
        //Debug.Log(data[0]);
        for (int i = 1; i < data.Count; i++)
        {
            string[] str = data[i].Split(',');
            StageData stageData = new StageData();

            stageData.ID = int.Parse(str[0]);

            stageData.StageName= str[1];
            stageData.Video= str[2];
            stageData.Audio=str[3];
            stageData.Image = str[4];
            stageData.Text = str[5];
            
            if(!string.IsNullOrEmpty( str[6]))stageData.Select = int.Parse(str[6]);
            stageData.PreState = new List<string>(str[7].Split('|'));
            stageData.State = new List<string>(str[8].Split('|'));
            //stageData.defNext = int.Parse(str[9]);
            
            stageDataList.Add(stageData.ID, stageData);
        } 
        return stageDataList;

    }
    public Dictionary<int, SelectData> GetSelectData()
    {
        Dictionary<int, SelectData> selectDataList = new Dictionary<int, SelectData>();
        string path = Application.streamingAssetsPath + "/Config/Select.csv";
        List<string> data = ReadCSV(path);
        //Debug.Log(data[0]);
        for (int i = 1; i < data.Count; i++)
        {
            string[] str = data[i].Split(',');
            SelectData selectData = new SelectData();

            selectData.ID = int.Parse(str[0]);
            selectData.Title = str[1];
            List<SelectItem> items = new List<SelectItem>();
            
            string[] itemButtons = str[2].Split('|');
            string[] itemTexts = str[3].Split('|');
            string[] itemNexts = str[4].Split('|');
            for (int j = 0; j < itemButtons.Length; j++)
            {
                SelectItem item = new SelectItem();
                item.Button = itemButtons[j];
                item.Text = itemTexts[j];
                string[] next =itemNexts[j].Split('*');
                //item.PreState = int.Parse(next[0]);
                item.Next = int.Parse(next[0]);
                items.Add(item);
            }
            selectData.items = items;



            selectDataList.Add(selectData.ID, selectData);
        }
        return selectDataList;

    }
    public Dictionary<int, PhoneData> GetPhoneData()
    {
        Dictionary<int, PhoneData> phoneDataList = new Dictionary<int, PhoneData>();
        string path = Application.streamingAssetsPath + "/Config/Phone.csv";
        List<string> data = ReadCSV(path);
        //Debug.Log(data[0]);
        for (int i = 1; i < data.Count; i++)
        {
            string[] str = data[i].Split(',');
            PhoneData phoneData = new PhoneData();

            phoneData.ID = int.Parse(str[0]);
            phoneData.Num = str[1];
            phoneData.PreStage=new List<string>(str[2].Split('|'));
            phoneData.Stage = int.Parse(str[3]);


            phoneDataList.Add(phoneData.ID, phoneData);
        }
        return phoneDataList;

    }
    public Dictionary<int, StateData> GetStateData()
    {
        Dictionary<int, StateData> stateDataList = new Dictionary<int, StateData>();
        string path = Application.streamingAssetsPath + "/Config/State.csv";
        List<string> data = ReadCSV(path);
        //Debug.Log(data[0]);
        for (int i = 1; i < data.Count; i++)
        {
            string[] str = data[i].Split(',');
            StateData stateData = new StateData();

            stateData.ID = int.Parse(str[0]);
            stateData.Detail = str[1];
            stateData.state = false;


            stateDataList.Add(stateData.ID, stateData);
        }
        return stateDataList;

    }

}
public class StageData
{
    public int ID;
    public string StageName;
    public string Video;
    public string Audio;
    public string Image;
    public string Text;

    public int Select;
    public List<string> PreState;
    public List<string> State;
    //public int defNext;
}

public class SelectData
{
    public int ID;
    public string Title;
    public List<SelectItem> items;
}
public class SelectItem
{
    public string Button;
    public string Text;
    public int PreState;//便于处理使用string，实际上是int型id
    //Next  对应选项的对应ID，使用|分隔，若为正则跳转至对应stage，负则显示对应select，进行进一步选择
    //      若存在state状态判断，则在对应next前添加stateid,使用*分隔
    public int Next;
}

public class PhoneData
{
    public int ID;
    public string Num;
    public List<string> PreStage;//便于处理使用string，实际上是int型id
    public int Stage;
}
public class StateData
{
    public int ID;
    public string Detail;
    public bool state;
}




