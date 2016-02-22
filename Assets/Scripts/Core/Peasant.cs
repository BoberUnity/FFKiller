using UnityEngine;

public class Peasant : Dialog
{
  [SerializeField] private Animator bodyAnimator = null;
  protected Animator thisAnimator = null;
  private Vector3 previousPosition = Vector3.zero;

  protected override void Start()
  {
    base.Start();
    previousPosition = transform.position;
    thisAnimator = GetComponent<Animator>();
    thisAnimator.speed = Random.value * 0.5f + 0.5f;
  }

  protected override void StartDialog()
  {
    base.StartDialog();
    bodyAnimator.SetBool("IsMoving", false);
    bodyAnimator.SetBool("Running", false);
    thisAnimator.speed = 0;
  }

  protected override void EndDialog()
  {
    base.EndDialog();
    //bodyAnimator.SetBool("IsMoving", true);
    //bodyAnimator.SetBool("Running", true);
    //thisAnimator.speed = Random.value * 0.5f + 0.5f;    
  }

  protected override void Update()
  {
    base.Update();
    if (transform.position.x - previousPosition.x > 0)
      bodyAnimator.SetInteger("Direction", 1);
    if (transform.position.x - previousPosition.x < 0)
      bodyAnimator.SetInteger("Direction", 3);
    bodyAnimator.SetFloat("SpeedX", transform.position.x - previousPosition.x);
    bodyAnimator.SetFloat("SpeedY", transform.position.y - previousPosition.y);
    bodyAnimator.SetBool("Running", previousPosition != transform.position);
    bodyAnimator.SetBool("IsMoving", previousPosition != transform.position);
    previousPosition = transform.position;    
  }

  protected override void OnCharacterTriggerEnter()
  {
    thisAnimator.speed = 0;
  }

  protected override void OnCharacterTriggerExit()
  {
    thisAnimator.speed = 1;
  }  
}
