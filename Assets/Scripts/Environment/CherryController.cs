
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CherryController : MonoBehaviour
{
    [SerializeField] private GameObject cherryPrefab;

    private Tweener tweener;
    private float timeSinceLastSpawn = 0f;
    
    private const float SpawnInterval = 10f;

    private readonly Vector2 centerPos = new Vector2(13.5f, -14.5f);
    
    [Header("Camera Out bound")]
    private int minX;
    private int maxX;
    private int minY;
    private int maxY;

    private bool canSpawn = false;

    private void Awake()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        Vector2 camPos = cam.transform.position;
        minX = Mathf.FloorToInt(camPos.x - width / 2);
        maxX = Mathf.CeilToInt(camPos.x + width / 2);
        minY = Mathf.FloorToInt(camPos.y - height / 2);
        maxY = Mathf.CeilToInt(camPos.y + height / 2);
    }

    private void Start()
    {
        GameManager.Instance.OnPlayGame += (() => canSpawn = true);
        GameManager.Instance.OnPausedGame += (() => canSpawn = false);
    }

    private void Update()
    {
        if (!canSpawn) return;
        SpawnCherry();
    }

    private void SpawnCherry()
    {
        timeSinceLastSpawn -= Time.deltaTime;
        if (timeSinceLastSpawn <= 0f)
        {
            timeSinceLastSpawn = SpawnInterval;
            Vector2 spawnPos = GetSpawnPosition();
            
            GameObject cherry = Instantiate(cherryPrefab, spawnPos, Quaternion.identity);
            CherryMovement movement = cherry.GetComponent<CherryMovement>();
            movement.SetDirection(centerPos - spawnPos);
            movement.SetCameraBound(minX, maxX, minY, maxY);
        }
    }

    private Vector2 GetSpawnPosition()
    {
        if (Random.Range(0, 2) == 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                return new Vector2(minX, Random.Range(minY, maxY));
            }
            else return new Vector2(maxX, Random.Range(minY, maxY));
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                return new Vector2(Random.Range(minX, maxX), minY);
            }
            else return new Vector2(Random.Range(minX, maxX), maxY);
        }
    }
}
