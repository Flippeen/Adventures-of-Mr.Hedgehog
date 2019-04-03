using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameController : MonoBehaviour
{
    Meny meny;
    Rigidbody rb;
    int maxSticks;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        meny = FindObjectOfType<Meny>();
        transform.position = playerSpawn.transform.position;
        spawnSticks();
        maxSticks = sticks.Count;
    }
    
    Quaternion lastRotation;
    float moveInput;
    float trickInput;
    float trickInput2;
    bool hitRight;
    bool hitLeft;
    [SerializeField] LayerMask riverFloor;
    RaycastHit hitInfo;
    RaycastHit hitInfo2;
    float timePassed;
    Quaternion groundRotation;
    [SerializeField] Transform rotateThis;
    [SerializeField] GameObject[] particleSystems;
    bool onGround;
    [SerializeField] Text score;
    void Update()
    {
        score.text = "Sticks: " + stickCounter.ToString() + " / 10";
        if (!noInputs)
        {
            moveInput = Input.GetAxis("Horizontal");
            trickInput = Input.GetAxis("Horizontal");
            trickInput2 = Input.GetAxis("Vertical");
        }

        if (Time.time > timePassed)
        {
            if(Physics.Raycast(transform.position, -transform.up, out hitInfo, 1.1f, riverFloor))
            {
                onGround = true;
                groundRotation = hitInfo.transform.rotation;
                Vector3 noraml = hitInfo.normal;
                transform.up = Vector3.RotateTowards(transform.up, noraml, 2.5f * Time.deltaTime, 0);

                if(hitInfo.transform.tag == "River")
                {
                    foreach (GameObject ps in particleSystems)
                    {
                        ps.SetActive(true);
                    }
                }
                else
                {
                    foreach (GameObject ps in particleSystems)
                    {
                        ps.SetActive(false);
                    }
                }

                if(hitInfo.transform.tag == "Tree")
                {
                    rotateThis.localRotation = Quaternion.RotateTowards(rotateThis.localRotation, Quaternion.Euler(0, 0, 0), 200 * Time.deltaTime);
                }

                timePassed = Time.time + 0.01f;
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(25, 0, 0), 40 * Time.deltaTime);
                rb.velocity += Vector3.down * 5 * Time.deltaTime;

                if (onGround)
                    StartCoroutine(Tick());
                else
                {
                    rotateThis.localRotation = Quaternion.RotateTowards(rotateThis.localRotation, Quaternion.Euler(0, rotateThis.localEulerAngles.y + trickInput * 15, 0), 600 * Time.deltaTime);
                }
            }
        }

        if(Input.GetAxisRaw("Horizontal") != 0 && Physics.Raycast(transform.position, -transform.up, out hitInfo, 1.1f, riverFloor))
        {
            if(moveInput > 0)
            {
                rotateThis.localRotation = Quaternion.RotateTowards(rotateThis.localRotation, Quaternion.Euler(3, 25 + 90, 0), 90 * Time.deltaTime);
            }
            else
            {
                rotateThis.localRotation = Quaternion.RotateTowards(rotateThis.localRotation, Quaternion.Euler(-3, -25 + 90, 0), 90 * Time.deltaTime);
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, -transform.up, out hitInfo2, 1.1f, riverFloor) && hitInfo2.transform.tag != "Tree")
                rotateThis.localRotation = Quaternion.RotateTowards(rotateThis.localRotation, Quaternion.Euler(0,90,0), 400 * Time.deltaTime);
        }
        
        if (rb.velocity.magnitude > 15)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, rb.velocity.z * 0.9f);
        }
    }
    IEnumerator Tick()
    {
        yield return new WaitForSeconds(0.2f);
        if (!Physics.Raycast(transform.position, -transform.up, out hitInfo, 1.1f, riverFloor))
        {
            onGround = false;
        }
    }
    bool noInputs;
    public IEnumerator NoInputTimer(float timer)
    {
        noInputs = true;
        yield return new WaitForSeconds(timer);
        moveInput = 0;
        noInputs = false;
        moveInput = 0;
    }
    [SerializeField] int moveSpeed;
    [SerializeField] int maxSpeedForward = 10;
    [SerializeField] int speedForwardIncrease = 20;
    void FixedUpdate()
    {
        if (noInputs)
            return;
        if (Physics.Raycast(transform.position, transform.right, out hitInfo, (transform.localScale.x / 2) + 0.01f, riverFloor))
        {
            hitRight = RayCastCollision(hitInfo);
        }
        else
        {
            hitRight = false;
        }
        if (Physics.Raycast(transform.position, -transform.right, out hitInfo, (transform.localScale.x / 2) + 0.01f, riverFloor))
        {
            hitLeft = RayCastCollision(hitInfo);
        }
        else
        {
            hitLeft = false;
        }
        Debug.DrawRay(transform.position, rb.velocity, Color.magenta);
        if (rb.velocity.magnitude < maxSpeedForward)
            rb.velocity += transform.forward * Time.deltaTime * speedForwardIncrease;
        if (!Physics.Raycast(transform.position, -transform.up, out hitInfo, 1.1f, riverFloor))
            return;
        if ((!hitLeft && moveInput < 0) || (!hitRight && moveInput > 0))
        {
            rb.velocity = new Vector3(moveInput * Time.deltaTime * moveSpeed, rb.velocity.y, rb.velocity.z);
        }
    }

    bool RayCastCollision(RaycastHit hitInfo)
    {
        Vector3 collidePos = hitInfo.transform.position;
        if (transform.position.x - collidePos.x > 0)
        {
            Debug.Log("Left");
            return true;
        }
        else if (transform.position.x - collidePos.x < 0)
        {
            Debug.Log("Right");
            return true;
        }
        return false;
    }

    int stickCounter;
    List<StickMoving> sticks = new List<StickMoving>();
    private void OnTriggerEnter(Collider c)
    {
        if(c.gameObject.tag == "Stick")
        {
            stickCounter++;
            sticks.Remove(c.GetComponent<StickMoving>());
            Destroy(c.gameObject, 0.1f);
        }
        if(c.gameObject.tag == "End")
        {
            if(stickCounter >= 10)
            {
                EndingOfLevel(true);
            }
            else
            {
                EndingOfLevel(false);
            }
        }
    }
    
    [SerializeField] Transform[] stickSpawns;
    [SerializeField] Transform playerSpawn;
    void ResetPositions()
    {
        while(sticks.Count > 0)
        {
            StickMoving stick = sticks[0];
            sticks.Remove(stick);
            Destroy(stick.gameObject);
        }
        stickCounter = 0;
        Image blackFade = endingCanvas.GetComponentInChildren<Image>();
        blackFade.enabled = false;
        spawnSticks();
    }
    [SerializeField] StickMoving[] stickPrefabs;
    void spawnSticks()
    {
        foreach (var stick in stickSpawns)
        {
            StickMoving newStick = Instantiate(stickPrefabs[Random.Range(0,stickPrefabs.Length)], stick.position, Quaternion.identity);
            sticks.Add(newStick);
        }
        rb.velocity = Vector3.zero;
        transform.position = playerSpawn.position;
    }

    [SerializeField] Canvas endingCanvas;
    public void EndingOfLevel(bool won)
    {
        Image blackFade = endingCanvas.GetComponentInChildren<Image>();
        blackFade.enabled = true;
        blackFade.color = new Vector4(0, 0, 0, 0);

        StartCoroutine(FadeScreen(blackFade, won));
    }

    [SerializeField] string mainMenu;
    DontDestroy dontDestroyObject;
    IEnumerator FadeScreen(Image blackFade, bool won)
    {
        while (blackFade.color.a < 1)
        {
            blackFade.color = new Vector4(0, 0, 0, blackFade.color.a + 0.02f);

            yield return new WaitForSeconds(0.05f);

            if (blackFade.color.a >= 0.95f)
            {
                dontDestroyObject = FindObjectOfType<DontDestroy>();
                if (dontDestroyObject != null)
                {
                    dontDestroyObject.ChangeName();
                }

                if (won)
                    meny.SwitchScene(mainMenu);
                else
                    ResetPositions();
            }
        }
    }
}
