using System;
using System.Collections;
using UnityEngine;

public enum Side
{
    Left, Right
}

public interface IHealth
{
    void TakeDamage(int damageAmount);
    void Heal(int healAmount);
}

public interface ICharacterScaler
{
    void Scale(int minScale, int maxScale, float duration);  
}

public interface ISideSwapper
{
    void ChangeSides();
}

public abstract class Character : MonoBehaviour, IHealth, ICharacterScaler, ISideSwapper
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public float moveSpeed;
            public Rigidbody2D rb;
            public Side curSide;

            [Header("Position Clamping")]
            public Transform ballPos;
            public Transform wallPos;

            [Header("Extra")]
            private Coroutine scaler;

        [Header("HEALTH")]

            [Header("Basic Variables")]
            public int maxHealth;
            public int curHealth;
            public event Action OnDamaged;
            public event Action OnDied;

    #endregion

    #region LIFE CYCLE METHODS

        public void CharacterAwake()
        {
            rb = GetComponent<Rigidbody2D>();
            
            ballPos = GameObject.Find("Ball").transform;
            wallPos = GameObject.Find("Wall").transform;
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        public void CharacterStart()
        {
            curHealth = maxHealth;
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        public void CharacterUpdate()
        {
            // Add your per-frame logic here.
            // Example: Move objects, check user input, update animations, etc.
        }

        /// <summary>
        /// Called at fixed intervals, ideal for physics updates.
        /// Use this for physics-related updates like applying forces or handling Rigidbody physics.
        /// </summary>
        public void CharacterFixedUpdate()
        {
            // Add physics-related logic here.
            // Example: Rigidbody movement, applying forces, or collision detection.
        }

    #endregion

    #region CUSTOM METHODS

        public abstract void Move();
        public abstract void Rotate();

    #endregion

    #region HEALTH
    
        public void TakeDamage(int damageAmount)
        {
            if (LevelManager.instance.curLevelState == LevelState.End) return;
            
            OnDamaged?.Invoke();

            UpdateHealth(-damageAmount);

            if (curHealth <= 0) Death();
        }

        public void Heal(int healAmount)
        {
            UpdateHealth(healAmount);

            if (curHealth > maxHealth) curHealth = maxHealth;
        }

        void UpdateHealth(int amount)
        {
            curHealth += amount;
        }

        public void Death()
        {
            OnDied?.Invoke();

            Destroy(gameObject);
        }

    #endregion

    #region COLLECTIBLES

        public void ChangeSides()
        {
            if (curSide == Side.Left)
            {
                curSide = Side.Right;
                transform.position = LevelManager.instance.SetSidePosition(curSide);
            }
            else if (curSide == Side.Right)
            {
                curSide = Side.Left;
                transform.position = LevelManager.instance.SetSidePosition(curSide);
            }
        }

        public void Scale(int minScale, int maxScale, float duration)
        {
            if (scaler != null) return;

            int value = UnityEngine.Random.Range(minScale, maxScale);
            Vector3 newScale = new Vector3(value, value, value);

            if (value != 1)
            {
                int randValue = UnityEngine.Random.Range(1, 3);
                if (randValue == 1) newScale = new Vector3(newScale.x - 0.5f, newScale.y - 0.5f, newScale.z - 0.5f); 
            }
            else
            {            
                newScale = new Vector3(0.5f, 0.5f, 0.5f);
            }

            scaler = StartCoroutine(ScaleTimer(newScale, duration));
        }

        IEnumerator ScaleTimer(Vector3 targetScale, float duration)
        {
            Vector3 originalScale = transform.localScale;

            while (transform.localScale != targetScale)
            {
                Vector3 cur = transform.localScale;
                Vector3 target = targetScale;
                float step = 0.3f;

                transform.localScale = Vector3.MoveTowards(cur, target, step);
                
                yield return new WaitForSeconds(0.05f);
            }

            transform.localScale = targetScale;

            yield return new WaitForSeconds(duration);
            
            while (transform.localScale != originalScale)
            {
                Vector3 cur = transform.localScale;
                Vector3 target = originalScale;
                float step = 0.3f;

                transform.localScale = Vector3.MoveTowards(cur, target, step);
                
                yield return new WaitForSeconds(0.05f);
            }
            
            transform.localScale = originalScale;
            scaler = null;
        }

    #endregion

}