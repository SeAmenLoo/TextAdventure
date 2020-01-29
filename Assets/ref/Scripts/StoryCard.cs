using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryCard : StoryItemBase {

  public string Description;

  public string[] Options;
  public StoryItemBase[] Items;

  public StoryState[] StatesToSetTrue;
  public StoryState[] StatesToSetFalse;

	public override void Activate(GameManager gm)
  {
    gm.SetCardDetails (Description, Options, Items);

    UpdateStates ();
  }

  private void UpdateStates()
  {
    if (StatesToSetTrue != null)
    {
      foreach (StoryState s in StatesToSetTrue)
        s.Value = true;
    }

    if (StatesToSetFalse != null)
    {
      foreach (StoryState s in StatesToSetFalse)
        s.Value = false;
    }
  }
}
