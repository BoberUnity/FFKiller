using UnityEngine;

public class OnTriggerActionSetOtherTrigger : MonoBehaviour
{  
  [SerializeField] [Tooltip("Значение текущего триггера, при котором сработает переключение")] private int numTrigger = 1;
  [SerializeField] [Tooltip("Интервал в секундах")] private float time = 0;
  [SerializeField] [Tooltip("Триггер, который нужно переключить")] TriggerBase targetTrigger = null;
  [SerializeField] [Tooltip("Значение устанавливаемое триггеру")] private int newState = 0;  
  [SerializeField] [Tooltip("Запускать ли на триггере EndDialog")] private bool runEndDialog = false;
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
      Invoke("SetTrigger", time);
    }
  }

  private void SetTrigger()
  {
    targetTrigger.CurrentTrigger = newState;
    if (runEndDialog)
      targetTrigger.EndDialog();
  }
}
