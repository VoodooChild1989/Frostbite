using UnityEngine;

public class Heal : Collectible
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            public int minHeal;
            public int maxHeal;

    #endregion

    #region CUSTOM METHODS

        /// <summary>
        /// An example custom method.
        /// Replace with your own custom logic.
        /// </summary>
        public override void OnCollected(GameObject collector)
        {
            int randomHeal = UnityEngine.Random.Range(minHeal, maxHeal);

            Gate[] gates = FindObjectsByType<Gate>(FindObjectsSortMode.None);
            int randomIndex = UnityEngine.Random.Range(1, gates.Length);
            gates[randomIndex].Heal(randomHeal);
        }

    #endregion

}