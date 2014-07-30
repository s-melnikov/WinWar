﻿using System;

namespace WinWarCS.Data.Game
{
   internal class OrcNecro : Unit
   {
      public OrcNecro (Map currentMap)
         : base(currentMap)
      {
         sprite = new UnitSprite (WarFile.GetSpriteResource (KnowledgeBase.IndexByName ("Orc Necro")));
      }
   }
}
