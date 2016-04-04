using UnityEngine;
using UnityEngine.UI;

public class Status : HeroUI
{
  [SerializeField] private Text atkIndicator = null;
  [SerializeField] private Text defIndicator = null;
  [SerializeField] private Text matIndicator = null;
  [SerializeField] private Text mdfIndicator = null;
  [SerializeField] private Text agiIndicator = null;

  public override void UpdateUI ()
  {
    base.UpdateUI();
    atkIndicator.text = hero.HeroPropetries.Atk.ToString();
    defIndicator.text = hero.HeroPropetries.Def.ToString();
    matIndicator.text = hero.HeroPropetries.Mat.ToString();
    mdfIndicator.text = hero.HeroPropetries.Mdf.ToString();
    agiIndicator.text = hero.HeroPropetries.Agi.ToString();
  }  
}
