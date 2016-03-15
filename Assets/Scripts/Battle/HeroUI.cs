using UnityEngine;
using UnityEngine.UI;

public class HeroUI : MonoBehaviour
{
  [SerializeField] private Hero hero = null;
  [SerializeField] private Text nameIndicator = null;
  [SerializeField] private Image portraitImage = null;
  [SerializeField] private Text hpIndicator = null;
  [SerializeField] private Image hpLine = null;
  [SerializeField] private Image mpLine = null;
  [SerializeField] private Text mpIndicator = null;
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
    hpIndicator.text = hero.Hp.ToString("f0") + "/" + hero.Mhp.ToString("f0");
    hpLine.fillAmount = hero.Hp / hero.Mhp;
    mpIndicator.text = hero.Mp.ToString("f0") + "/" + hero.Mmp.ToString("f0");
    mpLine.fillAmount = hero.Mp / hero.Mmp;
  }
}
