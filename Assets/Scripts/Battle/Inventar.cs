using UnityEngine;
using System.Collections.Generic;

public class Inventar : MonoBehaviour
{
  [SerializeField] private List<ItemButton> items = new List<ItemButton>();
  private int itemsCount = 0;

  public void AddItem(HeroPropetries heroPropetries)
  {
    items[itemsCount].Show(heroPropetries);
  }
}
