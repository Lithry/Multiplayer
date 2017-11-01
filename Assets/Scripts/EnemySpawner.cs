using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

    public GameObject enemyPrefab;
    public int numberOfEnemies;

    public override void OnStartServer()
    {
        for (int i=0; i < numberOfEnemies; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-50.0f, 30.0f), Random.Range(-30.0f, 30.0f), 0.0f);

            var spawnRotation = Quaternion.Euler(0.0f, 0.0f, 0);

            var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
            NetworkServer.Spawn(enemy);
        }
    }
}