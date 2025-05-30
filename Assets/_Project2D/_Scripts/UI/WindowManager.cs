using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WindowManager : MonoBehaviour
{

    #region BASIC FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]            
            public GameObject window;
            private CanvasGroup canvasGroup;
            public RectTransform rectTransform;
            private bool isTweening;
            [SerializeField] private float tweenDuration = 0.5f;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        private void Awake()
        {
            rectTransform = window.GetComponent<RectTransform>();
            canvasGroup = window.GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        private void Start()
        {
            rectTransform.anchoredPosition = new Vector2(0f, -1000f);
            canvasGroup.alpha = 0f;
        }

    #endregion

    #region CUSTOM METHODS

        /// <summary>
        /// Opening a window.
        /// </summary>
        public void OpenWindow()
        {
            float curY = rectTransform.anchoredPosition.y;
            if (isTweening || ((curY > -999.5f) || (curY < -1000.5f))) return;

            isTweening = true;
            
            // RectTransform
            rectTransform.anchoredPosition = new Vector2(0f, 1000f);
            rectTransform.localScale = new Vector2(0.3f, 0.3f);

            // CanvasGroup
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;   
            canvasGroup.blocksRaycasts = true;
                        
            Sequence seq = DOTween.Sequence();
            seq.Append(rectTransform.DOAnchorPos(Vector2.zero, tweenDuration).SetEase(Ease.InOutSine));
            seq.Join(rectTransform.DOScale(0.9f, tweenDuration).SetEase(Ease.InOutSine));
            seq.Join(canvasGroup.DOFade(1f, tweenDuration).SetEase(Ease.InOutSine));
            seq.OnComplete(() =>
            {
                canvasGroup.interactable = true;
                isTweening = false;
            });
        }

        /// <summary>
        /// Closing a window.
        /// </summary>
        public void CloseWindow()
        { 
            float curY = rectTransform.anchoredPosition.y;
            if (isTweening || ((curY < -0.5f) || (curY > 0.5f))) return;

            isTweening = true;

            // RectTransform
            rectTransform.anchoredPosition = new Vector2(0f, 0f);
            rectTransform.localScale = new Vector2(0.9f, 0.9f);

            // CanvasGroup
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;   
            canvasGroup.blocksRaycasts = true;

            Sequence seq = DOTween.Sequence();
            seq.Append(rectTransform.DOAnchorPos(new Vector2(0f, -1000f), tweenDuration).SetEase(Ease.InOutSine));
            seq.Join(rectTransform.DOScale(0.3f, tweenDuration).SetEase(Ease.InOutSine));
            seq.Join(canvasGroup.DOFade(0f, tweenDuration).SetEase(Ease.InOutSine));
            seq.OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = false;
                isTweening = false;
            });
        }

        /// <summary>
        /// Opening a window.
        /// </summary>
        public void OpenPlayerDeathWindow()
        {
            float curY = rectTransform.anchoredPosition.y;
            if (isTweening || ((curY > -999.5f) || (curY < -1000.5f))) return;

            isTweening = true;
            
            // RectTransform
            rectTransform.anchoredPosition = new Vector2(0f, 1000f);
            rectTransform.localScale = new Vector2(0.3f, 0.3f);

            // CanvasGroup
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;   
            canvasGroup.blocksRaycasts = true;
                        
            Sequence seq = DOTween.Sequence();
            seq.Append(rectTransform.DOAnchorPos(Vector2.zero, tweenDuration).SetEase(Ease.InOutSine));
            seq.Join(rectTransform.DOScale(Vector3.one, tweenDuration).SetEase(Ease.InOutSine));
            seq.Join(canvasGroup.DOFade(1f, tweenDuration).SetEase(Ease.InOutSine));
            seq.OnComplete(() =>
            {
                canvasGroup.interactable = true;
                isTweening = false;
                
                DOTween.To(() => Time.timeScale, x => Time.timeScale = x, LevelManager.instance.timeStopValue, 0.8f).SetEase(Ease.OutQuad);
                AudioSource music = GameObject.Find("Music").GetComponent<AudioSource>();
                DOTween.To(() => music.pitch, x => music.pitch = x, 0.6f, 0.8f).SetEase(Ease.OutQuad);
            });
        }
    
        /// <summary>
        /// Opening a window.
        /// </summary>
        public void OpenMessageWindow()
        {
            float curY = rectTransform.anchoredPosition.y;
            if (isTweening || ((curY < 999.5f) || (curY > 1000.5f))) return;

            isTweening = true;

            SFXManager.PlaySFX(MessageManager.instance.messageSFX, transform, 1f);
            
            // RectTransform
            rectTransform.anchoredPosition = new Vector2(0f, 1000f);
            rectTransform.localScale = new Vector2(0.3f, 0.3f);

            // CanvasGroup
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;   
            canvasGroup.blocksRaycasts = true;
                        
            Sequence seq = DOTween.Sequence();
            seq.Append(rectTransform.DOAnchorPos(new Vector2(0f, 300f), tweenDuration).SetEase(Ease.InOutSine));
            seq.Join(rectTransform.DOScale(Vector3.one, tweenDuration).SetEase(Ease.InOutSine));
            seq.Join(canvasGroup.DOFade(1f, tweenDuration).SetEase(Ease.InOutSine));
            seq.OnComplete(() =>
            {
                canvasGroup.interactable = true;
                isTweening = false;
            });
        }
        
        /// <summary>
        /// Closing a window.
        /// </summary>
        public void CloseMessageWindow()
        { 
            float curY = rectTransform.anchoredPosition.y;
            if (isTweening || ((curY < 299.5f) || (curY > 300.5f))) return;

            isTweening = true;

            // RectTransform
            rectTransform.anchoredPosition = new Vector2(0f, 300f);
            rectTransform.localScale = new Vector2(1f, 1f);

            // CanvasGroup
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;   
            canvasGroup.blocksRaycasts = true;

            Sequence seq = DOTween.Sequence();
            seq.Append(rectTransform.DOAnchorPos(new Vector2(0f, 1000f), tweenDuration).SetEase(Ease.InOutSine));
            seq.Join(rectTransform.DOScale(0.3f, tweenDuration).SetEase(Ease.InOutSine));
            seq.Join(canvasGroup.DOFade(0f, tweenDuration).SetEase(Ease.InOutSine));
            seq.OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = false;
                isTweening = false;
            });
        }

    #endregion

}