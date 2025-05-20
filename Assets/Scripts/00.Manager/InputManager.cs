using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Manager
{
    public partial class InputManager : Singleton<InputManager>
    {
        public InputSystem_Actions input;

        public Vector2 MousePosition => input.UI.Point.ReadValue<Vector2>();
        public Vector2 MouseDelta => input.UI.Delta.ReadValue<Vector2>();
        public InputAction Click => input.UI.Click;
        public InputAction Delta => input.UI.Delta;
        public InputAction Press => input.UI.Press;
        public InputAction DoubleClick => input.UI.DoubleClick;

        protected override void Initialize()
        {
            base.Initialize();
            input = new();
            input.Enable();
        }
        

    }
}