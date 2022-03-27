using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    public GameObject treeObstacle;
    public GameObject coin;

    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnObstacle();
        SpawnCoin();
    }

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile();
        Destroy(gameObject, 2); // 2 second after exit destroy it.
    }
    
    void SpawnObstacle()
    {
        //choose a random point to spawn the obstacle
        //2 is included but 5 not and they represent the middle, left and right;
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
        //Spawn the obstecale at the position
        Instantiate(treeObstacle, spawnPoint.position, Quaternion.identity, transform);
    }

    void SpawnCoin()
    {
        //choose a random point to spawn the coin
        int coinSpawnIndex = Random.Range(5, 11);
        Transform spawnPoint = transform.GetChild(coinSpawnIndex).transform;
        //spawnPoint2 = new Vector3(spawnPoint.x);
        //Spawn the coin at the position
        //Instantiate(coin, spawnPoint.position, Quaternion.Euler(90, 0, 0), transform);
        //Instantiate(coin, spawnPoint.position, Quaternion.Euler(90, 0, 0), transform);
        //Instantiate(coin, spawnPoint.position, Quaternion.Euler(90, 0, 0), transform);
    }

    void Update()
    {
        
    }

}
