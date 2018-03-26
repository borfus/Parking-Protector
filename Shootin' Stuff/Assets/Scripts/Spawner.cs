using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject EnemyOne;
    public GameObject EnemyTwo;
    public GameObject EnemyThree;
    public GameObject EnemyFour;
    public float SpawnTime;

    private Vector3 randLocation;
    private float posX;
    private float rNum;
    private bool spawning = false;
    private int randomEnemy;
    private float playedTime;
    private int min = 0;
    private int max;
    List<GameObject> Enemies;
    Movement movement = new Movement();
    public int defaultBirdSpeed = 2;
    GameObject normalBird;
    GameObject fastBird;
    GameObject bigBird;
    GameObject sinBird; 

    // Use this for initialization
    void Start()
    {
        playedTime = 0.0f;
        posX = EnemyOne.transform.position.x;
        Enemies = new List<GameObject> { EnemyOne, EnemyTwo, EnemyThree, EnemyFour };

        normalBird = EnemyOne;
        fastBird = EnemyTwo;
        bigBird = EnemyThree;
        sinBird = EnemyFour;

        normalBird.GetComponent<Movement>().speed = 2;
        fastBird.GetComponent<Movement>().speed = 4;
        bigBird.GetComponent<Movement>().speed = 1;
        sinBird.GetComponent<WaveMovement>().speed = 2;
    }

    // Update is called once per frame
    void Update()
    {
        print(defaultBirdSpeed);
        playedTime += Time.deltaTime;
        //print(playedTime);

        if (!spawning && playedTime < 30)
        {
            max = 1;
            GenerateYValue();
            GenerateRandomEnemy(min, max);
        }
        if (!spawning && playedTime >= 30 && playedTime < 60)
        {
            max = 2;
            GenerateYValue();
            GenerateRandomEnemy(min, max);
        }
        if (!spawning && playedTime >= 60 && playedTime < 90)
        {
            max = 3;
            GenerateYValue();
            GenerateRandomEnemy(min, max);
        }
        if (!spawning && playedTime >= 90 && playedTime < 120)
        {
            max = 4;
            GenerateYValue();
            GenerateRandomEnemy(min, max);
        }
        if (!spawning && playedTime >= 120 && playedTime < 180)
        {
            max = 4;
            GenerateYValue();
            GenerateRandomEnemy(min, max);

            if (defaultBirdSpeed == 2)
            {
                defaultBirdSpeed = 3;
                normalBird.GetComponent<Movement>().speed++;
                fastBird.GetComponent<Movement>().speed++;
                bigBird.GetComponent<Movement>().speed++;
                sinBird.GetComponent<WaveMovement>().speed++;
            }

        }
        if (!spawning && playedTime >= 180)
        {
            max = 4;
            GenerateYValue();
            GenerateRandomEnemy(min, max);

            if (defaultBirdSpeed == 3)
            {
                defaultBirdSpeed = 4;
                normalBird.GetComponent<Movement>().speed++;
                fastBird.GetComponent<Movement>().speed++;
                bigBird.GetComponent<Movement>().speed++;
                sinBird.GetComponent<WaveMovement>().speed++;
            }
        }
    }

    void GenerateYValue()
    {        
        spawning = true;
        rNum = Random.Range(-3, 9); // -3 - 9 Y random range limits
        randLocation = new Vector3(posX, rNum, 0);
        StartCoroutine(waitAndSpawn());
    }

    int GenerateRandomEnemy(int tempMin, int tempMax)
    {
        randomEnemy = Random.Range(tempMin, tempMax);
        print("Spawning enemy " + randomEnemy);
        return randomEnemy;
    }

    IEnumerator waitAndSpawn()
    {
        yield return new WaitForSeconds(SpawnTime);
        var newEnemy = GameObject.Instantiate(Enemies[randomEnemy], randLocation, Quaternion.identity);
        spawning = false;
    }
}
