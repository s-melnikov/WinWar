﻿using System;

namespace WinWarCS.Data.Game
{
   internal class HumanBalista : Unit
   {
      public HumanBalista (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Catapult")));
      }
   }
}
