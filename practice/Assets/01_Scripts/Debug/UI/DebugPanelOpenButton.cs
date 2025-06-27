using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanelOpenButton : UIButton
{
    public GameObject DebugPanel;

#if DEBUG_ON
        private void Start()
        {
            DebugPanel.SetActive(false);
        }

        protected override void OnClicked()
        {
            if (DebugPanel.activeSelf == true)
                DebugPanel.SetActive(false);
            else
                DebugPanel.SetActive(true);
        }
#endif
}
