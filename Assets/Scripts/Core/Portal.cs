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
    characterMoving.KeyboardControl = false;      
    cameraController.StartEffect();
    Invoke("ChangePosition", 0.5f * cameraController.EffectTime);
    //DV{
    Invoke("SetMove", cameraController.EffectTime);
    //DV}
      if (doorAnimator != null)
      doorAnimator.SetTrigger("Open"); 
  }

  private void OnTriggerExit2D(Collider2D other)
  {

  }

  private void ChangePosition()
  {
    characterMoving.transform.position = otherPortal.TargetPoint.transform.position;
    //characterMoving.CanMove = true;
    //if (doorAnimator != null)
    //  doorAnimator.SetTrigger("Open");

    //DV{
    cameraController.TuneMap(TargetMap);
    //DV}
  }

    //DV{
    private void SetMove()
    {
        characterMoving.KeyboardControl = true;
    }
    //DV}
}