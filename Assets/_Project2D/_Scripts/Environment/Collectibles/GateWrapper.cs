using UnityEngine;

public class GateWrapper : Collectible
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public float minScale;
            public float maxScale;
            public float minDuration;
            public float maxDuration;

    #endregion

    #region CUSTOM METHODS

        /// <summary>
        /// An example custom method.
        /// Replace with your own custom logic.
        /// </summary>
        public override void OnCollected(GameObject collector)
        {
            Gate[] gates = FindObjectsByType<Gate>(FindObjectsSortMode.None);
            float randomDuration = UnityEngine.Random.Range(minDuration, maxDuration);
            
            if (gates[0].scaler == null)
            {
                gates[0].Wrap(minScale, maxScale, randomDuration);
            }
            else if (gates[1].scaler == null)
            {
                gates[1].Wrap(minScale, maxScale, randomDuration);
            }
        }

    #endregion

}