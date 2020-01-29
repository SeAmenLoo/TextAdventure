using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBranch : StoryItemBase {

  public StoryState StateToCheck;
  public StoryItemBase ItemIfTrue;
  public StoryItemBase ItemIfFalse;

  public override void Activate(GameManager gm)
  {
    if (StateToCheck.Value == true)
      gm.SetCurrentStoryItem (ItemIfTrue);
    else
      gm.SetCurrentStoryItem (ItemIfFalse);
  }
}
