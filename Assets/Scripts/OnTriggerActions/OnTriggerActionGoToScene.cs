using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTriggerActionGoToScene : MonoBehaviour
{  
  [SerializeField] private int numTrigger = 1;
  [SerializeField] int sceneIndex = 1;
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
      SceneManager.LoadScene(sceneIndex);
    }
  }  
}
