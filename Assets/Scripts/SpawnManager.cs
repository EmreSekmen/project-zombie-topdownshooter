using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;


    public int currenWawe = 1;
    public int maxWawe = 5;

    public int EnemiesPerWawe = 2;
    public float spawnDelay = 0.7f;

    public int EnemiesAlive = 0;
    public bool waweInProgress = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!waweInProgress && EnemiesAlive == 0)
        {
            if(currenWawe <= maxWawe)
                StartCoroutine(StartWawe());
            
           
        }
    }

    public IEnumerator StartWawe()
    {
        waweInProgress = true;

        Debug.Log("Wawe" + currenWawe + " Baþladý");

        int spawnCount = EnemiesPerWawe + (currenWawe - 1) * 2;

        for(int i = 0; i < spawnCount; i++)
        {
            SpawnSystem();
            yield return new WaitForSeconds(spawnDelay);
        }

        waweInProgress = false;
        currenWawe++;

        if(currenWawe > maxWawe)
        {
            Debug.Log("Chapter Tamamnlandý!");
        }
    }




    void SpawnSystem()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[index], SpawnPos(), enemyPrefabs[index].transform.rotation);


        EnemiesAlive++;


    }

   public void OnEnemyKilled()
    {
        EnemiesAlive--;
    }

    Vector2 SpawnPos()
    {
        return new Vector2(Random.Range(-10, 10), Random.Range(-4.41f, 4.41f));
    }

    
}
