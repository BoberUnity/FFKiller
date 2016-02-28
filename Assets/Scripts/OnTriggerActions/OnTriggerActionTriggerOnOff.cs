using UnityEngine;

public class OnTriggerActionTriggerOnOff : MonoBehaviour
{
  [SerializeField] TriggerBase targetTrigger = null;
  [SerializeField] private int numTrigger = 1;
  [SerializeField] private bool newState = false;
  TriggerBase thisTrigger = null;

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
      targetTrigger.gameObject.SetActive(newState);
    }
  }
}
