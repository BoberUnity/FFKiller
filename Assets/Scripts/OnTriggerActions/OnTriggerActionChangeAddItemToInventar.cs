using UnityEngine;

public class OnTriggerActionChangeAddItemToInventar : MonoBehaviour
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
      Invoke("AddItem", time);
    }
  }

  private void AddItem()
  {
    FindObjectOfType<Inventar>().AddItem(heroPropetries);    
  }
}
