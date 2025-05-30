using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class TextureScroller : MonoBehaviour
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public float scrollSpeedX = 0.5f;
            public float scrollSpeedY = 0f;
            private Renderer rend;
            private Vector2 offset;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        void Awake()
        {
            rend = GetComponent<Renderer>();
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        void Update()
        {
            offset.x += scrollSpeedX * Time.deltaTime;
            offset.y += scrollSpeedY * Time.deltaTime;
            rend.material.mainTextureOffset = offset;
        }

    #endregion

}