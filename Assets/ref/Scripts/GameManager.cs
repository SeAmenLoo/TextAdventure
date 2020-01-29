using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour 
{
  public Text Description;
  public Button[] OptionButtons;

  public StoryItemBase CurrentItem;

  private int _numButtons;
  private Text[] _buttonTexts;
  private string[] _optionTexts;
  private StoryItemBase[] _optionItems;

	// Use this for initialization
	void Start () 
  {
    _numButtons = OptionButtons.GetLength(0);
        
    GetButtonTexts ();

    CurrentItem.Activate (this);
	}
	
	// Update is called once per frame
	void Update () 
  {
		
	}

  private void GetButtonTexts()
  {
    _buttonTexts = new Text[_numButtons];

    for (int i = 0; i < _numButtons; i++)
    {
      _buttonTexts[i] = 
        OptionButtons[i].GetComponentInChildren<Text>(true);
    }
  }

  public void SetCurrentStoryItem(StoryItemBase item)
  {
    CurrentItem = item;
    CurrentItem.Activate (this);
  }
    
  public void OnButton(int index)
  {
    SetCurrentStoryItem(_optionItems [index]);
  }

  public void SetCardDetails(string desc, string[] optionTexts, 
                             StoryItemBase[] optionItems)
  {
    Description.text = desc;
    _optionTexts = optionTexts;
    _optionItems = optionItems;

    UpdateButtons();
  }

  public void UpdateButtons()
  {
    int numOptionTexts = _optionTexts == null ? 0 : 
                                           _optionTexts.GetLength (0);
    int numOptionItems = _optionItems == null ? 0 :
                                           _optionItems.GetLength (0);

    int numActiveButtons = Math.Min (numOptionItems, numOptionTexts);

    for (int i = 0; i < _numButtons; i++)
    {
      if (i < numActiveButtons)
      {
        OptionButtons [i].gameObject.SetActive (true);
        _buttonTexts [i].text = _optionTexts [i];
      }
      else
      {
        OptionButtons [i].gameObject.SetActive (false);
      }
    }
  }

}
