using UnityEngine;
using JZ.AUDIO;

namespace JZ.STARTUP
{
    /// <summary>
    /// <para>Runs at the start of the game</para>
    /// <para>Place in the initialization scene</para>
    /// </summary>
    public class GameStartUp : MonoBehaviour
    {
        [SerializeField] private int frameRate = 60;
        [SerializeField] private bool runInBackground = true;

        private void Awake()
        {
            Application.targetFrameRate = frameRate;
            Application.runInBackground = runInBackground;
            JZAudioSettings.Initialize();
        }
    }
}
