using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventar : MonoBehaviour
{
  [SerializeField] private List<ItemButton> itemButtons = new List<ItemButton>();  
  public Text DescriptionField = null;
  [SerializeField] private Animator heroesAnimaator = null;
  public HeroesPanel HeroesPanel = null;
  [HideInInspector] public bool IsReadyAddPower = false;
  [HideInInspector] public HeroPropetries HeroPropetries = null;

 public void AddItem(ThingPropetries thingPropetries)
  {
    bool addToExistButton = false;
    foreach (var itemButton in itemButtons)
    {
      if (itemButton.ThingPropetries.Name == thingPropetries.Name)
      {
        itemButton.UpdateCount(thingPropetries.Count);
        addToExistButton = true;
      }
    }
    if (!addToExistButton)
    {
      int buttonIndex = FirstFreeButton;
      itemButtons[buttonIndex].Load(thingPropetries);
    }    
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

  public void ShowHeroes()
  {
    heroesAnimaator.SetBool("IsVisible", true);
  }

  public void HideHeroes()
  {
    heroesAnimaator.SetBool("IsVisible", false);
  }
}
