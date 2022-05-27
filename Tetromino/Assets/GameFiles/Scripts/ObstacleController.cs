using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour {

    private Vector3 dir; 
    private GameObject temp;
    void Start()
    {
        dir = Vector3.left;
        temp = GameObject.FindGameObjectsWithTag("ObstacleManager")[0];
        StartCoroutine(MoveObstacle());       
    }

    void Update()
    {


       
        if (PlayerController.gameOver)
        {
            temp.GetComponent<ObstaclesManager>().speedObstacleMoving = 20f;
        }

       
        if (!CameraController.isCameraRotateFinish)
        {
            Destroy(gameObject);
        }
        
    }

   
    void OnBecameInvisible()
    {
        StopCoroutine(MoveObstacle());
        Destroy(gameObject);
    }

   
    IEnumerator MoveObstacle()
    {
        while (true)
        {
            if (CameraController.isCameraRotateFinish)
            {
                gameObject.transform.position = gameObject.transform.position + dir * temp.GetComponent<ObstaclesManager>().speedObstacleMoving * Time.deltaTime;
            }
            yield return null;
        }
    }
}
