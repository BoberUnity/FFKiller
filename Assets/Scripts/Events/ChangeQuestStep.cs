using UnityEngine;

public class ChangeQuestStep : MonoBehaviour
{
  [SerializeField] private Quest quest = null;
  [SerializeField] private int newValue = 1;

  public void OnEventAction()
  {
    if (quest != null)
      quest.CurrentStep = newValue;
    else
      Debug.LogWarning("Дмитрий! Объект " + gameObject.name + " quest не назначен!");
  }
}
