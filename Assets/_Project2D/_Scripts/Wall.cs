using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class Wall : MonoBehaviour
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public float moveSpeed;
            private Rigidbody2D rb;
            private Coroutine move;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        void Start()
        {
            // Perform initial setup that occurs when the game starts.
            // Example: Initialize game state, start coroutines, load resources, etc.
            
            // Example of starting a coroutine.
            // StartCoroutine(ExampleCoroutine());
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

    #region CUSTOM METHODS

        /// <summary>
        /// An example custom method.
        /// Replace with your own custom logic.
        /// </summary>
        public void StartMovement(Vector3 newPos)
        {
            if (move != null) return;

            move = StartCoroutine(Move(newPos));
        }

        /// <summary>
        /// An example coroutine that waits for 2 seconds.
        /// </summary>
        IEnumerator Move(Vector3 newPos)
        {
            float timeStep = 0.1f;

            while (Mathf.Abs(rb.position.x - newPos.x) > 0.1f)
            {
                Vector2 moveAmount = Vector2.zero;

                if (rb.position.x > newPos.x)
                {
                    moveAmount = new Vector2(-1f, rb.position.y) * moveSpeed * timeStep;
                }
                else if (rb.position.x < newPos.x)
                {
                    moveAmount = new Vector2(1f, rb.position.y) * moveSpeed * timeStep;
                }
                
                rb.MovePosition(rb.position + moveAmount);

                yield return timeStep;
            }
            
            rb.MovePosition(newPos);
            move = null;
        }

    #endregion

}