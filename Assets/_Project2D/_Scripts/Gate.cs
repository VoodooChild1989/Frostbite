using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class Gate : MonoBehaviour
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public Coroutine scaler;
            
        [Header("HEALTH")]

            [Header("Basic Variables")]
            public int maxHealth;
            public int curHealth;
            public EnemyHealthDisplay enemyHealthDisplay;

            [Header("SFX")]
            public AudioClip playerDamagedSFX;
            public AudioClip enemyDamagedSFX;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        void Awake()
        {
            // Initialize variables or cache references here.
            // Example: Setting up components or data before start is called.
            
            // Example of adding a component.
            // MyGame.Utils.AddComponent<SpriteRenderer>(out spriteRenderer, gameObject, this.GetType().Name);   
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        void Start()
        {
            curHealth = maxHealth;

            enemyHealthDisplay = FindFirstObjectByType<EnemyHealthDisplay>();
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        void Update()
        {
            // Add your per-frame logic here.
            // Example: Move objects, check user input, update animations, etc.
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

    #region HEALTH
    
        public void CheckHealth()
        {
            Player playerScript = FindFirstObjectByType<Player>();

            if (((playerScript.curSide == Side.Right) && (name == "RightGate")) || ((playerScript.curSide == Side.Left) && (name == "LeftGate")))
            {
                SFXManager.PlaySFX(playerDamagedSFX, transform, 1f);
                LevelManager.instance.UpdatePlayerHealth();
            }
            else
            {
                SFXManager.PlaySFX(enemyDamagedSFX, transform, 1f);
                enemyHealthDisplay.UpdateHealth();
            }
        }

        public void TakeDamage(int damageAmount)
        {
            if (LevelManager.instance.curLevelState == LevelState.End) return;

            UpdateHealth(-damageAmount);
            CheckHealth();
            
            if (curHealth <= 0) Death();
        }

        public void Heal(int healAmount)
        {
            UpdateHealth(healAmount);

            if (curHealth > maxHealth) curHealth = maxHealth;
            
            CheckHealth();
        }

        void UpdateHealth(int amount)
        {
            curHealth += amount;
        }

        void Death()
        {
            Player playerScript = FindFirstObjectByType<Player>();

            if (((playerScript.curSide == Side.Right) && (name == "RightGate")) || ((playerScript.curSide == Side.Left) && (name == "LeftGate")))
            {
                LevelManager.instance.PlayerSoloDeathInvoke();
            }
            else
            {
                LevelManager.instance.CanStartWaveInvoke();
                curHealth = UnityEngine.Random.Range(10, 15);
                CheckHealth();
            }   
        }

    #endregion

    #region COLLECTIBLES

        /// <summary>
        /// An example custom method.
        /// Replace with your own custom logic.
        /// </summary>
        public void Wrap(float minScale, float maxScale, float duration)
        {
            if (scaler != null) return;

            float newScale = UnityEngine.Random.Range(minScale, maxScale);

            if ((newScale >= 0.7f) && (newScale <= 1.3f))
            {
                newScale = 2f;   
            }

            scaler = StartCoroutine(ScaleTimer(newScale, duration));
        }

        /// <summary>
        /// An example coroutine that waits for 2 seconds.
        /// </summary>
        IEnumerator ScaleTimer(float targetScale, float duration)
        {
            Vector3 originalScale = transform.localScale;

            while (transform.localScale.x != targetScale)
            {
                Vector3 cur = transform.localScale;
                Vector3 target = new Vector3(targetScale, transform.localScale.y, transform.localScale.z);
                float step = 0.1f;

                transform.localScale = Vector3.MoveTowards(cur, target, step);
                
                yield return new WaitForSeconds(0.05f);
            }

            transform.localScale = new Vector3(targetScale, transform.localScale.y, transform.localScale.z);

            yield return new WaitForSeconds(duration);
            
            while (transform.localScale != originalScale)
            {
                Vector3 cur = transform.localScale;
                Vector3 target = originalScale;
                float step = 0.1f;

                transform.localScale = Vector3.MoveTowards(cur, target, step);
                
                yield return new WaitForSeconds(0.05f);
            }
            
            transform.localScale = originalScale;
            scaler = null;
        }

    #endregion

}