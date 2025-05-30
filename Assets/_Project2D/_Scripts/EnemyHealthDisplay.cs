using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyHealthDisplay : MonoBehaviour
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            private Slider slider;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        private void Awake()
        {
            slider = GetComponent<Slider>();
        }
        
        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        private void Start()
        {
            slider.value = 1f;
        }

    #endregion

    #region CUSTOM METHODS

        /// <summary>
        /// An example custom method.
        /// Replace with your own custom logic.
        /// </summary>
        public void UpdateHealth()
        {   
            Player playerScript = GameObject.Find("Player").GetComponent<Player>();
            
            if (playerScript.curSide == Side.Left)
            {
                Gate rightGateScript = GameObject.Find("RightGate").GetComponent<Gate>();       
                float newValue = (float)rightGateScript.curHealth / (float)rightGateScript.maxHealth;
                DOTween.To(() => slider.value, x => slider.value = x, newValue, 0.5f).SetEase(Ease.OutQuad);
            }
            else if (playerScript.curSide == Side.Right)
            {
                Gate leftGateScript = GameObject.Find("LeftGate").GetComponent<Gate>();            
                float newValue = (float)leftGateScript.curHealth / (float)leftGateScript.maxHealth;
                DOTween.To(() => slider.value, x => slider.value = x, newValue, 0.5f).SetEase(Ease.OutQuad);
            }
        }

    #endregion

}