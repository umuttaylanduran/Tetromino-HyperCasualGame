using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstaclesManager : MonoBehaviour {

    public GroundManager groundManager;
    public PlayerController playerController;
    public CameraController cameraController;
    public GameObject obstaclesManager;
    public GameObject[] arrayObstacles; 
    public GameObject item; 
    [Range(0f, 1f)]
    public float itemFrequency; 
    public float timeToWaitToCreateObstacle = 3f; 
    public float timeDecreaseWhenCameraRotate = 1f;
    public float speedItemMoving = 4f; 
    public float speedObstacleMoving = 4f;
    public float speedItemMovingIncrease = 1.5f; 
    public float speedObstacleMovingIncrease = 1.5f;
    
    private List<Vector3> listPosition = new List<Vector3>();  
    private bool enableCheck = true;
	
	void Start () {
        

        
        int i = -groundManager.numberOfGround;
        while (i <= groundManager.numberOfGround)
        {
            Vector3 pos = obstaclesManager.transform.position + new Vector3(0, 0, i);
            listPosition.Add(pos);
            i = i + 2;
        }

        StartCoroutine(RandomObstacle(timeToWaitToCreateObstacle));

       
    }
	
	
	void Update () {

        
        if (cameraController.startToRotateCamera && enableCheck)
        {
            enableCheck = false;
            timeToWaitToCreateObstacle = timeToWaitToCreateObstacle - timeDecreaseWhenCameraRotate;
            speedItemMoving += speedItemMovingIncrease;
            speedObstacleMoving += speedObstacleMovingIncrease;
            if (timeToWaitToCreateObstacle <= 1f)
            {
                timeToWaitToCreateObstacle = 1f;
            }
            StartCoroutine(WaitAndEnableCheck());
        }
	}

    IEnumerator RandomObstacle(float time)
    {
        yield return new WaitForSeconds(time);

        while (true)
        {
            
            if (groundManager.finishMoveGround && !PlayerController.gameOver && !cameraController.startToRotateCamera)
            {
                int indexArrayObstacle = Random.Range(0, arrayObstacles.Length); 
                int indexPositionOfObstacle = Random.Range(0, listPosition.Count); 


                
                float itemIndex = Random.Range(0f, 1f); 
                if (itemIndex <= itemFrequency)
                {
                    int indexPositionOfItem = Random.Range(0, listPosition.Count); 
                    while (indexPositionOfItem == indexPositionOfObstacle)
                    {
                        indexPositionOfItem = Random.Range(0, listPosition.Count); 
                    }
                    Instantiate(item, listPosition[indexPositionOfItem], Quaternion.identity);
                }
                GameObject currentObstacle = (GameObject)Instantiate(arrayObstacles[indexArrayObstacle], listPosition[indexPositionOfObstacle], Quaternion.identity);
                currentObstacle.transform.SetParent(obstaclesManager.transform);
                yield return new WaitForSeconds(timeToWaitToCreateObstacle);
            }
            yield return null;
        }
    }

    IEnumerator WaitAndEnableCheck()
    {
        yield return new WaitForSeconds(1.5f);
        enableCheck = true;
    }
   

}
