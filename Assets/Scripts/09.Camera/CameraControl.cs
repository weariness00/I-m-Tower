using System;
using System.Threading;
using DG.Tweening;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraBase
{
    public class CameraControl : MonoBehaviour
    {
        public float speed;
        public float rotateSpeed;
        public float distanceOffset;

        private Sequence rotateSequence;

        public void Awake()
        {
            InputManager.Instance.DoubleClick.performed += Rotate;
        }

        public void OnDestroy()
        {
            if (InputManager.HasInstance)
            {
                InputManager.Instance.DoubleClick.performed -= Rotate;
            }
        }

        public void LateUpdate()
        {
            Move();
        }

        public void Move()
        {
            if(InputManager.Instance.Click.ReadValue<float>() <= 0f) return;
            var rotate = transform.eulerAngles;
            var mouseDelta = InputManager.Instance.MouseDelta * speed;
            var pos = transform.position;
            transform.eulerAngles = new Vector3(0, rotate.y, 0);
            pos += transform.right * -mouseDelta.x + transform.forward * -mouseDelta.y;
            transform.position = pos;
            transform.eulerAngles = rotate;
        }

        public void Rotate(InputAction.CallbackContext context)
        {
            if (context.performed)
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
    }
}

