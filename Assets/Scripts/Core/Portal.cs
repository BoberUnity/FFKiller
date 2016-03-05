using UnityEngine;


public class Portal : MonoBehaviour
{
  [SerializeField] private Portal otherPortal = null;
  public Transform TargetPoint = null;
  [SerializeField] private Animator doorAnimator = null;
  private CharacterMoving characterMoving = null;
  [HideInInspector] public bool IsFinish = false;
  private CameraController cameraController = null;
    //DV{
    public GameObject TargetMap;
    //DV}

    private void Start ()
  {
    characterMoving = FindObjectOfType<CharacterMoving>();
    cameraController = FindObjectOfType<CameraController>();    
  }
	
	private void OnTriggerEnter2D(Collider2D other)
  {    
    characterMoving.CanMove = false;      
    cameraController.StartEffect();
    Invoke("ChangePosition", cameraController.EffectTime);
    if (doorAnimator != null)
      doorAnimator.SetTrigger("Open"); 
  }

  private void OnTriggerExit2D(Collider2D other)
  {

  }

  private void ChangePosition()
  {
    characterMoving.transform.position = otherPortal.TargetPoint.transform.position;
    characterMoving.CanMove = true;
    //if (doorAnimator != null)
    //  doorAnimator.SetTrigger("Open");

    //DV{
    cameraController.Map = TargetMap;
    cameraController.TuneMap();
    //DV}
  }
}