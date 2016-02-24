using UnityEngine;

public class OnTriggerActionAnimatorSetBool : MonoBehaviour
{
  [SerializeField] TriggerBase trigger = null;
  [SerializeField] private int numTrigger = 1;
  [SerializeField] private Animator thisAnimator = null;
  [SerializeField] private string paramName = "GoAway";
  [SerializeField] private bool newValue = true;

  private void Start ()
  {
    trigger.OnTriggerAction += OnTriggerAction;  
  }
	
	private void OnDestroy ()
  {
    trigger.OnTriggerAction -= OnTriggerAction;
  }

  private void OnTriggerAction(int currTrigger)
  {
    if (currTrigger == numTrigger)
    {
      thisAnimator.SetBool(paramName, newValue);
    }
  }
}
