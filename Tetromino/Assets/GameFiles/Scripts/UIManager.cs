using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour {
    public GroundManager groundManager;
    public CameraController cameraController;
    public Text score;
    public Text bestScore;
    public Text gold;
    public Image muteButton;
    public Image unMuteButton;
    public Image replayButton;
    public int scoreToScaleGroundIncrease = 15; 
    public int scoreToRotateCameraIncrease = 20;


   
    public int scoreToScaleGround = 15; 
    public int scoreToRotateCamera = 20; 


    private bool enableCheck = true;
   
    void Start () {
        ScoreManager.Instance.Reset();
        muteButton.enabled = false;
        unMuteButton.enabled = false;
        replayButton.enabled = false;
        StartCoroutine(CountScore());
	}
	
	
	void Update () {
        score.text = ScoreManager.Instance.Score.ToString();
        bestScore.text = ScoreManager.Instance.HighScore.ToString();
        gold.text = CoinManager.Instance.Coins.ToString();
        if (cameraController.startToRotateCamera && enableCheck) 
        {
            enableCheck = false;
            scoreToScaleGround += scoreToScaleGroundIncrease;
            scoreToRotateCamera += scoreToRotateCameraIncrease;
            StartCoroutine(WaitAndEnableCheck());
        }

        if (PlayerController.gameOver)
        {
            Invoke("EnableButton", 1.5f);
        }
	}


    
    IEnumerator CountScore()
    {
        while (true)
        {
            if (groundManager.finishMoveGround && !PlayerController.gameOver && !cameraController.startToRotateCamera)
            {
                ScoreManager.Instance.AddScore(1);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator WaitAndEnableCheck()
    {
        yield return new WaitForSeconds(2f);
    }

    public void SoundClick()
    {
        if (SoundManager.Instance.IsMuted())
        {
            unMuteButton.enabled = true;
            muteButton.enabled = false;
            SoundManager.Instance.ToggleMute();
        }
        else
        {
            unMuteButton.enabled = false;
            muteButton.enabled = true;
            SoundManager.Instance.ToggleMute();
        }
        SoundManager.Instance.PlaySound(SoundManager.Instance.hitButton);
    }

    void EnableButton()
    {
        replayButton.enabled = true;
        if (SoundManager.Instance.IsMuted())
        {
            muteButton.enabled = true;
            unMuteButton.enabled = false;
        }
        else
        {
            muteButton.enabled = false;
            unMuteButton.enabled = true;
        }
    }

    public void ReplayButton()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.hitButton);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
