using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRTK
{

    public class AnimWindowPointerRenderer : VRTK_StraightPointerRenderer
    {

        [SerializeField] AnimationWindowController animationWindowController;
        VRTK_UIPointer uiPointer;
        bool inUIElements = false;

        uTouchInjection.Pointer pointer;

        static int currentId = 0;

        public enum TouchState
        {
            Release,
            Hover,
            Touch,
        }

        public TouchState state
        {
            get;
            set;
        }

        private void Start()
        {
            uiPointer = GetComponent<VRTK_UIPointer>();
        }

        protected override void OnEnable() 
        {
            base.OnEnable();

            var controllerEvents = GetComponent<VRTK_ControllerEvents>();
            controllerEvents.TriggerPressed += DoTriggrPressed;
            controllerEvents.TriggerReleased += DoTriggerReleased;
            controllerEvents.ButtonTwoPressed += DoMenuPressed;
            controllerEvents.ButtonTwoReleased += DoMenuReleased;
            //controllerEvents.ButtonTwoPressed += DoMenuPressed;
            //controllerEvents.ButtonTwoReleased += DoMenuReleased;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            var controllerEvents = GetComponent<VRTK_ControllerEvents>();
            controllerEvents.TriggerPressed -= DoTriggrPressed;
            controllerEvents.TriggerReleased -= DoTriggerReleased;
            controllerEvents.ButtonTwoPressed -= DoMenuPressed;
            controllerEvents.ButtonTwoReleased -= DoMenuReleased;
        }

        protected override float CastRayForward()
        {
            Transform origin = GetOrigin();
            Ray pointerRaycast = new Ray(origin.position, origin.forward);
            RaycastHit pointerCollidedWith;
            bool rayHit = VRTK_CustomRaycast.Raycast(customRaycast, pointerRaycast, out pointerCollidedWith, defaultIgnoreLayer, maximumLength);

            CheckRayMiss(rayHit, pointerCollidedWith);
            CheckRayHit(rayHit, pointerCollidedWith);

            float actualLength = maximumLength;
            if (rayHit && pointerCollidedWith.distance < maximumLength)
            {
                actualLength = pointerCollidedWith.distance;
            }

            //当たってる対象がAnimationWindowならマウス操作
            if (rayHit && pointerCollidedWith.collider.tag == "AnimationWindow")
            {
                if (state == TouchState.Release)
                {
                    GetPointer();
                    pointer.Hover();
                    state = TouchState.Hover;
                }
                var desktopPos = animationWindowController.GetMousePosFromUV(pointerCollidedWith.textureCoord);
                if (desktopPos != null)
                {
                    pointer.position = desktopPos.Value;
                }
                GetComponent<VRTK.VRTK_Pointer>().Toggle(true);
            }
            //UIに当たってればＵＩ操作
            else if (uiPointer.pointerEventData.pointerEnter)
            {
                GetComponent<VRTK.VRTK_Pointer>().Toggle(true);
            }
            //AnimationWindowにもUIにも当たっていない
            else
            {
                ReleasePointer();
                state = TouchState.Release;
                GetComponent<VRTK.VRTK_Pointer>().Toggle(false);
            }

            return OverrideBeamLength(actualLength);
        }

        private void DoTriggrPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (state == TouchState.Release) { return; }
            //animationWindowController.OnTouch();
            GetPointer();
            pointer.Touch();
            state = TouchState.Touch;
        }

        private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
        {
            if (state == TouchState.Release) { return; }
            //animationWindowController.OnRelease();
            GetPointer();
            pointer.Hover();
            state = TouchState.Hover;
        }

        private void DoMenuPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (state == TouchState.Release) { return; }

            DoTriggrPressed(sender, e);
            animationWindowController.DownAlt();
        }

        private void DoMenuReleased(object sender, ControllerInteractionEventArgs e)
        {
            if (state == TouchState.Release) { return; }

            DoTriggerReleased(sender, e);
            animationWindowController.UpAlt();
        }

        /*
        private void DoMenuPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (!isHover) { return; }
            animationWindowController.OnRightTouch();
        }

        private void DoMenuReleased(object sender, ControllerInteractionEventArgs e)
        {
            if (!isHover) { return; }
            animationWindowController.OnRightRelease();
        }*/

        /// <summary>
        /// The UpdateRenderer method is used to run an Update routine on the pointer.
        /// </summary>
        public override void UpdateRenderer()
        {
            //タッチステートを処理
            switch (state)
            {
                case TouchState.Release:
                    break;
                case TouchState.Hover:
                    GetPointer();
                    pointer.Hover();
                    break;
                case TouchState.Touch:
                    GetPointer();
                    pointer.Touch();
                    break;
            }

            float tracerLength = CastRayForward();
            SetPointerAppearance(tracerLength);
            if ((controllingPointer != null && controllingPointer.IsPointerActive()) || IsVisible())
            {
                MakeRenderersVisible();
            }

            if (playareaCursor != null)
            {
                playareaCursor.SetHeadsetPositionCompensation(headsetPositionCompensation);
                playareaCursor.ToggleState(IsCursorVisible());
            }

            if (directionIndicator != null)
            {
                UpdateDirectionIndicator();
            }
        }

        void GetPointer()
        {
            if (pointer != null) return;

            pointer = uTouchInjection.Manager.GetPointer(currentId);
            currentId++;
        }

        void ReleasePointer()
        {
            if (pointer == null) return;

            pointer.Release();
            pointer = null;
            currentId--;
        }
    }

}