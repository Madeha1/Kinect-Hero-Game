using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private GroundSpawner _groundSpawner;
    public GameObject[] Obstacles;
    public GameObject Coin;

    void Start()
    {
        _groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnObstacle();
        SpawnCoin();
    }

    private void OnTriggerExit(Collider other)
    {
        _groundSpawner.SpawnTile();
        Destroy(gameObject, 2); // 2 second after exit destroy it.
    }
    
    void SpawnObstacle()
    {
        //choose a random point to spawn the obstacle
        //2 is included but 5 not and they represent the middle, left and right;
        int obstacleSpawnIndex = Random.Range(2, 5);
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;
        //Spawn the obstecale at the position
        int rand = Random.Range(0, Obstacles.Length);
        GameObject obstacle = Obstacles[rand];
        Instantiate(obstacle, spawnPoint.position, Quaternion.identity, transform);
    }

    void SpawnCoin()
    {
        //choose a random point to spawn the coin
        int coinSpawnIndex = Random.Range(5, 11);
        Transform spawnPoint = transform.GetChild(coinSpawnIndex).transform;
        Vector3 spawnPointPosioton2 = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z - 1.2f);
        Vector3 spawnPointPosioton3 = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z - 2.4f);
        //Spawn the coin at the position
        Instantiate(Coin, spawnPoint.position, Quaternion.Euler(90, 0, 0), transform);
        Instantiate(Coin, spawnPointPosioton2, Quaternion.Euler(90, 0, 0), transform);
        Instantiate(Coin, spawnPointPosioton3, Quaternion.Euler(90, 0, 0), transform);
    }
}
