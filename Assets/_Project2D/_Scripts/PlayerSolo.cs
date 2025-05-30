using UnityEngine;

public class PlayerSolo : Player
{

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        void Awake()
        {
            base.PlayerAwake();

            curHealth = maxHealth; 
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        void Start()
        {
            base.PlayerStart();
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        void Update()
        {
            base.PlayerUpdate();
        }

        /// <summary>
        /// Called at fixed intervals, ideal for physics updates.
        /// Use this for physics-related updates like applying forces or handling Rigidbody physics.
        /// </summary>
        void FixedUpdate()
        {
            base.PlayerFixedUpdate();
        }

    #endregion

}