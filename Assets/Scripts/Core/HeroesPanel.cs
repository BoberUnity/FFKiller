using UnityEngine;
using System.Collections.Generic;

public class HeroesPanel : MonoBehaviour
{
  public List<HeroUI> HeroesUi = new List<HeroUI>(4);
  private int attachedHeroes = 0;

  public HeroUI AttachHero (Hero hero)
  {
    if (attachedHeroes < 4)
    {
      HeroesUi[attachedHeroes].gameObject.SetActive(true);
      HeroesUi[attachedHeroes].Hero = hero;      
      ++attachedHeroes;
      return HeroesUi[attachedHeroes - 1];
    }
    Debug.LogWarning("You try attach more than 4 heroes");
    return null;
  }	
}
