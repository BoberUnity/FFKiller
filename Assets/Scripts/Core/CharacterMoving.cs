using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterMoving : MonoBehaviour
{
  public Sprite Portrait = null;
  [SerializeField] private float speedWalk = 1;
  [SerializeField] private float speedRun = 1;
  private float currentSpeed = 0;
  private Animator thisAnimator = null;
  [HideInInspector] public bool CanMove = true;

	private void Start ()
  {
    thisAnimator = GetComponent<Animator>();
  }
	
	private	void FixedUpdate ()
  {
    if (CanMove)
    {
      currentSpeed = Input.GetKey(KeyCode.RightShift) ? speedRun : speedWalk;
      if (Input.GetKey(KeyCode.UpArrow))
      {
        transform.position += Vector3.up * Time.fixedDeltaTime * currentSpeed;
        thisAnimator.SetBool("Running", true);
        thisAnimator.SetFloat("SpeedY", 1);
        thisAnimator.SetFloat("SpeedX", 0);
      }

      if (Input.GetKey(KeyCode.DownArrow))
      {
        transform.position -= Vector3.up * Time.fixedDeltaTime * currentSpeed;
        thisAnimator.SetBool("Running", true);
        thisAnimator.SetFloat("SpeedY", -1);
        thisAnimator.SetFloat("SpeedX", 0);
      }
      if (Input.GetKey(KeyCode.RightArrow))
      {
        transform.position += Vector3.right * Time.fixedDeltaTime * currentSpeed;
        thisAnimator.SetBool("Running", true);
        thisAnimator.SetFloat("SpeedX", 1);
        thisAnimator.SetFloat("SpeedY", 0);
      }
      if (Input.GetKey(KeyCode.LeftArrow))
      {
        transform.position -= Vector3.right * Time.fixedDeltaTime * currentSpeed;
        thisAnimator.SetBool("Running", true);
        thisAnimator.SetFloat("SpeedX", -1);
        thisAnimator.SetFloat("SpeedY", 0);
      }
    }

    if ((!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow)) || !CanMove)
        thisAnimator.SetBool("Running", false);    
  }
}
