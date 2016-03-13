using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{
  [SerializeField] private Hero hero = null;
  [SerializeField] private Text nameIndicator = null;
  [SerializeField] private Image portraitImage = null;
  [SerializeField] private Text hpIndicator = null;
  [SerializeField] private Text mhpIndicator = null;  
  [SerializeField] private Image aglImage = null;

  public Hero Hero
  {
    get { return hero; }
    set
    {
      hero = value;
      UpdateUI();
    }
  }
  public void UpdateUI ()
  {
    nameIndicator.text = hero.Name;
    portraitImage.sprite = hero.Portrait;
    hpIndicator.text = hero.Hp.ToString("f0");
    mhpIndicator.text = hero.Mhp.ToString("f0");    
  }
}
