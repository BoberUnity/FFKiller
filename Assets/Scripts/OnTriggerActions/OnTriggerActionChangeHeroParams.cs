using UnityEngine;

public class OnTriggerActionChangeHeroParams : MonoBehaviour
{
  [SerializeField] private int numTrigger = 1;
  [SerializeField] private float time = 0;
  [SerializeField] private float hp = 0;
  [SerializeField] private float mhp = 0;  
  
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
    hero.Hp += hp;
    hero.Mhp += mhp;
  }
}
