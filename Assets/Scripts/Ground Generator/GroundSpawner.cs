using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject GroundTile;
    private Vector3 _nextSpawnPoint;

    public void SpawnTile()
    {
        GameObject temp = Instantiate(GroundTile, _nextSpawnPoint, Quaternion.identity);
        //the second child in the groundTile 
        temp.transform.parent = GameObject.Find("GameLife").transform;
        _nextSpawnPoint = temp.transform.GetChild(1).transform.position;
    }

    void Start()
    {
        for(int i = 0; i < 7; i++)
        {
            SpawnTile();
        }
    }
}
