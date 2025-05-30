using UnityEngine;
using System.Collections;

public class CharacterScaler : Collectible
{

    #region FIELDS
    
        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public int minScale;
            public int maxScale;
            public float minDuration;
            public float maxDuration;

    #endregion

    #region CUSTOM METHODS

        public override void OnCollected(GameObject collector)
        {
            float randomDuration = UnityEngine.Random.Range(minDuration, maxDuration);
            
            collector.GetComponent<ICharacterScaler>()?.Scale(minScale, maxScale + 1, randomDuration);
        }

    #endregion

}