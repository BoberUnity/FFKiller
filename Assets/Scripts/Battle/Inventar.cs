using UnityEngine;
using System.Collections.Generic;

public class Inventar : MonoBehaviour
{
  [SerializeField] private List<ItemButton> itemButtons = new List<ItemButton>();
  

  public void AddItem(HeroPropetries heroPropetries)
  {
    int buttonIndex = FirstFreeButton;
    itemButtons[buttonIndex].Show(heroPropetries);    
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
