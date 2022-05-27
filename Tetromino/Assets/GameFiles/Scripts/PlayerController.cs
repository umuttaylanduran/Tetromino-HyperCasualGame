using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject player;
    public GroundManager groundManager;
    public CameraController cameraController;
    public ParticleSystem particle; 
    public bool touchDisable; 
    public float movingSpeedOfPlayer = 7; 
    public float movingSpeedIncreaseOfPlayer = 0.7f; 
    public float speedPlayerFalling = 20f; 
    public static bool gameOver; 

    private Vector3 dir; 
    private float dirTurn; 
    private bool isGameOverSoundPlay = false;
    private bool enableCheck = true;
	
	void Start () {
     
        gameOver = false;
        touchDisable = false;
        dirTurn = 1;
        StartCoroutine(MovePlayer());
	}
	
	
	void Update () {

       
        if (Input.GetMouseButtonDown(0) && groundManager.finishMoveGround && !touchDisable)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.click);
            dirTurn = dirTurn * (-1);
            if (dirTurn < 0)
            {
                dir = Vector3.forward;
            }
            else
            {
                dir = Vector3.back;
            }
        }
        

       
        RaycastHit hit;
        Ray rayDown = new Ray(player.transform.position, Vector3.down);
        if(!Physics.Raycast(rayDown,out hit, 0.5f))
        {
            if (!isGameOverSoundPlay)
            {
                isGameOverSoundPlay = true;
                SoundManager.Instance.PlaySound(SoundManager.Instance.gameOver);
            }
            
            gameOver = true;
            touchDisable = true;
            player.GetComponent<Rigidbody>().isKinematic = false;
            movingSpeedOfPlayer = speedPlayerFalling;
            dir = Vector3.down;
        }

        
        if (cameraController.startToRotateCamera && enableCheck)
        {
            enableCheck = false;
            movingSpeedOfPlayer += movingSpeedIncreaseOfPlayer;
            StartCoroutine(WaitAndEnableCheck(2f));
        }
	}

    void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Obstacle")
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.playerHitObstacle);
            touchDisable = true;
            gameOver = true;
            dir = Vector3.left;
        }


       
        if (other.tag == "Item")
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.hitItem);
            CoinManager.Instance.AddCoins(1);
            ParticleSystem particleTemp;
            particleTemp = (ParticleSystem)Instantiate(particle, other.gameObject.transform.position, Quaternion.identity);
            particleTemp.Simulate(0.5f, true, false);
            particleTemp.Play();
            Destroy(particleTemp, 0.5f);
            Destroy(other.gameObject);
        }
    }


    
    IEnumerator MovePlayer()
    {
        while (true)
        {
            if (!cameraController.startToRotateCamera)
            {
                player.transform.position = player.transform.position + dir * movingSpeedOfPlayer * Time.deltaTime;
            }
            yield return null;
        }
    }

    IEnumerator WaitAndEnableCheck(float time)
    {
        yield return new WaitForSeconds(time);
        enableCheck = true;
    }
}
