using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public enum LevelState
{
    Start,
    Middle,
    End    
}

public class LevelManager : MonoBehaviour
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Data")]
            public int curWave;
            public int curScore;
            public int curSeconds;
            public LevelState curLevelState;
            public Light2D globalLight;
            
            [Header("Wave Settings")]
            public float newWaveCooldown;
            private bool canStartWave;
            public float startFirstWaveIn;
            public float showDeathWindowIn;
            public float timeStopValue;
            public TMP_Text timeTMP;
            public Coroutine timerCoroutine;
            public TMP_Text scoreTMP;
            public TMP_Text healthTMP;
            public AudioClip playerDeathSFX;
            public ObstacleManager obstacleManager;

            [Header("Entities")]
            public GameObject[] enemyPrefabs;
            public List<GameObject> aliveEnemies;

            [Header("Position Clamping")]
            public Transform wallPos;
            public float upperBound;
            public float lowerBound;
            public float rightBound;
            public float leftBound;
            public float leftSideWallOffset;
            public float rightSideWallOffset;
            public static LevelManager instance;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        void Awake()
        {
            SingletonUtility.MakeSingleton(ref instance, this, false);
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        void Start()
        {
            curLevelState = LevelState.Start;
            curScore = 0;
            UpdateScore(curScore);
            UpdatePlayerHealth();

            int randValue = UnityEngine.Random.Range(1, 3);
            if (randValue == 1)
            {
                SetInitialSides(Side.Right, Side.Left);
            }
            else if (randValue == 2)
            {
                SetInitialSides(Side.Left, Side.Right);
            }

            Invoke("FirstWaveInvoke", startFirstWaveIn);
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        void Update()
        {
            if (canStartWave)
            {
                canStartWave = false;

                NewWave();
            }
        }

        /// <summary>
        /// Called at fixed intervals, ideal for physics updates.
        /// Use this for physics-related updates like applying forces or handling Rigidbody physics.
        /// </summary>
        void FixedUpdate()
        {
            // Add physics-related logic here.
            // Example: Rigidbody movement, applying forces, or collision detection.
        }

    #endregion

    #region CUSTOM METHODS

        private void FirstWaveInvoke()
        {
            timerCoroutine = StartCoroutine(Timer());
            NewWave();

            Ball ballScript = UnityEngine.Object.FindFirstObjectByType<Ball>();
            ballScript.Launch(0);
            
            curLevelState = LevelState.Middle;
        }

        void SetInitialSides(Side playerSide, Side enemySide)
        {
            // Player
            Player player = GameObject.Find("Player").GetComponent<Player>();
            player.curSide = playerSide;
            player.gameObject.transform.position = SetSidePosition(playerSide);
            
            // Enemy
            GameObject[] characters = GameObject.FindGameObjectsWithTag("Character");
            foreach(GameObject character in characters)
            {
                if (character.name != "Player")
                {
                    Enemy enemyScript = character.GetComponent<Enemy>();
                    enemyScript.curSide = enemySide;
                    enemyScript.gameObject.transform.position = SetSidePosition(enemySide);
                }
            }
        }

        public Vector3 SetSidePosition(Side side)
        {
            float posX = 0f;
            float posY = 0f;

            if (side == Side.Left)
            {         
                posX = UnityEngine.Random.Range(leftBound, wallPos.position.x + leftSideWallOffset);   
            }
            else if (side == Side.Right)
            {
                posX = UnityEngine.Random.Range(wallPos.position.x + rightSideWallOffset, rightBound);    
            }
            
            posY = UnityEngine.Random.Range(lowerBound, upperBound);   
            
            return new Vector3(posX, posY, 0f);
        }

        void NewWave()
        {                    
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 1f, 0.8f).SetEase(Ease.OutQuad);
            MessageManager.instance.NewWave();

            obstacleManager.MoveObstacles();
            
            curWave++;

            int enemyNum = UnityEngine.Random.Range(2, 5);
            for (int i = 1; i <= enemyNum; i++)
            {
                CreateEnemy();
            }
        }

        void CreateEnemy()
        {
            int rndIndex = UnityEngine.Random.Range(0, enemyPrefabs.Length);
            GameObject enemyToSpawn = enemyPrefabs[rndIndex];
            Enemy enemyScript = enemyToSpawn.GetComponent<Enemy>();
            Player playerScript = GameObject.Find("Player").GetComponent<Player>();
            if (playerScript.curSide == Side.Left)
            {
                enemyScript.curSide = Side.Right;
            }
            else if (playerScript.curSide == Side.Right)
            {
                enemyScript.curSide = Side.Left;
            }

            Vector3 spawnPos = Vector3.zero;
            Player player = GameObject.Find("Player").GetComponent<Player>();
            if (player.curSide == Side.Left)
            {
                spawnPos = SetSidePosition(Side.Right);
            }
            else
            {
                spawnPos = SetSidePosition(Side.Left);
            }

            float rndRotation = UnityEngine.Random.Range(0f, 359f);
            Quaternion rotation = Quaternion.Euler(0f, 0f, rndRotation);

            GameObject obj = Instantiate(enemyToSpawn, spawnPos, rotation);
            
            AddEnemy(obj);
        }

        public void AddEnemy(GameObject objToRemove)
        {
            aliveEnemies.Add(objToRemove);
        }

        public void RemoveEnemy(GameObject objToRemove)
        {
            aliveEnemies.Remove(objToRemove);

            if (aliveEnemies.Count == 0)
            {
                Player player = GameObject.Find("Player").GetComponent<Player>();
                
                if (player.curSide == Side.Left)
                {
                    Gate rightGate = GameObject.Find("RightGate").GetComponent<Gate>();
                    rightGate.TakeDamage(rightGate.curHealth - 1);
                    MessageManager.instance.FinishHim();
                    
                    DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.6f, 0.8f).SetEase(Ease.OutQuad);
                }
                else if (player.curSide == Side.Right)
                {
                    Gate leftGate = GameObject.Find("LeftGate").GetComponent<Gate>();
                    leftGate.TakeDamage(leftGate.curHealth - 1);
                    MessageManager.instance.FinishHim();
                    
                    DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0.6f, 0.8f).SetEase(Ease.OutQuad);
                }
            }
        }

        public void CanStartWaveInvoke()
        {
            Invoke("CanStartWave", newWaveCooldown);
        }

        void CanStartWave()
        {
            canStartWave = true;
        }

        public void PlayerSoloDeathInvoke()
        {
            GameObject.Find("Player").SetActive(false);
            StopCoroutine(timerCoroutine);

            Invoke("PlayerSoloDeath", showDeathWindowIn);
            
            curLevelState = LevelState.End;
            AddMoney();
            FindFirstObjectByType<SceneChanger>().ActivateDirectorOutro();
            GameObject.Find("DeathTimerDisplay").GetComponent<TMP_Text>().text = curSeconds.ToString();
            GameObject.Find("DeathScoreDisplay").GetComponent<TMP_Text>().text = curScore.ToString();
            
            SFXManager.PlaySFX(playerDeathSFX, transform, 1f);
        }

        void PlayerSoloDeath()
        {
            WindowManager windownScript = GetComponent<WindowManager>();
            windownScript.OpenPlayerDeathWindow();
        }

        public void AddMoney()
        {
            GameManager.instance.AddMoney(curScore);
        }

    #endregion

    #region LEVEL DATA

        public void AddScore(int scoreAmount)
        {
            curScore += scoreAmount;
            UpdateScore(curScore);
        }

        void UpdateScore(int scoreAmount)
        {
            scoreTMP.text = scoreAmount.ToString();
        }

        public void UpdatePlayerHealth()
        {
            Player playerScript = GameObject.Find("Player").GetComponent<Player>();
            
            if (playerScript.curSide == Side.Left)
            {
                Gate leftGateScript = GameObject.Find("LeftGate").GetComponent<Gate>();            
                healthTMP.text = leftGateScript.curHealth.ToString();
            }
            else if (playerScript.curSide == Side.Right)
            {
                Gate rightGateScript = GameObject.Find("RightGate").GetComponent<Gate>();            
                healthTMP.text = rightGateScript.curHealth.ToString();
            }
        }

        private IEnumerator Timer()
        {
            while (true)
            {
                int minutes = curSeconds / 60;
                int seconds = curSeconds % 60;

                string minutesText = "";
                string secondsText = "";

                if (minutes < 10) minutesText = "0" + minutes.ToString();
                else minutesText = minutes.ToString();
                
                if (seconds < 10) secondsText = "0" + seconds.ToString();
                else secondsText = seconds.ToString();
                
                timeTMP.text = $"{minutesText}:{secondsText}";

                yield return new WaitForSeconds(1f);

                curSeconds++;
            }
        }

    #endregion

}