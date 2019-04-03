using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform playerPivot;
    private Animator charAnim;

    Rigidbody rb;
    void Start()
    {
        charAnim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    Vector3 moveInput;
    Vector3 lookInputCap;
    [SerializeField] Transform cam;
    Vector3 camR;
    Vector3 camF;
    RaycastHit hitInfo;
    [SerializeField] LayerMask groundLayer;
    bool walking = false;
    void Update()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        lookInputCap = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (!walking && moveInput.magnitude != 0 && !freezePlayer)
        {
            walking = true;
            charAnim.SetBool("Walking", true);
        }
        else if (walking && moveInput.magnitude == 0)
        {
            walking = false;
            charAnim.SetBool("Walking", false);
        }
        camF = cam.forward;
        camR = cam.right;
        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        if (!freezePlayer && Physics.Raycast(playerPivot.position, Vector3.down, 1.1f, groundLayer) && Input.GetKeyDown(KeyCode.Space))
        {
            if (rb.velocity.y > 0)
                rb.velocity = new Vector3(rb.velocity.x, -rb.velocity.y * 1.1f, rb.velocity.z);
            rb.AddForce(Vector3.up * 7, ForceMode.Impulse);
            charAnim.SetTrigger("Jump");
        }           
    }
    bool freezePlayer;

    public bool FreezePlayerMovement(bool shouldIFreeze, Transform lookAtCharacter)
    {
        freezePlayer = shouldIFreeze;
        if (lookAtCharacter != null)
            transform.LookAt(new Vector3(lookAtCharacter.position.x, transform.position.y, lookAtCharacter.position.z));
        return freezePlayer;
    }
    [SerializeField] float moveSpeed = 300, turnSpeed = 5, maxSpeed = 5;
    float fallmulti = 2.5f;
    float lowFallMulti = 2;
    [SerializeField] LayerMask slipperyWalls;
    private void FixedUpdate()
    {
        if (freezePlayer)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return;
        }

        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * Physics.gravity.y * (fallmulti - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space))
            rb.velocity += Vector3.up * Physics.gravity.y * (lowFallMulti - 1) * Time.deltaTime;

        if (Physics.Raycast(transform.position, (camF * moveInput.z + camR * moveInput.x), 0.8f, slipperyWalls))
            return;

        if (lookInputCap.normalized != new Vector3(0.0f, 0.0f, 0.0f))
        {
            Quaternion rotation = Quaternion.LookRotation((camF * moveInput.z + camR * moveInput.x).normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, turnSpeed);
            Debug.DrawRay(transform.position, (camF * moveInput.z + camR * moveInput.x), Color.red);
        }
        Vector3 Yvelocity = new Vector3(0, rb.velocity.y, 0);
        if (lookInputCap.x != 0 || lookInputCap.z != 0)
            rb.velocity = (camF * moveInput.z + camR * moveInput.x).normalized * Time.deltaTime * moveSpeed + Yvelocity;
        else
            rb.velocity = new Vector3(rb.velocity.x * 0.85f, rb.velocity.y, rb.velocity.z * 0.85f);

        if (moveInput == new Vector3(0, 0, 0))
            rb.velocity = rb.velocity * 0.94f; 
    }
}
