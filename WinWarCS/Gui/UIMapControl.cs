﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinWarCS.Data;
using WinWarCS.Data.Game;
using WinWarCS.Data.Resources;
using WinWarCS.Gui.Input;

namespace WinWarCS.Gui
{
    class UIMapControl : UIBaseComponent
    {
        private float mapOffsetX;
        private float mapOffsetY;

        internal int CameraTileX { get { return ((int)mapOffsetX / TileWidth); } }
        internal int CameraTileY { get { return ((int)mapOffsetY / TileHeight); } }

        internal int MapWidth
        {
            get { return (CurrentMap != null ? CurrentMap.MapWidth : 0); }
        }
        internal int MapHeight
        {
            get { return (CurrentMap != null ? CurrentMap.MapHeight : 0); }
        }
        internal int TileWidth
        {
            get { return (CurrentMap != null ? CurrentMap.TileWidth : 0); }
        }
        internal int TileHeight
        {
            get { return (CurrentMap != null ? CurrentMap.TileHeight : 0); }
        }
        internal int WidthInTiles
        {
            get { return (CurrentMap != null ? Width / TileWidth : 0); }
        }
        internal int HeightInTiles
        {
            get { return (CurrentMap != null ? Height / TileHeight : 0); }
        }

        internal Map CurrentMap { get; private set; }

        internal UIMapControlInputHandler InputHandler { get; private set; }

        internal UIMapControl()
        {
            CurrentMap = null;

            InputHandler = new UIMapControlInputHandlerEnhancedMouse(this);
            InputHandler.OnMapDidScroll += inputHandler_OnMapDidScroll;
        }

        void inputHandler_OnMapDidScroll(float setMapOffsetX, float setMapOffsetY)
        {
            SetCameraOffset(setMapOffsetX, setMapOffsetY);
        }

        internal void SetCameraOffset(float setCamOffsetX, float setCamOffsetY)
        {
            mapOffsetX = setCamOffsetX;
            mapOffsetY = setCamOffsetY;

            if (mapOffsetX < 0)
                mapOffsetX = 0;
            if (mapOffsetY < 0)
                mapOffsetY = 0;

            float maxX = (this.MapWidth * this.TileWidth) - this.Width;
            if (mapOffsetX > maxX)
                mapOffsetX = maxX;
            float maxY = (this.MapHeight * this.TileHeight) - this.Height;
            if (mapOffsetY > maxY)
                mapOffsetY = maxY;
        }

        internal void LoadCampaignLevel(string basename)
        {
            LevelInfoResource levelInfo = new LevelInfoResource(basename);
            LevelPassableResource levelPassable = new LevelPassableResource(basename + " (Passable)");
            LevelVisualResource levelVisual = new LevelVisualResource(basename + " (Visual)");

            CurrentMap = new Map(levelInfo, levelVisual, levelPassable);
        }

        internal void LoadCustomLevel(string basename)
        {
            LevelPassableResource levelPassable = new LevelPassableResource(basename + " (Passable)");
            LevelVisualResource levelVisual = new LevelVisualResource(basename + " (Visual)");

            CurrentMap = new Map(null, levelVisual, levelPassable);
        }

        internal override void Render()
        {
            base.Render();

            if (CurrentMap != null)
            {
                CurrentMap.Render(this.X, this.Y, this.Width, this.Height, mapOffsetX, mapOffsetY);
            }
        }

        internal override bool PointerDown(Microsoft.Xna.Framework.Vector2 position)
        {
            return InputHandler.PointerDown(position);
        }

        internal override bool PointerUp(Microsoft.Xna.Framework.Vector2 position)
        {
            return InputHandler.PointerUp(position);
        }

        internal override bool PointerMoved(Microsoft.Xna.Framework.Vector2 position)
        {
            return InputHandler.PointerMoved(position);
        }
    }
}