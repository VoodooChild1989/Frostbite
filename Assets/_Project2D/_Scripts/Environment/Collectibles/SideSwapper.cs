using UnityEngine;

public class SideSwapper : Collectible
{

    #region FIELDS

        [Header("NOTES")] [TextArea(4, 10)]
        public string notes;

        [Header("VARIABLES")]
            
            [Header("Basic Variables")]
            private int field;

    #endregion

    #region CUSTOM METHODS

        /// <summary>
        /// An example custom method.
        /// Replace with your own custom logic.
        /// </summary>
        public override void OnCollected(GameObject collector)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Character");
            foreach (GameObject obj in objs)
            {
                obj.GetComponent<ISideSwapper>().ChangeSides();
            }
            
            Gate[] gates = Object.FindObjectsByType<Gate>(FindObjectsSortMode.None);
            Gate leftGate = null;
            Gate rightGate = null;
            foreach (Gate gate in gates)
            {
                if (gate.gameObject.name == "RightGate") rightGate = gate;
                else if (gate.gameObject.name == "LeftGate") leftGate = gate;
            }

            int leftGateCurHealth = leftGate.curHealth;
            int leftGateMaxHealth = leftGate.maxHealth;
            int rightGateCurHealth = rightGate.curHealth;
            int rightGateMaxHealth = rightGate.maxHealth;

            leftGate.maxHealth = rightGateMaxHealth;
            leftGate.curHealth = rightGateCurHealth;
            rightGate.maxHealth = leftGateMaxHealth;
            rightGate.curHealth = leftGateCurHealth;
        }

    #endregion

}