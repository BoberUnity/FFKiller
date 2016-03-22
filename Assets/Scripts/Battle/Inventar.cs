using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class Inventar : MonoBehaviour
{
  [SerializeField] private List<ItemButton> itemButtons = new List<ItemButton>();
  public Text DescriptionField = null;

  public void AddItem(ThingPropetries thingPropetries)
  {
    int buttonIndex = FirstFreeButton;
    itemButtons[buttonIndex].Load(thingPropetries);    
  }

  private int FirstFreeButton
  {
    get
    {
      int i = 0;
      foreach (var itemButton in itemButtons)
      {
        if (!itemButton.IsBusy)
          return i;
        i++;
      }
      Debug.LogError("Has no free buttons");
      return 0;
    }
  }
}
