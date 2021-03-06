﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VRGIN.Core;
using VRGIN.Modes;

namespace VRGIN.Visuals
{
    public class GUIQuad : ProtectedBehaviour
    {
#if !UNITY_4_5
        private Renderer renderer;
#endif

        public static GUIQuad Create()
        {
            VRLog.Info("Create GUI");
            var gui = GameObject.CreatePrimitive(PrimitiveType.Quad).AddComponent<GUIQuad>();
            gui.name = "GUIQuad";

            gui.UpdateGUI(true, true);

            return gui;
        }


        protected override void OnAwake()
        {
#if !UNITY_4_5
            renderer = GetComponent<Renderer>();
#endif

            transform.localPosition = Vector3.zero;// new Vector3(0, 0, distance);
            transform.localRotation = Quaternion.identity;
            gameObject.layer = LayerMask.NameToLayer(VRManager.Instance.Context.GuiLayer);
        }

        protected override void OnStart()
        {
            base.OnStart();
            UpdateAspect();
        }

        protected virtual void OnEnable()
        {
            VRLog.Info("Listen!");

            VRGUI.Instance.Listen();
        }

        protected virtual void OnDisable()
        {
            VRLog.Info("Unlisten!");

            VRGUI.Instance.Unlisten();
        }
        
        public virtual void UpdateAspect()
        {
            var height = transform.localScale.y;
            var width = height / Screen.height * Screen.width;

            transform.localScale = new Vector3(width, height, 1);
        }

        public virtual void UpdateGUI(bool transparent, bool renderGUI)
        {
            //VRLog.Info();
            //renderGUI = false;
            UpdateAspect();
            if (!renderer) VRLog.Warn("No renderer!");
            try
            {
                renderer.receiveShadows = false;
#if UNITY_4_5
                renderer.castShadows = false;
#else
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
#endif

                renderer.castShadows = false;
                if (transparent)
                {
                    if (renderGUI)
                    {
                        renderer.material = VRManager.Instance.Context.Materials.UnlitTransparentCombined;
                        renderer.material.SetTexture("_MainTex", VRGUI.Instance.uGuiTexture);
                        renderer.material.SetTexture("_SubTex", VRGUI.Instance.nGuiTexture);
                    }
                    else
                    {
                        renderer.material = VRManager.Instance.Context.Materials.UnlitTransparent;
                        renderer.material.mainTexture = VRGUI.Instance.uGuiTexture;
                    }
                }
                else
                {
                    renderer.material = VRManager.Instance.Context.Materials.Unlit;
                    renderer.material.mainTexture = VRGUI.Instance.uGuiTexture;
                }
            }
            catch (Exception e)
            {
                VRLog.Info(e);
            }
        }
    }
}
