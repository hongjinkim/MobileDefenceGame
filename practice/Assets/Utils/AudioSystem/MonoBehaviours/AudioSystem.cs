using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace POOM
{
    public class AudioSystem : MonoBehaviour
    {
        private static AudioSystem Instance { get; set; } = null;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

    }

}
