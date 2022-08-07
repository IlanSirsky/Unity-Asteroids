using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public int lives = 3;

    public float respawnTime = 3.0f;
    public float respawnInvulnerabillityTime = 3.0f;
    public int score = 0;
    
    public Text scoreText;
    public Text livesText;
    public Text gameOver;
    public Text playAgain;

    private void Awake(){
        Application.targetFrameRate = 60;
        gameOver.gameObject.SetActive(false);
        Pause();
    }

    private void Pause(){
        player.enabled = false;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && player.isActiveAndEnabled == false) {
            Play();
        }
    }

    private void Play(){
        this.lives = 3;
        livesText.text = "x" + lives.ToString();
        this.score = 0;
        scoreText.text = score.ToString();
        playAgain.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        Time.timeScale = 1f;

        player.enabled = true;

        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
        foreach (Asteroid asteroid in asteroids)
        {
            Destroy(asteroid.gameObject);
        }
        Respawn();
    }

    public void AsteroidDestroyed(Asteroid asteroid){
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if (asteroid.size < 0.75f){
            this.score += 100;
        } else if (asteroid.size < 1.0f){
            this.score += 50;
        } else { 
            this.score += 25;
        }
        scoreText.text = score.ToString();
    }

    public void PlayerDied(){
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;
        //livesText.text = lives.ToString();
        livesText.text = "x" + lives.ToString();

        if (this.lives <= 0){
            GameOver();
        } else {
            Invoke(nameof(Respawn), this.respawnTime);
        }

    }

    private void Respawn(){
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.layer = LayerMask.NameToLayer("Ignore Collisions");
        this.player.gameObject.SetActive(true);
        Invoke(nameof(TurnOnCollisions), respawnInvulnerabillityTime);
    }

    private void TurnOnCollisions(){
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void GameOver(){
        playAgain.gameObject.SetActive(true);
        gameOver.gameObject.SetActive(true);
        Pause();
    }
}
