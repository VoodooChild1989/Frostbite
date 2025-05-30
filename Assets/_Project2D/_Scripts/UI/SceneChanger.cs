using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

public class SceneChanger : MonoBehaviour
{

    #region BASIC FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public PlayableDirector levelOutroDirector;
            public Animator levelOutroAnimator;
            public bool playDirector;
            public bool playAnimator;

    #endregion

    private void Start()
    {
        if (levelOutroAnimator != null)
        {
            levelOutroAnimator.Play("EndFade");      
        }

        if (levelOutroDirector == null) playDirector = false;
        if (levelOutroAnimator == null) playAnimator = false;
    }

    #region CUSTOM METHODS

        /// <summary>
        /// Method used to trigger the coroutine with the level outro animation.
        /// </summary>
        public void ChangeScene(string sceneName)
        {
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, 0.3f).SetEase(Ease.OutQuad);
            AudioSource music = GameObject.Find("Music").GetComponent<AudioSource>();
            DOTween.To(() => music.pitch, x => music.pitch = x, 1f, 0.8f).SetEase(Ease.OutQuad);

            if ((!playAnimator) && (!playDirector))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }
            else
            {
                StartCoroutine(StartChangingScene(sceneName));
            }
        }

        /// <summary>
        /// The coroutine with the level outro animation.
        /// </summary>
        private IEnumerator StartChangingScene(string sceneName)
        {
            if (playAnimator)
            {
                ActivateAnimatorOutro();
            }
            if (playDirector)
            {
                ActivateDirectorOutro();
            }

            yield return new WaitForSeconds(1f);

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public void ActivateAnimatorOutro()
        {   
            levelOutroAnimator.Play("StartFade");   
        }

        public void ActivateDirectorOutro()
        {   
            levelOutroDirector.Play();
        }

    #endregion

}