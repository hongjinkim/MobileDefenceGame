using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLineOpenButton : UIButton
{
    private bool IsOpen = true;
    List<GameObject> Buttons = new List<GameObject>();

#if DEBUG_ON
        private void Awake()
        {
            for (int i = 1; i < transform.parent.childCount; i++)
            {
                Buttons.Add(transform.parent.GetChild(i).gameObject);
            }
        }

        protected override void OnClicked()
        {
            if (IsOpen)
            {
                IsOpen = false;
                Buttons.ForEach(x => x.SetActive(false));
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                IsOpen = true;
                Buttons.ForEach(x => x.SetActive(true));
                transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
        }
#endif
}
