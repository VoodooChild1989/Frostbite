using UnityEngine;

public interface ICollectible
{
    void OnCollected(GameObject collector);
}

public abstract class Collectible : MonoBehaviour, ICollectible
{
    public AudioClip collectedSFX;


    #region CUSTOM METHODS

        public abstract void OnCollected(GameObject collector);
        
        /// <summary>
        /// Sent when another object enters a trigger collider attached to this
        /// object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision.</param>
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Character"))
            {
                OnCollected(other.gameObject);
                SFXManager.PlaySFX(collectedSFX, transform, 0.2f);
                Destroy(gameObject);
            }
            else if (other.gameObject.name == "CollectibleDestroyer")
            {
                Destroy(gameObject);
            }
        }

    #endregion

}