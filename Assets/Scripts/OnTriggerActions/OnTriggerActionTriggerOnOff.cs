using UnityEngine;

public class OnTriggerActionTriggerOnOff : MonoBehaviour
{
  [SerializeField] TriggerBase trigger = null;
  [SerializeField] private int numTrigger = 1;
  [SerializeField] private bool newState = false;

  private void Start ()
  {
    trigger.OnTriggerAction += OnTriggerAction;  
  }

  private void OnDestroy()
  {
    trigger.OnTriggerAction -= OnTriggerAction;
  }

  private void OnTriggerAction(int currTrigger)
  {
    if (currTrigger == numTrigger)
    {
      trigger.gameObject.SetActive(newState);
    }
  }
}
