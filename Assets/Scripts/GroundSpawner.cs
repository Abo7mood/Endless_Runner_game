using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] PlayerMovement player;
    public GameObject[] groundTiles;
    Vector3 nextSpawnPoint;
    [SerializeField] float SpawnTime;

    public void SpawnTile()
    {
        if (player.isDead == false)
        {
            int randomization = Random.Range(0, 4);

            GameObject temp = Instantiate(groundTiles[randomization], nextSpawnPoint, Quaternion.identity);
            nextSpawnPoint = temp.transform.GetChild(0).transform.position;
        }
        else
            return;
      
    }

    // CHIMONEY CODE
    void Start()
    {
        InvokeRepeating(nameof(SpawnTile), 0, SpawnTime);


    }
}