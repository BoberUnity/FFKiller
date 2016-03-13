using UnityEngine;

public class OnTriggerActionConnectToParty : MonoBehaviour
{
  [SerializeField] TriggerBase trigger = null;
  [SerializeField] private int numTrigger = 1;

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
      FindObjectOfType<Party>().Connect(gameObject.name);
      Destroy(gameObject);    
    }
  }
}
