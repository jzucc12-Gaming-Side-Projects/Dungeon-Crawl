using UnityEngine;
using UnityEngine.UI;

namespace JZ.UI
{
    /// <summary>
    /// <para>Ensures content size fitters are the proper size</para>
    /// </summary>
    public class ContentSizeFitterUpdater : MonoBehaviour
    {
        private void Start()
        {
            UpdateContentSizeFitter();
        }

        public void UpdateContentSizeFitter()
        {
            foreach(var layout in GetComponentsInChildren<LayoutGroup>())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());
            }
        }
    }
}