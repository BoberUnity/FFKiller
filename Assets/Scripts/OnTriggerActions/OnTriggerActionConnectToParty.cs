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
      GetComponent<Animator>().enabled = false;
      FindObjectOfType<Party>().Connect(transform);
      BoxCollider2D[] boxColliders2D = GetComponents<BoxCollider2D>();
      foreach (var boxCollider2D in boxColliders2D)
      {
        if (!boxCollider2D.isTrigger)
          boxCollider2D.enabled = false;
      }
    }
  }
}
