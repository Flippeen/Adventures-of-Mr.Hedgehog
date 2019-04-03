using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorisController : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    AudioSource aS;
    Rigidbody rb;
    void Start()
    {
        rb = playerPos.GetComponent<Rigidbody>();
        aS = GetComponent<AudioSource>();
        StartCoroutine(CheckForPlayer());
    }
    bool roaring;
    bool canSeePlayer;
    void Update()
    {
        if (roaring && aS.isPlaying && !canSeePlayer)
        {
            if (Physics.Raycast(transform.position, playerPos.position - transform.position, out hitInfo, 100) &&
            hitInfo.transform.tag == "Player")
            {
                canSeePlayer = true;
                StartCoroutine(BlowHimAway(hitInfo));
            }
        }
    }

    RaycastHit hitInfo;
    [SerializeField] AudioClip[] bearSounds;
    IEnumerator CheckForPlayer()
    {
        aS.clip = bearSounds[0];
        aS.Play();
        yield return new WaitForSeconds(bearSounds[0].length);
        aS.Stop();

        roaring = true;
        aS.clip = bearSounds[1];
        aS.Play();
        yield return new WaitForSeconds(bearSounds[1].length);
        roaring = false;

        yield return new WaitForSeconds(3);
        StartCoroutine(CheckForPlayer());
    }
    IEnumerator BlowHimAway(RaycastHit hit)
    {
        yield return new WaitForSeconds(0.3f);
        hit.transform.GetComponent<PlayerMovement>().FreezePlayerMovement(true, transform);
        rb.velocity = Vector3.zero;
        while (Vector3.Distance(playerPos.position, transform.position) < 100)
        {
            if (!aS.isPlaying)
            {
                aS.clip = bearSounds[1];
                aS.Play();
            }
            rb.velocity += (playerPos.position - transform.position).normalized * 50 * Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
        if(Vector3.Distance(playerPos.position, transform.position) >= 100)
        {
            hit.transform.GetComponent<PlayerMovement>().FreezePlayerMovement(false, transform);
            roaring = false;
            canSeePlayer = false;
        }
    }
}
