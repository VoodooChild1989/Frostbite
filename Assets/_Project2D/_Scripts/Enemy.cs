using UnityEngine;
using System.Collections;

public abstract class Enemy : Character
{

    #region FIELDS

        [Header("ENEMY")]

            [Header("Reward")]
            public int rewardScore;

            [Header("Navigation")]
            public float attentionRange;
            public float defaultStep;
            public float runningStep;
            public float maxChaseDuration;
            private float curChaseDuration;
            public float chaseCooldown;
            public float maxWanderDuration;
            private float curWanderDuration;
            private Vector2 randomDirection = Vector2.zero;
            
            [Header("Rotation")]
            public float maxRotationDuration;
            private float curRotationDuration;
            private float randRotation;
            public float rotationSpeed;
            private bool canRotate;
            public float canRotateCooldown;
            private Coroutine canRotateCoroutine;

    #endregion

    #region LIFE CYCLE METHODS

        public void EnemyAwake()
        {
            base.CharacterAwake();
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        public void EnemyStart()
        {
            base.CharacterStart();

            curChaseDuration = maxChaseDuration;
            curRotationDuration = maxRotationDuration;
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        void EnemyUpdate()
        {
            // Add your per-frame logic here.
            // Example: Move objects, check user input, update animations, etc.
        }

        /// <summary>
        /// Called at fixed intervals, ideal for physics updates.
        /// Use this for physics-related updates like applying forces or handling Rigidbody physics.
        /// </summary>
        public void EnemyFixedUpdate()
        {
            // Add physics-related logic here.
            // Example: Rigidbody movement, applying forces, or collision detection.
        }

    #endregion

    #region CUSTOM METHODS

        public override void Move()
        {
            Vector2 currentPos = rb.position;
            Vector2 targetPos;
            float step = defaultStep;

            float distanceToBall = Vector2.Distance(currentPos, ballPos.position);

            GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
            float minDistanceToCol = 100f;
            Vector2 colTransform = Vector2.zero;

            foreach(GameObject col in collectibles)
            {   
                float distance = Vector2.Distance(currentPos, col.transform.position);

                if (distance < minDistanceToCol)
                {
                    minDistanceToCol = distance;
                    colTransform = col.transform.position;
                }
            }

            if (distanceToBall <= attentionRange && curChaseDuration > 0f)
            {
                targetPos = ballPos.position;
                step = runningStep;
                
                curChaseDuration -= Time.fixedDeltaTime;

                if (curChaseDuration < 0f) 
                {
                    Invoke("ResetChase", chaseCooldown);   
                }
            }
            else if (minDistanceToCol <= attentionRange)
            {
                targetPos = colTransform;
                step = runningStep;
            }
            else
            {
                if (curWanderDuration <= 0f)
                {
                    randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
                    curWanderDuration = maxWanderDuration;
                }   

                targetPos = currentPos + randomDirection;
                curWanderDuration -= Time.fixedDeltaTime;
            }

            rb.MovePosition(Vector2.MoveTowards(currentPos, targetPos, step));    
        }

        void ResetChase()
        {
            curChaseDuration = maxChaseDuration;
        }

        public override void Rotate()
        {
            if (canRotate)
            {
                if (curRotationDuration < 0)
                {
                    randRotation = UnityEngine.Random.Range(-45, 45f);

                    curRotationDuration = maxRotationDuration;
                }
                else
                {
                    curRotationDuration -= Time.fixedDeltaTime;
                }

                rb.MoveRotation(rb.rotation + randRotation * rotationSpeed);
            }
            else
            {
                if (canRotateCoroutine == null)
                    canRotateCoroutine = StartCoroutine(ResetCanRotate());

                rb.MoveRotation(rb.rotation);
            }
        }

        IEnumerator ResetCanRotate()
        {
            yield return new WaitForSeconds(canRotateCooldown);
            
            canRotate = true;
        }

        void OnEnable()
        {
            base.OnDied += EnemyDied;
        }

        void OnDisable()
        {
            base.OnDied -= EnemyDied;
        }

        void EnemyDied()
        {
            LevelManager.instance?.RemoveEnemy(gameObject);
            
            LevelManager.instance.AddScore(rewardScore);
        }

    #endregion

}