﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Graphics;
using WinWarCS.Gui;
using WinWarCS.Data.Game;
using WinWarCS.Data;

namespace WinWarCS.GameScreens.Windows
{
   class IngameMenuWindow : UIWindow
   {
      UIImage background;
      UIButton continueButton;
      UIButton quitButton;

      internal IngameMenuWindow (Race setRace)
      {
         LevelGameScreen.Game.GamePaused = true;

         BackgroundColor = new Microsoft.Xna.Framework.Color (0.3f, 0.0f, 0.0f, 0.5f);

         //Width = background.Width;
         //Height = background.Height;

         background = new UIImage (WWTexture.FromImageResource ("Large Box (" + setRace + ")"));
         background.InitWithUIResource (WarFile.GetUIResource (369));
         AddComponent (background);

         background.X = 120;
         background.Y = 20;

         ((UIButton)background.Components [1]).Type = UIButton.ButtonType.MediumButton;
         ((UIButton)background.Components [2]).Type = UIButton.ButtonType.MediumButton;
         ((UIButton)background.Components [3]).Type = UIButton.ButtonType.MediumButton;
         ((UIButton)background.Components [4]).Type = UIButton.ButtonType.MediumButton;
         ((UIButton)background.Components [5]).Type = UIButton.ButtonType.SmallButton;
         ((UIButton)background.Components [6]).Type = UIButton.ButtonType.SmallButton;

         continueButton = (UIButton)background.Components [6];
         continueButton.OnMouseUpInside += closeButton_OnMouseUpInside;

         quitButton = (UIButton)background.Components [5];
         quitButton.OnMouseUpInside += quitButton_OnMouseUpInside;

         MouseCursor.State = MouseCursorState.Pointer;
      }

      internal override void DidRemove ()
      {
         base.DidRemove ();

         LevelGameScreen.Game.GamePaused = false;
      }

      void quitButton_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         WinWarCS.Platform.Sys.Exit ();
      }

      void closeButton_OnMouseUpInside (Microsoft.Xna.Framework.Vector2 position)
      {
         Close ();
      }

      internal override bool PointerMoved (Microsoft.Xna.Framework.Vector2 position)
      {
         MouseCursor.State = MouseCursorState.Pointer;

         return base.PointerMoved (position);
      }
   }
}
