using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class DataHelper : Editor
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [MenuItem("Helper/RenameCSV(将csv改为txt格式)")]
    static void RenameCSV()
    {
        FileReader.Instance.GetPhoneData();
        return;
        string path = "Res/Excel/";
        if (Directory.Exists(path))
        {
            DirectoryInfo direction = new DirectoryInfo(path);
            FileInfo[] files = direction.GetFiles();
            //Debug.Log(files.Length);
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".csv"))
                {
                    //Debug.Log(files[i].Name);
                }             
            }
        }
    }
}
