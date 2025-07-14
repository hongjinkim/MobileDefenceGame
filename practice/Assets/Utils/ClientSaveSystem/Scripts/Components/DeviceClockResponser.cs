using System;
using UnityEngine;

namespace POOM
{
    public class DeviceClockResponser : MonoBehaviour, IClockResponser
    {
        [SerializeField]
        private string DateTimeFormat = "o";


        public string NowLocalString() => DateTimeOffset.Now.ToString(DateTimeFormat);

        public string NowUTCString() => DateTimeOffset.UtcNow.ToString(DateTimeFormat);
        
    }

}
