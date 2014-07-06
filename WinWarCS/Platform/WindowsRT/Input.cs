using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using WinWarCS.Gui;

namespace WinWarCS.Platform
{
   public static class Input
   {
      private static Vector2 prevMousePos;
      private static MouseState prevMouseState;

      static Input()
      {
         prevMousePos = new Vector2 (-1, -1);
      }

      public static void UpdateInput(GameTime gameTime)
      {
         MouseState mouseState = Mouse.GetState ();

         Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);

         Vector2 scaledPosition = new Microsoft.Xna.Framework.Vector2 ((mousePos.X - MainGame.ScaledOffsetX) / MainGame.ScaleX, 
            (mousePos.Y - MainGame.ScaledOffsetY) / MainGame.ScaleY);

         MouseCursor.Position = scaledPosition;

         if (mousePos != prevMousePos)
         {
            MainGame.WinWarGame.PointerMoved(scaledPosition);
            prevMousePos = mousePos;
         }

         if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
         {
            MainGame.WinWarGame.PointerPressed (scaledPosition);
         }

         if (mouseState.LeftButton == ButtonState.Released && prevMouseState.LeftButton == ButtonState.Pressed)
         {
            MainGame.WinWarGame.PointerReleased (scaledPosition);
         }

         prevMouseState = mouseState;
      }
   }
}
