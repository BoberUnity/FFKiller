using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
  [SerializeField] private Text nameText = null;
  private HeroPropetries heroPropetries = null;

  public void Show(HeroPropetries hP)
  {
    heroPropetries = hP;
    nameText.text = heroPropetries.Name;
    GetComponent<Image>().sprite = heroPropetries.Portrait;
  }

  public void Hide()
  {
    nameText.text = "";
  }

  public void OnPress()
  {
    Hero hero = GameObject.FindObjectOfType<CharacterMoving>().GetComponent<Hero>();
    Use(hero);
  }

  private void Use(Hero hero)
  {
    HeroPropetries newHprop = new HeroPropetries();
    newHprop.Hp = hero.HeroPropetries.Hp + heroPropetries.Hp;
    newHprop.Mhp = hero.HeroPropetries.Mhp += heroPropetries.Mhp;
    newHprop.Mp = hero.HeroPropetries.Mp += heroPropetries.Mp;
    newHprop.Mmp = hero.HeroPropetries.Mmp += heroPropetries.Mmp;
    newHprop.Co = hero.HeroPropetries.Co += heroPropetries.Co;
    newHprop.Mco = hero.HeroPropetries.Mco += heroPropetries.Mco;
    hero.HeroPropetries = newHprop;
  }
}
