using UnityEngine;
using System;

[Serializable] public class HeroPropetries
{
  public string Name = "";
  public Sprite Portrait = null;
  public float Hp = 500;
  public float Mhp = 1000;
  public float Mp = 500;
  public float Mmp = 1000;
  public float Co = 10;
  public float Mco = 100;
  public float Agi = 2;
}

[Serializable] public class ThingPropetries
{
  public string Name = "Name";
  public Sprite Portrait = null;
  public string Description = "Description";
  public int Count = 1;
  public float Hp = 0;
  public float Mhp = 0;
  public float Mp = 0;
  public float Mmp = 0;
  public float Co = 0;
  public float Mco = 0;
  public float Agi = 0;
}

public class Hero : MonoBehaviour
{
  [SerializeField] private HeroPropetries heroPropetries = null;
  private HeroUI heroUi = null;

  public HeroPropetries HeroPropetries
  {
    get { return heroPropetries;}
    set
    {
      heroPropetries = value;
      if (heroUi != null)
        heroUi.UpdateUI();      
    }
  }

  public void ConnectToPartyGui()
  {
    HeroesPanel heroesPanel = FindObjectOfType<HeroesPanel>();
    if (heroesPanel != null)
      heroUi = FindObjectOfType<HeroesPanel>().AttachHero(this);
  }
}
