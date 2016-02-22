using UnityEngine;

public class Klaus : Dialog
{
  [SerializeField] private Animator bodyAnimator = null;
  private Animator thisAnimator = null;
  private Vector3 previousPosition = Vector3.zero;

  protected override void Start()
  {
    base.Start();
    previousPosition = transform.position;
    thisAnimator = GetComponent<Animator>();
  }

  protected override void StartDialog()
  {
    base.StartDialog();
    bodyAnimator.SetBool("Running", false);
    thisAnimator.speed = 0;
    Debug.Log("Klaus start dialog");
  }

  protected override void EndDialog()
  {
    base.EndDialog();
    //bodyAnimator.SetBool("Running", true);
    //thisAnimator.speed = 1;
    //thisAnimator.SetBool("GoAway", true);
    //Debug.Log("Klaus end dialog");
  }

  protected override void Update()
  {
    base.Update();
    bodyAnimator.SetFloat("SpeedX", transform.position.x - previousPosition.x);
    bodyAnimator.SetFloat("SpeedY", transform.position.y - previousPosition.y);
    bodyAnimator.SetBool("Running", transform.position != previousPosition);
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
