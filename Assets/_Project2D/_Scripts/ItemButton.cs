using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using TMPro;

public class ItemButton : MonoBehaviour
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public int cost;
            public string itemName;
            private CanvasGroup canvasGroup;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        private void Awake()
        {
            canvasGroup = GetComponentInParent<CanvasGroup>();
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        private void Start()
        {
            if (PlayerPrefs.HasKey(itemName)) Bought();
        }

    #endregion

    #region CUSTOM METHODS

        public void Buy()
        {
            if (GameManager.instance.curMoney < cost) return;
            
            GameManager.instance.RemoveMoney(cost);
            PlayerPrefs.SetInt(itemName, 1);   
            Bought();
        }

        private void Bought()
        {
            canvasGroup.alpha = 0.3f;
            canvasGroup.interactable = false;
        }

    #endregion

}