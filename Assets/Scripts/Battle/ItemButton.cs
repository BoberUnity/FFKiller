using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
  [SerializeField] private Text nameText = null;
  [SerializeField] private Text countText = null;
  [HideInInspector] public bool IsBusy = false;
  private Inventar inventar = null;
  [HideInInspector] public ThingPropetries ThingPropetries = new ThingPropetries();
  private Image thisImage = null;

  private void Start()
  {
    inventar = FindObjectOfType<Inventar>();
  }

  public void Load(ThingPropetries tPropetries)
  {
    ThingPropetries.Name = tPropetries.Name;
    ThingPropetries.Portrait = tPropetries.Portrait;
    ThingPropetries.Description = tPropetries.Description;
    ThingPropetries.Count = tPropetries.Count;
    ThingPropetries.Hp = tPropetries.Hp;
    ThingPropetries.Mhp = tPropetries.Mhp;
    ThingPropetries.Mp = tPropetries.Mp;
    ThingPropetries.Mmp = tPropetries.Mmp;
    ThingPropetries.Cr = tPropetries.Cr;
    ThingPropetries.Mcr = tPropetries.Mcr;
    ThingPropetries.Atk = tPropetries.Atk;
    ThingPropetries.Def = tPropetries.Def;
    ThingPropetries.Mat = tPropetries.Mat;
    ThingPropetries.Mdf = tPropetries.Mdf;
    nameText.text = ThingPropetries.Name;
    countText.text = ThingPropetries.Count.ToString();
    thisImage = GetComponent<Image>();
    thisImage.sprite = ThingPropetries.Portrait;
    thisImage.color = Color.white;
    IsBusy = true;
  }

  public void UpdateCount(int count)
  {
    ThingPropetries.Count += count;
    countText.text = ThingPropetries.Count.ToString();
  }
  
  public void OnPress()
  {
    inventar.ShowHeroes();
    //Hero hero = GameObject.FindObjectOfType<CharacterMoving>().GetComponent<Hero>();
    //Use(hero);
  }

  private void Use(Hero hero)
  {
    if (IsBusy)
    {
      HeroPropetries newHprop = new HeroPropetries();
      newHprop.Portrait = hero.HeroPropetries.Portrait;
      newHprop.Hp = hero.HeroPropetries.Hp + ThingPropetries.Hp;
      newHprop.Mhp = hero.HeroPropetries.Mhp += ThingPropetries.Mhp;
      newHprop.Mp = hero.HeroPropetries.Mp += ThingPropetries.Mp;
      newHprop.Mmp = hero.HeroPropetries.Mmp += ThingPropetries.Mmp;
      newHprop.Cr = hero.HeroPropetries.Cr += ThingPropetries.Cr;
      newHprop.Mcr = hero.HeroPropetries.Mcr += ThingPropetries.Mcr;
      newHprop.Atk = hero.HeroPropetries.Atk += ThingPropetries.Atk;
      newHprop.Def = hero.HeroPropetries.Def += ThingPropetries.Def;
      newHprop.Mat = hero.HeroPropetries.Mat += ThingPropetries.Mat;
      newHprop.Mdf = hero.HeroPropetries.Mdf += ThingPropetries.Mdf;
      hero.HeroPropetries = newHprop;
      UpdateCount(-1);
      if (ThingPropetries.Count == 0)
      {        
        ThingPropetries = null;
        nameText.text = "";
        countText.text = "";
        thisImage.sprite = null;
        IsBusy = false;
        thisImage.color = new Color(1, 1, 1, 0.04f);
      }      
    }
  }

  public void ShowDescription()
  {
    if (IsBusy)
      inventar.DescriptionField.text = ThingPropetries.Description;
  }

  public void HideDescription()
  {
    inventar.DescriptionField.text = "";
  }
}
