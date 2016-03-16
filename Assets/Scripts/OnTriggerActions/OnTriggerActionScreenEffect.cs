using UnityEngine;
// Запускает эффект затемнения экрана указанным цветом на заданный интервал.
public class OnTriggerActionScreenEffect : MonoBehaviour
{
  [SerializeField] private Color effectColor = Color.black;
  [SerializeField] private float effectTime = 2;
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
      Invoke("SetScreenEffect", time);
    }
  }

  private void SetScreenEffect()
  {
    CameraController cameraController = FindObjectOfType<CameraController>();
    if (cameraController != null)
      cameraController.StartEffect(effectColor, effectTime);
  }
}
