using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Functions : MonoBehaviour
{
    public GameObject cubePrefab;
    public GameObject ballPrefab;
    public GameObject ballShooterPrefab;
    public GameObject foodPrefab1;
    public GameObject foodPrefab2;
    public GameObject foodPrefab3;
    public GameObject foodPrefab4;
    public GameObject foodPrefab5;

    public List<GameObject> puzzleCubes;
    public float spawnMargin;

    private int foodCounter = 1;


    // Functions called by the buttons on the grabbable menu
    public void SpawnCube()
    {
        GameObject Cube = Instantiate(cubePrefab);
        Cube.transform.position = transform.position;
    }
    public void SpawnBall()
    {
        GameObject Ball = Instantiate(ballPrefab);
        Ball.transform.position = transform.position;
    }
    public void SpawnBallShooter()
    {
        GameObject BallShooter = Instantiate(ballShooterPrefab);
        BallShooter.transform.position = transform.position;
    }
    public void SpawnFood()
    {

        switch (foodCounter)
        {
            case 0:
                GameObject Food = Instantiate(foodPrefab1);
                Food.transform.position = transform.position;
                break;
            case 1:
                GameObject Food1 = Instantiate(foodPrefab2);
                Food1.transform.position = transform.position;
                break;
            case 2:
                GameObject Food2 = Instantiate(foodPrefab3);
                Food2.transform.position = transform.position;
                break;
            case 3:
                GameObject Food3 = Instantiate(foodPrefab4);
                Food3.transform.position = transform.position;
                break;
            case 4:
                GameObject Food4 = Instantiate(foodPrefab5);
                Food4.transform.position = transform.position;
                break;
            default:
                break;
        }

        foodCounter += 1;
        foodCounter = foodCounter % 5;
    }
    public void SpawnPuzzle()
    {
        GameObject cube1 = Instantiate(puzzleCubes[0]);
        cube1.transform.position = transform.position;
        GameObject cube4 = Instantiate(puzzleCubes[3], cube1.transform.position 
                                                       + 1.2f * cube1.transform.localScale.x * new Vector3(0, 0, 1) 
                                                       + spawnMargin * new Vector3(0, 0, 1), Quaternion.identity);
        GameObject cube7 = Instantiate(puzzleCubes[6], cube4.transform.position
                                                       + 1.2f * cube1.transform.localScale.x * new Vector3(0, 0, 1)
                                                       + spawnMargin * new Vector3(0, 0, 1), Quaternion.identity);
        GameObject cube2 = Instantiate(puzzleCubes[1], cube1.transform.position
                                                       + 1.2f * cube1.transform.localScale.x * new Vector3(1, 0, 0)
                                                       + spawnMargin * new Vector3(1, 0, 0), Quaternion.identity);
        GameObject cube3 = Instantiate(puzzleCubes[2], cube2.transform.position
                                                       + 1.2f * cube1.transform.localScale.x * new Vector3(1, 0, 0)
                                                       + spawnMargin * new Vector3(1, 0, 0), Quaternion.identity);
        GameObject cube5 = Instantiate(puzzleCubes[4], cube4.transform.position
                                                       + 1.2f * cube1.transform.localScale.x * new Vector3(1, 0, 0)
                                                       + spawnMargin * new Vector3(1, 0, 1), Quaternion.identity);
        GameObject cube6 = Instantiate(puzzleCubes[5], cube5.transform.position
                                                       + 1.2f * cube1.transform.localScale.x * new Vector3(1, 0, 0)
                                                       + spawnMargin * new Vector3(1, 0, 1), Quaternion.identity);
        GameObject cube8 = Instantiate(puzzleCubes[7], cube7.transform.position
                                                       + 1.2f * cube1.transform.localScale.x * new Vector3(1, 0, 0)
                                                       + spawnMargin * new Vector3(1, 0, 1), Quaternion.identity);
        GameObject cube9 = Instantiate(puzzleCubes[8], cube8.transform.position
                                                       + 1.2f * cube1.transform.localScale.x * new Vector3(1, 0, 0)
                                                       + spawnMargin * new Vector3(1, 0, 1), Quaternion.identity);
    }
}
