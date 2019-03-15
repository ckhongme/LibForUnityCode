using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Third Preson View, the camera will follow the player
/// </summary>
public class CamFollowPlayer : MonoBehaviour
{
    public float smoothSpeed = 1.5f;         
    public Transform player;            
    private Vector3 camPos;             //摄像机相对于角色的位置

    void Awake()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        camPos = transform.position - player.position;
    }

    void FixedUpdate()
    {
        FollowMove();
        SmoothLookAt();
    }

    void FollowMove()
    {
        Vector3 standardPos = player.position + camPos;
        transform.position = Vector3.Lerp(transform.position, standardPos, smoothSpeed * Time.deltaTime);
    }

    void SmoothLookAt()
    {
        Vector3 currPlayerPos = player.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(currPlayerPos, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smoothSpeed * Time.deltaTime);
    }
}
