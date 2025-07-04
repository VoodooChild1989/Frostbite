using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class Levitation : MonoBehaviour
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public float moveSpeed;
            public float amplitude = 1f; 
            public float delay; 
            private float startY; 
            private RectTransform rectTransform;

    #endregion

    #region LIFE CYCLE METHODS

        void Awake()
        {
            if (gameObject.layer == LayerMask.NameToLayer("UI"))
                rectTransform = GetComponent<RectTransform>();
        }

        void Start()
        {
            Invoke("LevitateInvoke", delay);
        }
        
        void LevitateInvoke()
        {
            if (gameObject.layer == LayerMask.NameToLayer("UI"))
                startY = rectTransform.anchoredPosition.y;
            else 
                startY = transform.position.y;
                
            StartCoroutine(Levitate());
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        IEnumerator Levitate()
        {
            while (true)
            {
                float valueY = Mathf.Sin((Time.time - delay) * moveSpeed) * amplitude;
                
                if (gameObject.layer == LayerMask.NameToLayer("UI"))
                    rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, startY + valueY);
                else 
                    transform.position = new Vector2(transform.position.x, startY + valueY);

                yield return null;
            }
        }

    #endregion

}