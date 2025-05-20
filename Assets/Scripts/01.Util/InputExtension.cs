using System;
using UnityEngine.InputSystem;

namespace Util
{
    public static class InputExtension
    {
        public static void AddActionAllEvent(this InputAction action, Action<InputAction.CallbackContext> callback)
        {
            action.started += callback;
            action.performed += callback;
            action.canceled += callback;
        }
        
        public static void RemoveActionAllEvent(this InputAction action, Action<InputAction.CallbackContext> callback)
        {
            action.started -= callback;
            action.performed -= callback;
            action.canceled -= callback;
        }
    }
}