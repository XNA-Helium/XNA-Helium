using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using Engine;
using Engine.UI;
using Name.GameState;

namespace Name.Menus
{
    class PreviouslyPlayedLevelSelectionMenu : UIBaseMenu
    {
        TerrainSaver ts;
        Texture2D MiniMap;
        Texture2D MiniMapSource = Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\MiniMap\MiniMap");
        int Human = 0;
        int Computer = 0;

        bool ShowingNode = false;

        LinkedList<TerrainSaver> LL;
        LinkedListNode<TerrainSaver> CurrentNode;

        GameOptions Options;
        public PreviouslyPlayedLevelSelectionMenu(GameOptions options, int human, int computer )
            : base( @"SharedContent\Background", Point.Zero )
        {
            Options = options;
            Human = human;
            Computer = computer;
            UIButton b = new UIButton(@"Content\Menu\Back", CameraManager.GetPosition(0.025f, 0.625f));
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );

            Texture2D BackgroundLeft = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\BackgroundL" );
            Texture2D BackgroundRight = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\BackgroundR" );
            int width = (BackgroundLeft.Width + BackgroundRight.Width - (2 * Terrain.EdgeOffset) ) / 10;
            int height = (BackgroundRight.Height) / 10;

            MiniMap = new Texture2D( MenuSystemInstance.GraphicsDevice.GraphicsDevice, width, height, false, SurfaceFormat.Color );

            System.IO.StreamReader stream = FileMananger.Instance.ReadFile( FileMananger.PreviouslyPlayed );
            LL = new LinkedList<TerrainSaver>();
            while ( !stream.EndOfStream )
            {
                TerrainSaver tsg = new TerrainSaver();
                if ( tsg.Read( stream ) )
                    LL.AddLast( tsg );
            }
            stream.Close();
            CurrentNode = LL.First;
            ts = CurrentNode.Value;
            GenerateTerrain();
            ShowingNode = true;

            if ( ShowingNode )
            {
                b = new UIButton(@"Content\Menu\Previous", CameraManager.GetPosition(0.025f, 0.2375f));
                b.ValidStates = ( int ) TouchStates.Released;
                Add( Next, b );
                Point Position = CameraManager.GetPosition(0.975f, 0.2375f);
                Position.X -= Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\Menu\Next_active").Width;
                b = new UIButton(@"Content\Menu\Next", Position);
                b.ValidStates = ( int ) TouchStates.Released;
                Add( Previous, b );
            }
            b = new UIButton( @"Content\Menu\Start", CameraManager.GetPosition(0.025f,0.5f) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Start, b );


            Engine.ContentManager.Instance.SaveContent<Texture2D>( MiniMap, "MiniMap" );

            Point position = CameraManager.GetPosition(0.5f, 0.1875f);
            position.X -= (MiniMap.Width / 2);
            UIImageElement e = new UIImageElement(MiniMap, position);
            AddUIElement( e );
        }

            
        protected void GenerateTerrain()
        {
            int s_hr = ts.Square_Height;
            int s_wr = ts.Square_Width;
            int t_hr = ts.Triangle_Height;
            int t_wr = ts.Triangle_Width;
            int n1_hr = ts.Noise1_Height;
            int n1_wr = ts.Noise1_Width;
            int n2_hr = ts.Noise2_Height;
            int n2_wr = ts.Noise2_Width;
            int offset = ts.Offset_From_Zero;
            float pi = 3.14159265f;

            int EdgeOffset = Terrain.EdgeOffset;

            Color[] MiniColorData = new Color[MiniMap.Height * MiniMap.Width];
            MiniMapSource.GetData<Color>(MiniColorData);
            for (int i = (0 + offset + EdgeOffset); i < ((MiniMap.Width * 10) + offset + EdgeOffset); i += 10)
            {
                int square_height = 75 + s_hr, square_width = 10 + s_wr;
                double square = ((square_height) * (Math.Sin((double)0.01 / square_width * 2 * pi * i))) + ((square_height / 3) * (Math.Sin((double)0.01 / square_width * 6 * pi * i))) + ((square_height / 5) * (Math.Sin((double)0.01 / square_width * 10 * pi * i))) + ((square_height / 7) * (Math.Sin((double)0.01 / square_width * 14 * pi * i))) + ((square_height / 9) * (Math.Sin((double)0.01 / square_width * 18 * pi * i)));

                int triangle_height = 100 + t_hr, triangle_width = 8 + t_wr;
                double triangle = (((triangle_height) * (Math.Sin((double)(0.01 / triangle_width) * 2 * pi * i))) - ((triangle_height / 9) * (Math.Sin((double)(0.01 / triangle_width) * 6 * pi * i))) + ((triangle_height / 25) * (Math.Sin((double)(0.01 / triangle_width) * 10 * pi * i))));

                int sine_height = -50 + n1_hr, sine_width = 40 + n1_wr;
                double sine = ((sine_height) * (Math.Sin((double)(0.01 / sine_width) * 2 * pi * i)));

                int noise_height = -25 + n2_hr, noise_width = 20 + n2_wr;
                double noise = ((noise_height) * (Math.Cos((double)(0.1 / noise_width) * 2 * pi * i)));

                double piece6 = 1000;
                double temp = square + triangle + sine + noise + piece6;
                for (int j = 0; j < (MiniMap.Height * 10); j += 10)
                {
                    int index = ((i - offset - EdgeOffset) / 10) + (j * MiniMap.Width / 10);
                    if (j > temp)
                    {
                        MiniColorData[index] = Color.Black; //255 means no alpha
                    }
                }
            }
            MiniMap.SetData<Color>(MiniColorData);
            System.GC.Collect();
        }

        protected void Next( )
        {
            if ( ShowingNode )
            {
                if ( CurrentNode == LL.Last )
                {
                    CurrentNode = LL.First;
                }
                else
                {
                    CurrentNode = CurrentNode.Next;
                }
            }
            else
            {
                ShowingNode = true;
            }
            ts = CurrentNode.Value;
            GenerateTerrain();
        }
        protected void Previous( )
        {
            if ( ShowingNode )
            {
                if ( CurrentNode == LL.First )
                {
                    CurrentNode = LL.Last;
                }
                else
                {
                    CurrentNode = CurrentNode.Previous;
                }
            }
            else
            {
                ShowingNode = true;
            }
            ts = CurrentNode.Value;
            GenerateTerrain();

        }
               
        void Start( )
        {
            MenuSystem.Instance.PopTopMenu(); // PreviouslyPLayedLevelSelection Menu
            MenuSystem.Instance.PopTopMenu(); // Level Selection Menu
            MenuSystem.Instance.PopTopMenu(); // AI Selection Menu
            MenuSystem.Instance.PopTopMenu(); // Human Selection Menu
            Engine.ContentManager.Instance.SaveContent<Texture2D>( MiniMap, "MiniMap" );
            PlayingState ps = new Name.GameState.PlayingState(Options, Human, Computer, ts );
            Engine.GameState.GameStateSystem.Instance.AddGameState( ps );
            // Must add a GameState before we can setup the players
            ps.SetupPlayers();
            // Players setup now lets start the game
            Engine.MenuSystem.Instance.PushMenu( new Name.Menus.LocalGameMenu(ps) );
        }

    }
}
