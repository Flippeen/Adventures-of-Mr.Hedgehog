﻿using System.Collections;
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
    RaycastHit hitInfo;
    [SerializeField] AudioClip[] bearSounds;
    bool canSeePlayer;
    IEnumerator CheckForPlayer()
    {
        aS.clip = bearSounds[0];
        aS.Play();
        yield return new WaitForSeconds(bearSounds[0].length);
        
        aS.clip = bearSounds[1];
        aS.Play();
        while (aS.isPlaying)
        {
            if (Physics.Raycast(transform.position, playerPos.position - transform.position, out hitInfo, 100) &&
                hitInfo.transform != null && !canSeePlayer)
            {
                canSeePlayer = true;
                Debug.Log("htihthrth");
                StartCoroutine(BlowHimAway(hitInfo));
            }
            else
            {
                canSeePlayer = false;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(bearSounds[1].length);

        yield return new WaitForSeconds(3);
        StartCoroutine(CheckForPlayer());
    }
    IEnumerator BlowHimAway(RaycastHit hit)
    {
        yield return new WaitForSeconds(0.3f);
        //hit.transform.gameObject.GetComponent<PlayerMovement>().FreezePlayerMovement(true, transform);
        rb.velocity = Vector3.zero;
        while (Vector3.Distance(playerPos.position, transform.position) < 100)
        {
            if (!aS.isPlaying)
            {
                aS.clip = bearSounds[1];
                aS.Play();
            }
            rb.velocity += (playerPos.position - transform.position).normalized * 100 * Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
        if(Vector3.Distance(playerPos.position, transform.position) >= 100)
        {
            //hit.transform.GetComponent<PlayerMovement>().FreezePlayerMovement(false, transform);
        }
    }
}
