using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public float rubberbandSpeed;

    private void Update()
    {
        if (GameManager.instance.gameRunning)
        {
            //transform.Translate(Vector3.up * speed * Time.deltaTime);

            Vector2 targetPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

            if (transform.position.y < targetPosition.y)
            {
                var y = Mathf.Lerp(transform.position.y, targetPosition.y, rubberbandSpeed * Time.deltaTime);

                transform.position = new Vector2(transform.position.x, y);
            }
        }
    }
}