using UnityEngine;

public class WallMover : Collectible
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public float rightBound;
            public float leftBound;
            private GameObject wallObj;
            private Vector3 newPos;

    #endregion

    #region CUSTOM METHODS

        /// <summary>
        /// An example custom method.
        /// Replace with your own custom logic.
        /// </summary>
        public override void OnCollected(GameObject collector)
        {
            wallObj = GameObject.Find("Wall");
            float additionalX = UnityEngine.Random.Range(2f, 4f);
            
            int minPlus = 0;
            int randValue = UnityEngine.Random.Range(1, 3);
            if (randValue == 1) minPlus = 1;
            else if (randValue == 2) minPlus = -1;

            float newX = wallObj.transform.position.x + additionalX * Mathf.Sign(minPlus);

            if (OutOfBounds(newX))
                newX = wallObj.transform.position.x + additionalX * -Mathf.Sign(minPlus);
            
            newPos = new Vector3(newX, wallObj.transform.position.y, 0f);

            wallObj.GetComponent<Wall>().StartMovement(newPos);
        }

        bool OutOfBounds(float newPosX)
        {
            if ((newPosX >= rightBound) || (newPosX <= leftBound))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    #endregion

}