﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinWarRT.Gui.Input
{
    class UIMapControlInputHandlerClassic : UIMapControlInputHandler
    {
        public UIMapControlInputHandlerClassic(UIMapControl setUIMapControl)
            : base(InputMode.Classic, setUIMapControl)
        {
        }

        public override bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }

        public override bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }

        public override bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            return true;
        }
    }
}