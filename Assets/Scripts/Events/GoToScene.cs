using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
  [SerializeField] private string sceneName = "scene name";

  public void OnEventAction()
  {
    SceneManager.LoadScene(sceneName);
  }
}
