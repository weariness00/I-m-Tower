using TMPro;
using UnityEngine;

namespace Tower
{
    public class TowerPointView : MonoBehaviour
    {
        public TowerPointData pointData;
        public TMP_Text techPointText;
        public TMP_Text bioPointText;
        
        private void Start()
        {
            if (pointData == null)
            {
                pointData = GetComponent<TowerPointData>();
            }
        }
    }
}