using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    public static Bed instance;

    public GameObject zPrefab;
    public Transform zSpawnPos;

    public SpriteRenderer sprite;
    public Sprite emptyBed;

    private void Start()
    {
        instance = this;

        Instantiate(zPrefab, zSpawnPos.position, Quaternion.identity);

        StartCoroutine("SpawnZ");
    }

    private void Update()
    {
        if (GameManager.instance.firstCutscene)
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator SpawnZ()
    {
        yield return new WaitForSeconds(3);

        Instantiate(zPrefab, zSpawnPos.position, Quaternion.identity);

        StartCoroutine("SpawnZ");
    }
}