using UnityEngine;


public class Portal : MonoBehaviour
{
  [SerializeField] private Portal otherPortal = null;
  public Transform TargetPoint = null;
  private CharacterMoving characterMoving = null;
  [HideInInspector] public bool IsFinish = false;
  private CameraController cameraController = null;  
	
  private void Start ()
  {
    characterMoving = FindObjectOfType<CharacterMoving>();
    cameraController = FindObjectOfType<CameraController>();
    //enabled = false;
  }
	
	private void OnTriggerEnter2D(Collider2D other)
  {    
    //if (!IsFinish)
    //{
      characterMoving.CanMove = false;      
      cameraController.StartEffect();
      //otherPortal.IsFinish = true;
      Invoke("ChangePosition", cameraController.EffectTime);
    //}    
	}

  private void OnTriggerExit2D(Collider2D other)
  {
    //if (IsFinish)
    //{
    //  IsFinish = false;
    //}
  }

  private void ChangePosition()
  {
    characterMoving.transform.position = otherPortal.TargetPoint.transform.position;
    characterMoving.CanMove = true;
  }
}
