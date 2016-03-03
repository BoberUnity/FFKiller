using UnityEngine;

public class OnTriggerActionAnimatorSetBool : MonoBehaviour
{
  [SerializeField] TriggerBase trigger = null;
  [SerializeField] private int numTrigger = 1;
  private Animator thisAnimator = null;
  [SerializeField] private string paramName = "GoAway";
  [SerializeField] private bool newValue = true;
  [SerializeField] private float time = 0;

  private void Start ()
  {
    trigger.OnTriggerAction += OnTriggerAction;
    thisAnimator = GetComponent<Animator>();
  }
	
	private void OnDestroy ()
  {
    trigger.OnTriggerAction -= OnTriggerAction;
  }

  private void OnTriggerAction(int currTrigger)
  {
    if (currTrigger == numTrigger)
    {
      Invoke("SetAnimatorBool", time);
    }
  }

  private void SetAnimatorBool()
  {
    thisAnimator.SetBool(paramName, newValue);
  }
}
