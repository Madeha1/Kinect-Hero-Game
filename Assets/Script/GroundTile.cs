using UnityEngine;

public class GroundTile : MonoBehaviour
{

    GroundSpawner groundSpawner;
    public GameObject treeObstacle;

    void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        SpawnObstacle();
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

    void Update()
    {
        
    }

}
