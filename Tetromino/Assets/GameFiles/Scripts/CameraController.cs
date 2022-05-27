using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public PlayerController playerController;
    public UIManager UIManager;
    public Transform target; 
    public Camera cam;
    [HideInInspector]
    public bool startToRotateCamera = false; 
    public static bool isCameraRotateFinish = true; 
    public float rotateSpeed = 90f; 
    public Color[] colors; 
   


    private bool enableCheck = true;
    private const float rotateAngle = 90f; 
    private float timToEnableCheck = 5f;
	
	void Start () {
        int indexColor = Random.Range(0, colors.Length);
        cam.backgroundColor = colors[indexColor];


        cam.transform.LookAt(target.transform.position); 
        StartCoroutine(RotateCamera());
       
	}
	
	
	void Update () {
       
	}


    IEnumerator RotateCamera()
    {
        while (true)
        {
            
            if ((ScoreManager.Instance.Score != 0) && (ScoreManager.Instance.Score % UIManager.scoreToRotateCamera == 0) && !PlayerController.gameOver && enableCheck)
            {
                startToRotateCamera = true;
                isCameraRotateFinish = false;
                enableCheck = false;             
                SoundManager.Instance.PlaySound(SoundManager.Instance.cameraRotate);             
                playerController.touchDisable = true; 
                float currentAngle = 0f;
                
                while (currentAngle < rotateAngle)
                {
                    cam.transform.RotateAround(target.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
                    currentAngle += rotateSpeed * Time.deltaTime;
                    yield return null;
                }
                playerController.touchDisable = false; 
                startToRotateCamera = false;
                isCameraRotateFinish = true;
                StartCoroutine(WaiAndEnableCheck(timToEnableCheck));
            }
            yield return null;         
        }
    }

    IEnumerator WaiAndEnableCheck(float time)
    {
        yield return new WaitForSeconds(time);
        enableCheck = true;
    }
    
}
