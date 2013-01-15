#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using WinWarRT.Data;
using WinWarRT.Data.Resources;
#endregion

namespace WinWarRT.Gui
{
	public class Window : BaseComponent
	{
		#region Constructor
		protected Window()
		{
			X = Y = 0;
			Width = MainGame.AppWidth;
            Height = MainGame.AppHeight;
		}
		#endregion

        #region InitWithTextResource
        protected void InitWithTextResource(string name)
        {
            int idx = KnowledgeBase.IndexByName(name);
            if (idx == -1)
                return;

            TextResource tr = WarFile.GetTextResource(idx);
            InitWithTextResource(tr);
        }

        protected void InitWithTextResource(TextResource resource)
        {
            Components.Clear();

            for (int i = 0; i < resource.Texts.Count; i++)
            {
                if (resource.Texts[i].unknown1 == 0)
                {
                    Label lbl = new Label(resource.Texts[i].Text);
                    lbl.X = (int)(resource.Texts[i].X);
                    lbl.Y = (int)(70 + resource.Texts[i].Y);
                    Components.Add(lbl);
                }
                else
                {
                    Button.ButtonType type = Button.ButtonType.MediumButton;
                    if (resource.Texts[i].unknown4 == 66)
                        type = Button.ButtonType.SmallButton;

                    Button btn = new Button(resource.Texts[i].Text, type);
                    btn.X = (int)(resource.Texts[i].X);
                    btn.Y = (int)(70 + resource.Texts[i].Y);
                    Components.Add(btn);
                }
            }
        }
        #endregion

        #region FromTextResource
        public static Window FromTextResource(string name)
        {
            int idx = KnowledgeBase.IndexByName(name);
			if (idx == -1)
				return null;

			TextResource tr = WarFile.GetTextResource(idx);
			return FromTextResource(tr);
        }

		public static Window FromTextResource(TextResource resource)
		{
			Window wnd = new Window();

			for (int i = 0; i < resource.Texts.Count; i++)
			{
				Button btn = new Button(resource.Texts[i].Text, Button.ButtonType.MediumButton);
				btn.X = (int)(resource.Texts[i].X);
                btn.Y = (int)(70 + resource.Texts[i].Y);
				wnd.Components.Add(btn);
			}

			return wnd;
		}
		#endregion

		#region Render
		public override void Render()
		{
			base.Render();
			//BaseGame.StdFont.DrawText(null, "Window (X = " + X + "; Y = " + Y +
			//	"; Width = " + Width + "; Height = " + Height + ")", X, Y, System.Drawing.Color.White);
		}
		#endregion

		#region Unit testing
		public static void TestWindow()
		{
            throw new NotImplementedException();
			Window wnd = null;

			/*TestGame.Start("TestWindow",
				delegate
				{
					TextResource tr = new TextResource("Main Menu Text");
					wnd = Window.FromTextResource(tr);
				},
				delegate
				{
					wnd.Render();
				});*/
		}
		#endregion
	}
}
