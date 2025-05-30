using UnityEngine;

public abstract class Player : Character
{

    #region FIELDS

        [Header("MOVEMENT")]
            
            [Header("Input Keys")]
            public KeyCode upKey;
            public KeyCode downKey;
            public KeyCode rightKey;
            public KeyCode leftKey;
            private Vector2 input;

            [Header("Movement")]
            private Vector2 moveDirection;

            [Header("Rotation")]
            public float rotationSpeed;
            private float scroll;
            private float rotationAmount;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        public void PlayerAwake()
        {
            base.CharacterAwake();
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        public void PlayerStart()
        {
            base.CharacterStart();
        }

        /// <summary>
        /// Called once per frame.
        /// Use for logic that needs to run every frame, such as user input or animations.
        /// </summary>
        public void PlayerUpdate()
        {
            GetInput();
        }

        /// <summary>
        /// Called at fixed intervals, ideal for physics updates.
        /// Use this for physics-related updates like applying forces or handling Rigidbody physics.
        /// </summary>
        public void PlayerFixedUpdate()
        {
            Move();
            Rotate();
        }

    #endregion

    #region MOVEMENT

        void GetInput()
        {
            // Keys
            float verInput = 0f;
            if (Input.GetKey(upKey)) verInput += 1f;
            else if (Input.GetKey(downKey)) verInput -= 1f;

            float horInput = 0f;
            if (Input.GetKey(rightKey)) horInput += 1f;
            else if (Input.GetKey(leftKey)) horInput -= 1f;

            input = new Vector2(horInput, verInput).normalized;

            // Scroll wheel
            scroll = Input.GetAxis("Mouse ScrollWheel");
            rotationAmount = scroll * rotationSpeed * Time.fixedDeltaTime;
        }

        public override void Move()
        {
            if (input == Vector2.zero) return;

            Vector2 moveAmount = input * moveSpeed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + moveAmount);
        }

        public override void Rotate()
        {
            if (rotationAmount == 0) return;

            rb.MoveRotation(rb.rotation + rotationAmount);
        }

    #endregion

}