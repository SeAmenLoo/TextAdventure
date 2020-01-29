using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectionText : MonoBehaviour
{

    public Text num;
    public Text detail;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetSelect(SelectItem data)
    {
        num.text = data.Button;
        detail.text = data.Text;

    }
}
