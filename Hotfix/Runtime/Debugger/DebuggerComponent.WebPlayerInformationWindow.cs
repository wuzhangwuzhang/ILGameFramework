﻿//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace Hotfix.Runtime
{
    public partial class DebuggerComponent
    {
        private sealed class WebPlayerInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Web Player Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Is Web Player:", Application.isWebPlayer.ToString());
                    DrawItem("Absolute URL:", Application.absoluteURL);
                    DrawItem("Source Value:", Application.srcValue);
                    DrawItem("Streamed Bytes:", Application.streamedBytes.ToString());
                }
                GUILayout.EndVertical();
            }
        }
    }
}