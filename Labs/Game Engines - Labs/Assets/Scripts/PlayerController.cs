using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public CharacterWithGunActions inputAction;
    public Rigidbody rigid;
    public GameObject cameraPivot;
    
    [Header("Motion")]
    public Vector2 v_Movement;
    public Vector2 v_Rotation;
    public float f_Sensitivity = 1.0f;
    public float f_MoveSpeed = 1.0f;
    public float f_JumpForce = 1.0f;
    public bool b_IsGrounded = false;
    public bool b_GroundCheck = true;
    private float f_DistanceToGround;
    
    [Header("Combat")]
    public GameObject projectilePrefab;
    public Transform projectileOrigin;


    private void OnEnable() 
    {
        inputAction.Player.Enable();
    }

    private void OnDisable() 
    {
        inputAction.Player.Disable();
    }

    private void Awake() 
    {
        inputAction = new CharacterWithGunActions();
        inputAction.Player.Move.performed += cntxt => v_Movement = cntxt.ReadValue<Vector2>();
        inputAction.Player.Move.canceled += cntxt => v_Movement = Vector2.zero;

        inputAction.Player.Look.performed += cntxt => v_Rotation = cntxt.ReadValue<Vector2>();
        inputAction.Player.Look.canceled += cntxt => v_Rotation = Vector2.zero;

        inputAction.Player.Jump.performed += cntxt => Jump();

        inputAction.Player.Fire.performed += cntxt => Shoot();

        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() 
    {
        transform.Translate(Vector3.forward * v_Movement.y * f_MoveSpeed * Time.deltaTime, Space.Self);
        transform.Translate(Vector3.right * v_Movement.x * f_MoveSpeed * Time.deltaTime, Space.Self);
        
        animator.SetFloat("Movement-X", v_Movement.x);
        animator.SetFloat("Movement-Z", v_Movement.y);

        transform.Rotate(new Vector3(0, v_Rotation.x * f_Sensitivity, 0));

        // Ground Check
        if (b_GroundCheck) 
        {
            int layerMask = 1 << 7; // Ground Layer
            if (Physics.Raycast(rigid.transform.position, transform.TransformDirection(-Vector3.up), out RaycastHit hit, 0.5f, layerMask))
            {
                if (!b_IsGrounded) {
                    b_IsGrounded = true;
                    animator.SetTrigger("Landed");
                }
                           
            } else {
                b_IsGrounded = false;
                
            }

            animator.SetBool("Falling", !b_IsGrounded);
        }
    }

    private void Jump() 
    {
        Debug.Log("Jump");

        if (b_IsGrounded) 
        {
            rigid.AddForce(transform.up * f_JumpForce, ForceMode.Impulse);            
            b_IsGrounded = false;
            animator.SetTrigger("Jump");
            StartCoroutine(DelayGroundCheck());            
        }        
    }

    private void Shoot() 
    {
        Debug.Log("Shoot");

        // Instantiate Projectile
        GameObject newProjectileObj = Instantiate(projectilePrefab, projectileOrigin.position, Quaternion.identity); 
        Rigidbody rbProjectile = newProjectileObj.GetComponent<Rigidbody>();
        rbProjectile.AddForce(transform.forward * 500.0f);
    }

    private IEnumerator DelayGroundCheck() 
    {
        b_GroundCheck = false;
        yield return new WaitForSeconds(1);
        b_GroundCheck = true;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Coin")
        {
            GameManager.GetInstance().AddPoint();
            Destroy(other.gameObject);
        }
    }

}
