using UnityEngine;

public class Ball : MonoBehaviour
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public float minSpeed;
            public float maxSpeed;
            public int damage;
            public Transform initialPos;
            private Rigidbody2D rb;

            [Header("SFX")]
            public AudioClip ballBounce;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called when the script instance is being loaded.
        /// Useful for initialization before the game starts.
        /// </summary>
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        void Start()
        {
            //Launch(0);
        }

        private void FixedUpdate() 
        {
            ClampVelocity();   
        }

    #endregion

    #region CUSTOM METHODS

        // 0 - Start
        // 1 - To left
        // 2 - To Right
        public void Launch(int id)
        {
            float dirX = 0f;
            float dirY = 0f;
            transform.position = initialPos.position;

            if (id == 0)
            {
                dirX = UnityEngine.Random.Range(-1f, 1f);
            }
            else if (id == 1)
            {
                dirX = UnityEngine.Random.Range(-1f, 0f);
            }
            else if (id == 2)
            {
                dirX = UnityEngine.Random.Range(0f, 1f);
            }
            
            dirY = UnityEngine.Random.Range(-0.7f, 0.7f);

            Vector3 dir = new Vector3(dirX, dirY, 0f);
            rb.linearVelocity = dir.normalized * minSpeed;
        }

        private void ClampVelocity()
        {
            if (rb.linearVelocity.magnitude < minSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * minSpeed;
            }
            else if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }

        void PlaySFX()
        {
            SFXManager.PlaySFX(ballBounce, transform, 0.3f);
        }

        /// <summary>
        /// Sent when an incoming collider makes contact with this object's
        /// collider (2D physics only).
        /// </summary>
        /// <param name="other">The Collision2D data associated with this collision.</param>
        void OnCollisionEnter2D(Collision2D other)
        {
            PlaySFX();

            if (other.gameObject.CompareTag("Gate"))
            {
                string gateName = other.gameObject.name;
                Player playerScript = FindFirstObjectByType<Player>();
                Gate gate = other.gameObject.GetComponent<Gate>();

                if (playerScript.curSide == Side.Right)
                {
                    if (gateName == "RightGate")
                    {
                        gate.TakeDamage(damage);  
                        Launch(1);
                    }
                    else if (gateName == "LeftGate")
                    {
                        gate.TakeDamage(damage);  
                        LevelManager.instance.AddScore(100);
                        Launch(2);
                    }
                }
                else if (playerScript.curSide == Side.Left)
                {
                    if (gateName == "RightGate")
                    {
                        gate.TakeDamage(damage);  
                        LevelManager.instance.AddScore(100);              
                        Launch(1);
                    }
                    else if (gateName == "LeftGate")
                    {
                        gate.TakeDamage(damage);     
                        Launch(2);
                    }
                }
            }
            else if (other.gameObject.CompareTag("Character"))
            {
                other.gameObject.GetComponent<IHealth>().TakeDamage(damage);   
            }
        }

    #endregion

}