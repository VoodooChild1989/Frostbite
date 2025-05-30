using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class ObstacleManager : MonoBehaviour
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public GameObject[] obstacles;
            public float moveDistance = 1f;
            public float moveInterval = 1f;
            public Vector2 minBounds = new Vector2(-5, -5);
            public Vector2 maxBounds = new Vector2(5, 5);

            private Dictionary<GameObject, Vector2> lastDirections = new Dictionary<GameObject, Vector2>();
            private Vector2[] directions = new Vector2[] {
                Vector2.up,
                Vector2.down,
                Vector2.left,
                Vector2.right
            };

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        private void Awake()
        {
            // Initialize variables or cache references here.
            // Example: Setting up components or data before start is called. 
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        private void Start()
        {
            foreach (var obstacle in obstacles)
            {
                lastDirections[obstacle] = Vector2.zero;
            }

            // InvokeRepeating(nameof(MoveObstacles), 1f, moveInterval);
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        private void Update()
        {
            // Add your per-frame logic here.
            // Example: Move objects, check user input, update animations, etc.
        }

        /// <summary>
        /// Called at fixed intervals, ideal for physics updates.
        /// Use this for physics-related updates like applying forces or handling Rigidbody physics.
        /// </summary>
        private void FixedUpdate()
        {
            // Add physics-related logic here.
            // Example: Rigidbody movement, applying forces, or collision detection.
        }

    #endregion

    #region CUSTOM METHODS

        public void MoveObstacles()
        {
            foreach (var obstacle in obstacles)
            {
                List<Vector2> availableDirs = new List<Vector2>(directions);
                availableDirs.Remove(lastDirections[obstacle]);

                Vector2 chosenDir = Vector2.zero;
                Vector3 currentPos = obstacle.transform.position;

                while (availableDirs.Count > 0)
                {
                    int idx = UnityEngine.Random.Range(0, availableDirs.Count);
                    chosenDir = availableDirs[idx];
                    availableDirs.RemoveAt(idx);

                    Vector3 targetPos = currentPos + (Vector3)(chosenDir * moveDistance);

                    if (IsInBounds(targetPos))
                    {
                        lastDirections[obstacle] = chosenDir;
                        StartCoroutine(SmoothMove(obstacle, targetPos));
                        break;
                    }
                }
            }
        }

        private IEnumerator SmoothMove(GameObject obj, Vector3 targetPos)
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogWarning($"Obstacle {obj.name} has no Rigidbody2D!");
                yield break;
            }

            float elapsedTime = 0f;
            float duration = 0.3f;
            Vector3 startPos = rb.position;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                Vector3 newPos = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
                rb.MovePosition(newPos);
                yield return null;
            }

            rb.MovePosition(targetPos);
        }

        bool IsInBounds(Vector3 pos)
        {
            return pos.x >= minBounds.x && pos.x <= maxBounds.x &&
                pos.y >= minBounds.y && pos.y <= maxBounds.y;
        }

    #endregion

}