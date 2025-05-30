using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class MessageManager : MonoBehaviour
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public TMP_Text messageTMP;
            public float removeIn;
            public AudioClip messageSFX;
            private WindowManager windowManager;
            public static MessageManager instance;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        private void Awake()
        {
            SingletonUtility.MakeSingleton(ref instance, this, false);
            windowManager = GetComponent<WindowManager>();
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        private void Start()
        {
            windowManager.rectTransform.anchoredPosition = new Vector2(0f, 1000f);
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

        public void NewWave()
        {
            messageTMP.text = "New Wave!";
            Activate();
        }
        
        public void FinishHim()
        {
            messageTMP.text = "Finish Him!";
            Activate();
        }

        void Activate()
        {
            windowManager.OpenMessageWindow();
            Invoke("Back", removeIn);
        }

        void Back()
        {
            windowManager.CloseMessageWindow();
        }

        /// <summary>
        /// An example coroutine that waits for 2 seconds.
        /// </summary>
        private IEnumerator ExampleCoroutine()
        {
            // Wait for 2 seconds before executing further code.
            yield return new WaitForSeconds(2f);

            Debug.Log("Action after 2 seconds.");
        }

    #endregion

}