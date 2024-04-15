using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private List<Vector3> Spawners = new List<Vector3>()
    {
        new Vector3(-7f, 5f, -5f),
        new Vector3(-5f, 5f, -5f),
        new Vector3(-3f, 5f, -5f),
        new Vector3(-1f, 5f, -5f),
        new Vector3(1f, 5f, -5f),
        new Vector3(3f, 5f, -5f),
        new Vector3(5f, 5f, -5f),
        new Vector3(7f, 5f, -5f),
    };

    #region pause menu

    public GameObject PauseMenu;
    public GameObject SettingsPanel;
    public Slider MusicSlider;
    public TMP_Text MusicValue;
    public Slider EffectsSlider;
    public TMP_Text EffectsValue;

    #endregion

    public List<GameObject> Hearts;
    private int StartingPlayerHP = 8 - 2 * Game.GameDificulty;
    public TextMeshProUGUI Score;
    public TextMeshProUGUI HighScore;
    public TextMeshProUGUI Message;
    public Button QuitButton;
    //private List<Vector3> Spawners = Helper.Spawners;
    public List<GameObject> Enemies;
    public List<GameObject> Bosses;

    private int maxEnemiesOnScreen = 10;
    private int enemyPerWave = 10;
    private int enemiesSpwaned = 0;
    private int enemiesAlive = 0;
    private int waveCount = 0;

    private bool bossWave = false;
    private bool spawnerRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        Game.OnPlayerHit += Game_OnPlayerHit;
        Game.OnEnemyDestroyed += Game_OnEnemyDestroyed;
        Game.OnPlayerHeal += Game_OnPlayerHeal;
        Game.OnPlayerDestroyed += Game_OnPlayerDestroyed;
        SetupHearts();
        LoadHighScore();
        this.GetComponent<AudioSource>().volume = Game.MusicVolume;
        RunSpawner();
    }

    private void SetupHearts()
    {
        for (int i = 0; i < (Hearts.Count - StartingPlayerHP); i++)
        {
            if (Hearts[i].activeInHierarchy)
            {
                Hearts[i].SetActive(false);
            }
        }
    }


    private void RunSpawner()
    {
        if (!spawnerRunning)
        {
            StartCoroutine(Spawner());
        }
    }

    private IEnumerator Spawner()
    {
        spawnerRunning = true;
        yield return null;
        //SpawnBoss(2);
        while (enemiesAlive < maxEnemiesOnScreen)
        {
            if (enemiesSpwaned < enemyPerWave)
            {
                int time = Random.Range(1000, 3000);
                yield return new WaitForSeconds(time / 1000);
                int spawnPick = Random.Range(0, 8);
                GameObject enemy = PickEnemy();
                enemy.transform.position = Spawners[spawnPick];
                enemy.SetActive(true);
                enemiesAlive++;
                enemiesSpwaned++;
            }
            else
            {
                if (enemiesAlive == 0)
                {
                    //Pause before new wave
                    waveCount++;
                    Message.text = string.Format("Wave {0} completed!", waveCount);
                    Message.gameObject.SetActive(true);
                    yield return new WaitForSeconds(5);
                    Message.gameObject.SetActive(false);
                    enemiesSpwaned = 0;
                    if (waveCount % 10 == 0)
                    {
                        SpawnBoss((waveCount / 10 - 1) % 3);
                        bossWave = true;
                        enemiesAlive++;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        spawnerRunning = false;
    }

    private void SpawnBoss(int tier)
    {
        GameObject boss = GameObject.Instantiate(Bosses[tier]);
        boss.transform.position = new Vector3(0, 2.2f, -5f);
        boss.SetActive(true);
    }

    private GameObject PickEnemy()
    {
        //return GameObject.Instantiate(Enemies[2]);

        int t2Edge = (waveCount * 5 + Game.GameDificulty * 5) > 70 ? 70 : (waveCount * 5 + Game.GameDificulty * 5);
        int t3Edge = (waveCount * 2 + Game.GameDificulty * 2) > 30 ? 30 : (waveCount * 2 + Game.GameDificulty * 2);

        int rand = Random.Range(1, 100);

        if(rand <= t3Edge)
        {
            return GameObject.Instantiate(Enemies[2]);
        }
        else if (rand <= t2Edge)
        {
            return GameObject.Instantiate(Enemies[1]);
        }
        else
        {
            return GameObject.Instantiate(Enemies[0]);
        }
    }

    private void Game_OnEnemyDestroyed(object source, int tier)
    {
        int score = int.Parse(Score.text);
        score = score + 10 * tier;
        UpdateHighScore(score);
        Score.text = score.ToString();
        if(enemiesAlive > 0)
            enemiesAlive--;

        if(tier >= 10)
        {
            StartCoroutine(BossKillSequence());
        }

        RunSpawner();
    }

    private void UpdateHighScore(int score)
    {
        int highScore = int.Parse(HighScore.text);
        if(score > highScore)
        {
            highScore = score;
            HighScore.text = highScore.ToString();
            PlayerPrefs.SetInt(string.Format("highscore_{0}", Game.GameDificulty), highScore);
        }
    }

    private void LoadHighScore()
    {
        int highScore = PlayerPrefs.GetInt(string.Format("highscore_{0}", Game.GameDificulty), 0);
        HighScore.text = highScore.ToString();
    }

    private IEnumerator BossKillSequence()
    {
        waveCount++;
        Message.text = "Boss defeated!";
        Message.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        Message.gameObject.SetActive(false);
    }

    private void Game_OnPlayerHit(object source, int strength)
    {
        int cnt = strength;
        for (int i = 0; i < Hearts.Count; i++)
        {
            if (Hearts[i].activeInHierarchy)
            {
                Hearts[i].SetActive(false);
                cnt--;
            }
            if(cnt == 0)
            {
                break;
            }
        }
    }

    private void Game_OnPlayerDestroyed(object source)
    {
        Message.text = "Game over!";
        Message.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);
    }

    private void Game_OnPlayerHeal(object source)
    {
        int index = -1;
        for (int i = 0; i < Hearts.Count; i++)
        {
            if (Hearts[i].activeInHierarchy)
            {
                index = i - 1;
                break;
            }
        }
        if(index >= 0)
        {
            Hearts[index].SetActive(true);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    #region pause menu behavior

    private void PauseGame()
    {
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
        if (PauseMenu.activeInHierarchy)
        {
            Time.timeScale = 0;

            this.GetComponent<AudioSource>().Stop();
        }
        else
        {
            Time.timeScale = 1;
            this.GetComponent<AudioSource>().volume = Game.MusicVolume;
            this.GetComponent<AudioSource>().Play();
        }
    }

    public void ResumeGame()
    {
        if (PauseMenu.activeInHierarchy)
        {
            this.GetComponent<AudioSource>().volume = Game.MusicVolume;
            this.GetComponent<AudioSource>().Play();
            PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
            Time.timeScale = 1;
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }

    public void OppenSettings()
    {
        MusicSlider.value = Game.MusicVolume;
        EffectsSlider.value = Game.EffectVolume;
        MusicValue.text = ((int)(MusicSlider.value * 100)).ToString();
        EffectsValue.text = ((int)(EffectsSlider.value * 100)).ToString();
        SettingsPanel.SetActive(true);
    }

    public void SettingsApply()
    {
        Game.MusicVolume = MusicSlider.value;
        Game.EffectVolume = EffectsSlider.value;
        SettingsPanel.SetActive(false);
    }
    public void SettingsCancel()
    {
        MusicSlider.value = Game.MusicVolume;
        EffectsSlider.value = Game.EffectVolume;
    }

    public void SettingsBack()
    {
        SettingsPanel.SetActive(false);
    }

    public void MusicValueChanged()
    {
        MusicValue.text = ((int)(MusicSlider.value * 100)).ToString();
    }

    public void EffectsValueChanged()
    {
        EffectsValue.text = ((int)(EffectsSlider.value * 100)).ToString();
    }

    #endregion
}
