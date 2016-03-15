using UnityEngine;

public class Hero : MonoBehaviour
{
  [SerializeField] private Sprite portrait = null;
  [SerializeField] private float hp = 500;
  [SerializeField] private float mhp = 1000;
  [SerializeField] private float mp = 500;
  [SerializeField] private float mmp = 1000;
  [SerializeField] private float agi = 2;
  private HeroUI heroUi = null;
  private string thisName = "Hero";
  #region Propetries 
  public string Name
  {
    get { return gameObject.name; }
    set
    {
      thisName = value;
      heroUi.UpdateUI();
    }
  }
  public Sprite Portrait
  {
    get { return portrait; }
    set { portrait = value; }
  }
  public float Hp
  {
    get { return hp; }
    set
    {
      hp = value;
      if (heroUi != null)
        heroUi.UpdateUI();
    }
  }
  public float Mhp
  {
    get { return mhp; }
    set
    {
      mhp = value;
      if (heroUi != null)
        heroUi.UpdateUI();
    }
  }
  public float Mp
  {
    get { return mp; }
    set
    {
      mp = value;
      if (heroUi != null)
        heroUi.UpdateUI();
    }
  }
  public float Mmp
  {
    get { return mmp; }
    set
    {
      mmp = value;
      if (heroUi != null)
        heroUi.UpdateUI();
    }
  }
  public float Agi
  {
    get { return agi; }
    set
    {
      agi = value;
      if (heroUi != null)
        heroUi.UpdateUI();
    }
  }
  #endregion

  public void ConnectToPartyGui()
  {
    HeroesPanel heroesPanel = FindObjectOfType<HeroesPanel>();
    if (heroesPanel != null)
      heroUi = FindObjectOfType<HeroesPanel>().AttachHero(this);
  }
}
