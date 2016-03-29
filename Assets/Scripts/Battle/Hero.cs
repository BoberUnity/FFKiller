using UnityEngine;
using System;

[Serializable] public class HeroPropetries
{
  public string Name = "";
  public Sprite Portrait = null;
  public float Hp = 0;
  public float Mhp = 0;
  public float Mp = 0;
  public float Mmp = 0;
  public float Cr = 0;
  public float Mcr = 0;
  public float Atk = 0;
  public float Def = 0;
  public float Mat = 0;
  public float Mdf = 0;
  public float Agi = 0;
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
  public float Cr = 0;
  public float Mcr = 0;
  public float Atk = 0;
  public float Def = 0;
  public float Mat = 0;
  public float Mdf = 0;
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

  public void AddPower(HeroPropetries hps)
  {
    heroPropetries.Hp += hps.Hp;
    heroPropetries.Mhp += hps.Mhp;
    heroPropetries.Mp += hps.Mp;
    heroPropetries.Mmp += hps.Mmp;
    heroPropetries.Cr += hps.Cr;
    heroPropetries.Mcr += hps.Mcr;
    heroPropetries.Atk += hps.Atk;
    heroPropetries.Def += hps.Def;
    heroPropetries.Mat += hps.Mat;
    heroPropetries.Mdf += hps.Mdf;
    heroPropetries.Agi += hps.Agi;
    if (heroUi != null)
      heroUi.UpdateUI();
  }
}
