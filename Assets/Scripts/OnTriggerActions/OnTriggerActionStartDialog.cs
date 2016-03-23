using UnityEngine;

public class OnTriggerActionStartDialog : MonoBehaviour
{
  [SerializeField] private TriggerBase targetTrigger;
  [SerializeField] private int numTrigger = 1;
  [SerializeField] private float time = 0;
  TriggerBase thisTrigger = null;

  private void Start()
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
      Invoke("StartDialog", time);
    }
  }

  private void StartDialog()
  {
    targetTrigger.StartDialog();
    targetTrigger.SetAutoNextDialog();
  }
}
