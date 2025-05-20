using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Util;
using InputManager = Manager.InputManager;

namespace CameraBase
{
    public class CameraControl : MonoBehaviour
    {
        public float speed;
        public float rotateSpeed;
        public float distanceOffset;

        private bool isPress;
        private Sequence rotateSequence;

        public void Awake()
        {
            InputManager.Instance.Press.AddActionAllEvent(Press);
            InputManager.Instance.Delta.performed += Move;
            InputManager.Instance.DoubleClick.performed += Rotate;
        }

        public void OnDestroy()
        {
            if (InputManager.HasInstance)
            {
                InputManager.Instance.Press.RemoveActionAllEvent(Press);
                InputManager.Instance.Delta.performed -= Move;
                InputManager.Instance.DoubleClick.performed -= Rotate;
            }
        }

        private void Press(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isPress = true;
            }
            else if (context.canceled)
            {
                isPress = false;
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (context.performed && isPress && !IsPointerOverUI())
            {
                var rotate = transform.eulerAngles;
                var mouseDelta = InputManager.Instance.MouseDelta * speed;
                var pos = transform.position;
                transform.eulerAngles = new Vector3(0, rotate.y, 0);
                pos += transform.right * -mouseDelta.x + transform.forward * -mouseDelta.y;
                transform.position = pos;
                transform.eulerAngles = rotate;
            }
        }

        public void Rotate(InputAction.CallbackContext context)
        {
            if (context.started && !IsPointerOverUI())
            {
                rotateSequence?.Kill();
                rotateSequence = DOTween.Sequence();

                Vector3 mouseViewportPos = Camera.main.ScreenToViewportPoint(InputManager.Instance.MousePosition);
                var rotate = transform.eulerAngles;
                rotate.y += mouseViewportPos.x > 0.5f ? -90f : 90f;
                rotateSequence.Append(transform.DORotate(rotate, 0.3f));
                rotateSequence.OnKill(() => transform.eulerAngles = rotate);
                rotateSequence.OnComplete(() => transform.eulerAngles = rotate);
            }
        }
        
        private bool IsPointerOverUI()
        {
            // 마우스용 (에디터/PC)
            if (Mouse.current != null)
            {
                return EventSystem.current.IsPointerOverGameObject();
            }

            // 터치용 (모바일)
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                TouchControl touch = Touchscreen.current.primaryTouch;
                return EventSystem.current.IsPointerOverGameObject(touch.touchId.ReadValue());
            }

            return false;
        }
    }
}

