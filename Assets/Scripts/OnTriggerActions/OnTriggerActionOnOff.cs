using UnityEngine;

public class OnTriggerActionOnOff : MonoBehaviour
{
  [SerializeField] GameObject targetObject = null;
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
      Invoke("SetObjectState", time);
    }
  }

  private void SetObjectState()
  {
    targetObject.SetActive(newState);
  }
}
