using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using VTLTools;
using UnityEngine.UI;
using AntiStress.UI;
namespace DA.UI.Utilities
{
    public class PopupManager : Singleton<PopupManager>
    {
        private Dictionary<PopupType, PopupBase> cachePopups = new Dictionary<PopupType, PopupBase>();
        private Stack<PopupBase> popupStack = new Stack<PopupBase>();

        public void ShowPopup(PopupType type, bool closeCurrent = false)
        {
            if (popupStack.Count > 0 && cachePopups[type] == popupStack.Peek())
            {
                return;
            }
            if (closeCurrent) CloseCurrentPopup();
            if (cachePopups.TryGetValue(type, out PopupBase popup))
            {
                popup.gameObject.transform.SetAsLastSibling();
                popup.Show();
                popupStack.Push(popup);
            }
            else
            {
                Debug.LogWarning($"Popup of type {type} not found in cache.");
            }
        }

        public void CloseCurrentPopup()
        {
            if (popupStack.Count > 0)
            {
                PopupBase topPopup = popupStack.Pop();
                topPopup.Hide();
            }
            else
            {
                Debug.LogWarning("No popups to close.");
            }
        }

    }

    public enum PopupType
    {
        Home = 0,
        Settings = 1,
        Shop = 2
    }
}