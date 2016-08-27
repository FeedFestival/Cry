using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utils
{
    public static class GlobalData
    {
        public static SceneManager SceneManager { get; set; }
        public static CameraControl CameraControl { get; set; }
        public static Unit Player { get; set; }
    }
}