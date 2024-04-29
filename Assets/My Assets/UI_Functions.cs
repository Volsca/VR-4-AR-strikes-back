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

    private int foodCounter = 1;

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
}
