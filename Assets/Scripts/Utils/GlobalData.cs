using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utils
{
    public enum Layer
    {
        Default = 0, TransparentFx = 1, IgnoreRaycast = 2, Water = 4, UserInterface = 5, Map = 8, WallOrObstacle = 9, Vision = 10, UnitInteraction = 11, FogOfWar = 12, SensoryVision = 13
    }

    public static class GlobalData
    {
        public static SceneManager SceneManager { get; set; }
        public static CameraControl CameraControl { get; set; }
        public static Unit Player { get; set; }
        public static Unit Enemy { get; set; }

        public static int WallLayerMask = 1 << UnityEngine.LayerMask.NameToLayer(Layer.WallOrObstacle.ToString());
        public static int UnitInteractionLayerMask = 1 << UnityEngine.LayerMask.NameToLayer(Layer.UnitInteraction.ToString());
        public static int SensoryVisionLayerMask = 1 << UnityEngine.LayerMask.NameToLayer(Layer.SensoryVision.ToString());
    }
}