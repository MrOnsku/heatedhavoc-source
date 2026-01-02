using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothingSpeed;

    private Vector3 targetPos;

    private void LateUpdate()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            if (!PlayerController.instance.dead)
            {
                Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

                targetPos = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
            }
            else
            {
                targetPos = new Vector3(PlayerController.instance.deadCamPos.x, PlayerController.instance.deadCamPos.y, -10);
            }
        }

        transform.position = Vector3.Slerp(transform.position, targetPos, smoothingSpeed * Time.deltaTime);
    }
}