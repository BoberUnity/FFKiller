using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
  [SerializeField] private Text nameText = null;
  [HideInInspector] public bool IsBusy = false;
  private Inventar inventar = null;
  private ThingPropetries thingPropetries = null;
  private Image thisImage = null;

  private void Start()
  {
    inventar = FindObjectOfType<Inventar>();
  }

  public void Load(ThingPropetries tPropetries)
  {
    thingPropetries = tPropetries;
    nameText.text = thingPropetries.Name;    
    thisImage = GetComponent<Image>();
    thisImage.sprite = thingPropetries.Portrait;
    thisImage.color = Color.white;
    IsBusy = true;
  }
  
  public void OnPress()
  {
    Hero hero = GameObject.FindObjectOfType<CharacterMoving>().GetComponent<Hero>();
    Use(hero);
  }

  private void Use(Hero hero)
  {
    if (IsBusy)
    {
      HeroPropetries newHprop = new HeroPropetries();
      newHprop.Portrait = hero.HeroPropetries.Portrait;
      newHprop.Hp = hero.HeroPropetries.Hp + thingPropetries.Hp;
      newHprop.Mhp = hero.HeroPropetries.Mhp += thingPropetries.Mhp;
      newHprop.Mp = hero.HeroPropetries.Mp += thingPropetries.Mp;
      newHprop.Mmp = hero.HeroPropetries.Mmp += thingPropetries.Mmp;
      newHprop.Co = hero.HeroPropetries.Co += thingPropetries.Co;
      newHprop.Mco = hero.HeroPropetries.Mco += thingPropetries.Mco;
      hero.HeroPropetries = newHprop;
      nameText.text = "";
      thisImage.sprite = null;
      IsBusy = false;
      thisImage.color = new Color(1, 1, 1, 0.04f);
    }
  }

  public void ShowDescription()
  {
    if (IsBusy)
      inventar.DescriptionField.text = thingPropetries.Description;
  }

  public void HideDescription()
  {
    inventar.DescriptionField.text = "";
  }
}
