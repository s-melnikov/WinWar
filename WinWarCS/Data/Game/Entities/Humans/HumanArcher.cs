﻿using System;

namespace WinWarCS.Data.Game
{
   internal class HumanArcher : Unit
   {
      public HumanArcher (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Human Bowman")));
      }
   }
}
