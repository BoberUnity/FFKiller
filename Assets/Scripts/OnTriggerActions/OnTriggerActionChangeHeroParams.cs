using UnityEngine;

public class OnTriggerActionChangeHeroParams : MonoBehaviour
{
  [SerializeField] private int numTrigger = 1;
  [SerializeField] private float time = 0;
  [SerializeField] private HeroPropetries heroPropetries = null;  
  
  private TriggerBase thisTrigger = null;
  private Hero hero = null;

  private void Start ()
  {
    thisTrigger = GetComponent<TriggerBase>();
    thisTrigger.OnTriggerAction += OnTriggerAction;  
  }

  private void OnDestroy()
  {
    thisTrigger.OnTriggerAction -= OnTriggerAction;
  }

  private void OnTriggerAction(int currTrigger)
  {
    if (currTrigger == numTrigger)
    {
      Invoke("ChangeHeroParams", time);
    }
  }

  private void ChangeHeroParams()
  {
    hero.HeroPropetries.Hp += heroPropetries.Hp;
    hero.HeroPropetries.Mhp += heroPropetries.Mhp;
    hero.HeroPropetries.Mp += heroPropetries.Mp;
    hero.HeroPropetries.Mmp += heroPropetries.Mmp;
    hero.HeroPropetries.Co += heroPropetries.Co;
    hero.HeroPropetries.Mco += heroPropetries.Mco;
  }
}
