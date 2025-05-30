using System.Collections;
using UnityEngine;

public class CollectiblesSpawner : MonoBehaviour
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public GameObject[] collectibles;
            public float rangeX;
            public float rangeXDeviation;
            public float averageGravity;
            public float averageGravityDeviation;
            public float averageSpawnCooldown;
            public float averageSpawnCooldownDeviation;

    #endregion

    #region LIFE CYCLE METHODS

        /// <summary>
        /// Called before the first frame update.
        /// Useful for initialization once the game starts.
        /// </summary>
        IEnumerator Start()
        {
            while (true)
            {
                float cooldown = UnityEngine.Random.Range(averageSpawnCooldown - averageSpawnCooldownDeviation, averageSpawnCooldown + averageSpawnCooldownDeviation);   
                
                yield return new WaitForSeconds(cooldown);   

                float posX = UnityEngine.Random.Range(rangeX - rangeXDeviation, rangeX + rangeXDeviation);
                float posY = transform.position.y;
                Vector3 spawnPos = new Vector3(posX, posY, 0f);

                GameObject selectedObj = collectibles[UnityEngine.Random.Range(0, collectibles.Length)];
                GameObject obj = Instantiate(selectedObj, spawnPos, Quaternion.identity);

                obj.GetComponent<Rigidbody2D>().gravityScale = UnityEngine.Random.Range(averageGravity - averageGravityDeviation, averageGravity + averageGravityDeviation);
            }
        }

    #endregion

}