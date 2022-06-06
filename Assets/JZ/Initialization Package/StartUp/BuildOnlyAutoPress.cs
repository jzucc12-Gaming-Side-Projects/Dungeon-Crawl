using UnityEngine;
using UnityEngine.UI;

namespace JZ.STARTUP
{
    /// <summary>
    /// <para>Button that automatically presses in game builds</para>
    /// <para>Main use is to auto-transition out of the initialization scene in game builds</para>
    /// </summary>
    public class BuildOnlyAutoPress : MonoBehaviour
    {
        [SerializeField] private Button button = null;

        #if !UNITY_EDITOR
        private void Start()
        {
            button.onClick?.Invoke();
        }
        #endif
    }
}