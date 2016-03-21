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
  [SerializeField] private Image coLine = null;
  [SerializeField] private Text mpIndicator = null;
  [SerializeField] private Text coIndicator = null;
  [SerializeField] private Image aglImage = null;

  public Hero Hero
  {
    get { return hero; }
    set
    {
      hero = value;
      gameObject.SetActive(true);
      UpdateUI();
    }
  }

  public void UpdateUI ()
  {
    nameIndicator.text = hero.HeroPropetries.Name;
    portraitImage.sprite = hero.HeroPropetries.Portrait;
    hpIndicator.text = hero.HeroPropetries.Hp.ToString("f0") + "/" + hero.HeroPropetries.Mhp.ToString("f0");
    hpLine.fillAmount = hero.HeroPropetries.Hp / hero.HeroPropetries.Mhp;
    mpIndicator.text = hero.HeroPropetries.Mp.ToString("f0") + "/" + hero.HeroPropetries.Mmp.ToString("f0");
    mpLine.fillAmount = hero.HeroPropetries.Mp / hero.HeroPropetries.Mmp;
    coIndicator.text = hero.HeroPropetries.Co.ToString("f0") + "/" + hero.HeroPropetries.Mco.ToString("f0");
    coLine.fillAmount = hero.HeroPropetries.Co / hero.HeroPropetries.Mco;
  }
}
