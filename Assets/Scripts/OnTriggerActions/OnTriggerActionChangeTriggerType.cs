using UnityEngine;

public class OnTriggerActionChangeTriggerType : MonoBehaviour
{
  [SerializeField] private TriggerBase targetTrigger = null;
  [SerializeField] private TriggerType newType = TriggerType.Disabled;
  [SerializeField] private int numTrigger = 1;
  [SerializeField] private float time = 0;
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
      Invoke("SetNewType", time);
    }
  }

  private void SetNewType()
  {
    targetTrigger.Type = newType;
  }
}
