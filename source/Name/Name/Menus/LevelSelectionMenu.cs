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
    class LevelSelectionMenu : UIBaseMenu
    {
        TerrainSaver ts;
        Texture2D MiniMap;
        Texture2D MiniMapSource = Engine.ContentManager.Instance.GetObject<Texture2D>(@"Content\MiniMap\MiniMap");
        int Human = 0;
        int Computer = 0;
        GameOptions options;
        public LevelSelectionMenu(int human, int computer )
            : base( @"SharedContent\Background", Point.Zero )
        {
            options = new GameOptions(true);

            Human = human;
            Computer = computer;
            UIButton b = new UIButton( @"Content\Menu\Back", CameraManager.GetPosition( 0.025f, 0.8125f ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Back, b );
            /*
            b = new UIButton( @"Content\Menu\Previous", new Point( 19, 190 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Next, b );
            b = new UIButton( @"Content\Menu\Next", new Point( 420, 190 ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Previous, b );
            */
            b = new UIButton( @"Content\Menu\GenerateLevel", CameraManager.GetPosition( 0.025f, 0.4f ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( RandomLevel, b );


            System.IO.StreamReader stream = FileMananger.Instance.ReadFile( FileMananger.PreviouslyPlayed );
            LinkedList<TerrainSaver>  LL = new LinkedList<TerrainSaver>();
            while ( !stream.EndOfStream )
            {
                TerrainSaver ts = new TerrainSaver();
                if ( ts.Read( stream ) )
                    LL.AddLast( ts );
            }
            stream.Close();

            if ( LL.Count > 0 )
            {
                b = new UITextButton( "Previously Played", CameraManager.GetPosition( 0.025f, 0.49f ) );
                b.ValidStates = ( int ) TouchStates.Released;
                Add( PreviouslyPlayed, b );
            }
            b = new UIButton( @"Content\Menu\Start", CameraManager.GetPosition( 0.025f, 0.58f ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Start, b );

            b = new UIButton( @"Content\Menu\Options", CameraManager.GetPosition( 0.025f, 0.67f ) );
            b.ValidStates = ( int ) TouchStates.Released;
            Add( Options, b );

            Texture2D BackgroundLeft = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\BackgroundL" );
            Texture2D BackgroundRight = Engine.ContentManager.Instance.GetObject<Texture2D>( @"Content\MiniMap\BackgroundR" );
            int width = (BackgroundLeft.Width + BackgroundRight.Width - (2 * Terrain.EdgeOffset) ) / 10;
            int height = (BackgroundRight.Height) / 10;

            MiniMap = new Texture2D( MenuSystemInstance.GraphicsDevice.GraphicsDevice, width, height, false, SurfaceFormat.Color );
            GenerateRandomTerrain();

            Engine.ContentManager.Instance.SaveContent<Texture2D>( MiniMap, "MiniMap" );

            Point position =  CameraManager.GetPosition( 0.5f, 0.1875f );
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

        protected void GenerateRandomTerrain()
        {
            System.Random rand = new Random();

            int s_hr = rand.Next(0, 11) * 5 - 25;
            int s_wr = rand.Next(0, 7) - 3; //width +-3
            int t_hr = rand.Next(0, 11) * 5 - 25;
            int t_wr = rand.Next(0, 5) - 2; //width +-2
            int n1_hr = rand.Next(0, 11) - 5;
            int n1_wr = rand.Next(0, 21) - 10; //width  +- 10
            int n2_hr = rand.Next(0, 11) - 5;
            int n2_wr = rand.Next(0, 11) - 5; //width +- 5

            int offset = (rand.Next(0, 21) + 40) * 10;

            ts.Square_Height = s_hr;
            ts.Square_Width = s_wr;
            ts.Triangle_Height = t_hr;
            ts.Triangle_Width = t_wr;
            ts.Noise1_Height = n1_hr;
            ts.Noise1_Width = n1_wr;
            ts.Noise2_Height = n2_hr;
            ts.Noise2_Width = n2_wr;
            ts.Offset_From_Zero = offset;

            GenerateTerrain();
        }

        void RandomLevel( )
        {
            GenerateRandomTerrain();
        }

        void PreviouslyPlayed( )
        {
            MenuSystemInstance.AddMenu( new PreviouslyPlayedLevelSelectionMenu(options, Human, Computer ) );
        }

        void Next( )
        {

        }
        
        void Previous( )
        {

        }

        void Options( )
        {
            MenuSystemInstance.AddMenu( new GameOptionsMenu(options) );
        }
        
        void Start( )
        {
            if ( Human + Computer == 0 )
                return;

            System.IO.StreamWriter stream = FileMananger.Instance.OpenFileForAppend( FileMananger.PreviouslyPlayed );
            ts.Write( stream );
            stream.Flush();
            stream.Close();

            MenuSystem.Instance.PopTopMenu(); // Level Selection Menu
            MenuSystem.Instance.PopTopMenu(); // AI Selection Menu
            MenuSystem.Instance.PopTopMenu(); // Human Selection Menu
            Engine.ContentManager.Instance.SaveContent<Texture2D>( MiniMap, "MiniMap" );
            PlayingState ps = new Name.GameState.PlayingState(options, Human, Computer, ts );
            Engine.GameState.GameStateSystem.Instance.AddGameState( ps );
            // Must add a GameState before we can setup the players
            ps.SetupPlayers();
            // Players setup now lets start the game
            Engine.MenuSystem.Instance.PushMenu( new Name.Menus.LocalGameMenu(ps) );
        }

    }
}
