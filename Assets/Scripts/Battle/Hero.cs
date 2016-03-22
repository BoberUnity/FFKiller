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
