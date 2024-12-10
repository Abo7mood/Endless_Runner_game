using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
   [SerializeField] GameObject[] obstaclesPrefab;
   [SerializeField] GameObject[] obstaclesSpawns;


    // CHIMONEY CODE
    private void Start()
    {
        groundSpawner = FindObjectOfType<GroundSpawner>();
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            groundSpawner.SpawnTile();
            Destroy(gameObject, 15);
        }
        
    }
    

}
