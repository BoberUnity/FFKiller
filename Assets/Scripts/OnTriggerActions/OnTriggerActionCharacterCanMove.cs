using UnityEngine;

public class OnTriggerActionCharacterCanMove : MonoBehaviour
{
  [SerializeField] [Tooltip("block = true - блокировать управление персонажем, false - восстановить")] private bool block = false;
  [SerializeField] private int numTrigger = 1;
  [SerializeField] private float time = 0;
  TriggerBase thisTrigger = null;
  private CharacterMoving characterMoving = null;

  private void Start()
  {
    characterMoving = FindObjectOfType<CharacterMoving>();
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
      Invoke("BlockControl", time);
    }
  }

  private void BlockControl()
  {
    characterMoving.IsBlocked = block;
  }  
}
