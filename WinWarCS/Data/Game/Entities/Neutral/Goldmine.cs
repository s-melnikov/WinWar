﻿using System;
using WinWarCS.Data.Resources;

namespace WinWarCS.Data.Game
{
   internal class Goldmine : Building
   {
      public Goldmine (Map currentMap)
         : base(currentMap)
      {
         sprite = new Sprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Goldmine")));
      }
   }
}

