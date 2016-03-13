using UnityEngine;

public class OnTriggerActionComponentOnOff : MonoBehaviour
{
  [SerializeField] Behaviour targetComponent = null;
  [SerializeField] private int numTrigger = 1;
  [SerializeField] private bool newState = false;
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
      Invoke("SetComponentState", time);
    }
  }

  private void SetComponentState()
  {
    targetComponent.enabled = newState;      
  }
}
