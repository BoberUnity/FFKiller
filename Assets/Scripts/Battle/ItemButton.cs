using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
  [SerializeField] private Text nameText = null;
  [HideInInspector] public bool IsBusy = false;
  private HeroPropetries heroPropetries = null;
  private Image thisImage = null;

  public void Show(HeroPropetries hP)
  {
    heroPropetries = hP;
    nameText.text = heroPropetries.Name;
    thisImage = GetComponent<Image>();
    thisImage.sprite = heroPropetries.Portrait;
    thisImage.color = Color.white;
    IsBusy = true;
  }

  public void Hide()
  {
    nameText.text = "";
    GetComponent<Image>().sprite = null;
    heroPropetries = new HeroPropetries();
    heroPropetries.Portrait = null;
    heroPropetries.Hp = 0; ;
    heroPropetries.Mhp = 0;
    heroPropetries.Mp = 0;
    heroPropetries.Mmp = 0;
    heroPropetries.Co = 0;
    heroPropetries.Mco = 0;
    IsBusy = false;
    thisImage.color = new Color(1, 1, 1, 10/255);
  }

  public void OnPress()
  {
    Hero hero = GameObject.FindObjectOfType<CharacterMoving>().GetComponent<Hero>();
    Use(hero);
  }

  private void Use(Hero hero)
  {
    HeroPropetries newHprop = new HeroPropetries();
    newHprop.Portrait = hero.HeroPropetries.Portrait;
    newHprop.Hp = hero.HeroPropetries.Hp + heroPropetries.Hp;
    newHprop.Mhp = hero.HeroPropetries.Mhp += heroPropetries.Mhp;
    newHprop.Mp = hero.HeroPropetries.Mp += heroPropetries.Mp;
    newHprop.Mmp = hero.HeroPropetries.Mmp += heroPropetries.Mmp;
    newHprop.Co = hero.HeroPropetries.Co += heroPropetries.Co;
    newHprop.Mco = hero.HeroPropetries.Mco += heroPropetries.Mco;
    hero.HeroPropetries = newHprop;
    Hide();   
  }
}
