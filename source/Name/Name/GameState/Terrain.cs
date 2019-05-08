using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Name.GameState
{

    public struct TerrainSaver
    {
        public int Square_Height;
        public int Square_Width; //width +-3
        public int Triangle_Height;
        public int Triangle_Width; //width +-2
        public int Noise1_Height;
        public int Noise1_Width; //width  +- 10
        public int Noise2_Height;
        public int Noise2_Width; //width +- 5
        public int Offset_From_Zero;

        public void Write( System.IO.StreamWriter sw )
        {
            sw.WriteLine( "  " + Square_Height + "," + Square_Width + "," + Triangle_Height + "," + Triangle_Width + "," + Noise1_Height + "," + Noise1_Width + "," + Noise2_Height + "," + Noise2_Width + "," + Offset_From_Zero );
        }

        public bool Read( System.IO.StreamReader sr )
        {
            string data = "";
            try
            {
                data = sr.ReadLine();
            }
            catch ( Exception )
            {
                return false;
            }
            if ( !data.Contains( "," ) )
                return false;

            Square_Height = Convert.ToInt32( data.Substring( 0, data.IndexOf( "," ) ) );
            data = data.Substring( data.IndexOf( "," ) + 1 );

            Square_Width = Convert.ToInt32( data.Substring( 0, data.IndexOf( "," ) ) );
            data = data.Substring( data.IndexOf( "," ) + 1 );

            Triangle_Height = Convert.ToInt32( data.Substring( 0, data.IndexOf( "," ) ) );
            data = data.Substring( data.IndexOf( "," ) + 1 );

            Triangle_Width = Convert.ToInt32( data.Substring( 0, data.IndexOf( "," ) ) );
            data = data.Substring( data.IndexOf( "," ) + 1 );

            Noise1_Height = Convert.ToInt32( data.Substring( 0, data.IndexOf( "," ) ) );
            data = data.Substring( data.IndexOf( "," ) + 1 );

            Noise1_Width = Convert.ToInt32( data.Substring( 0, data.IndexOf( "," ) ) );
            data = data.Substring( data.IndexOf( "," ) + 1 );

            Noise2_Height = Convert.ToInt32( data.Substring( 0, data.IndexOf( "," ) ) );
            data = data.Substring( data.IndexOf( "," ) + 1 );

            Noise2_Width = Convert.ToInt32( data.Substring( 0, data.IndexOf( "," ) ) );
            data = data.Substring( data.IndexOf( "," ) + 1 );

            Offset_From_Zero = Convert.ToInt32( data );
            return true;
        }
        public int[] Values
        {
            get
            {
                int[] values = new int[9];
                values[0] = Square_Height;
                values[1] = Square_Width;
                values[2] = Triangle_Height;
                values[3] = Triangle_Width;
                values[4] = Noise1_Height;
                values[5] = Noise1_Width;
                values[6] = Noise2_Height;
                values[7] = Noise2_Width;
                values[8] = Offset_From_Zero;
                return values;
            }
            set
            {
                if ( value.Length < 9 )
                {
                    Engine.DebugHelper.Break( Engine.DebugHelper.DebugLevels.Critical );
                    throw new ArgumentException( "Integer Array must be 9 args long" );
                }
                else
                {
                    Square_Height = value[0];
                    Square_Width = value[1];
                    Triangle_Height = value[2];
                    Triangle_Width = value[3];
                    Noise1_Height = value[4];
                    Noise1_Width = value[5];
                    Noise2_Height = value[6];
                    Noise2_Width = value[7];
                    Offset_From_Zero = value[8];
                }
            }
        }
    }

    public class Terrain
    {
        public int ExtraHeight
        {
            get
            {
                return 16;
            }
        }
        public static Color BackGround = Color.Purple; //new Color( 241, 185, 71, 256 );// no alpha

        int[] Heights;
        public Texture2D BackgroundLeft;
        public Texture2D BackgroundRight;
        public Rectangle m_BoundingRectangle;
        public static int EdgeOffset = 240;

        public Terrain( string p_BackgroundLeft, string p_BackgroundRight, TerrainSaver ts )
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

            BackgroundLeft = Engine.ContentManager.Instance.GetObject<Texture2D>(p_BackgroundLeft);
            BackgroundRight = Engine.ContentManager.Instance.GetObject<Texture2D>(p_BackgroundRight);
            int TwoTimesEdgeOffset = 2 * EdgeOffset;
            Heights = new int[( BackgroundLeft.Width + BackgroundRight.Width ) - TwoTimesEdgeOffset];

            m_BoundingRectangle = new Rectangle( -1 * EdgeOffset, 0, BackgroundLeft.Width + BackgroundRight.Width - TwoTimesEdgeOffset, BackgroundLeft.Height );

            float pi = 3.14159265f;

            /*
            StorageDevice storageDevice = Game1.Instance.StorageDevice;
            StorageContainer storageContainer = storageDevice.OpenContainer("Name");
            string filename = Path.Combine(storageContainer.Path, "PreviouslyPlayed.txt");
            FileStream stream = File.Open(filename, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(stream);
            ts.Write(sw);
            sw.Flush();
            sw.Close();
            storageContainer.Dispose();
            */

            for ( int i = ( 0 + offset + EdgeOffset ); i < ( BackgroundLeft.Width + BackgroundRight.Width + offset - EdgeOffset ); ++i )
            {

                // Change heights and widths to adjust shape of dynamically generated terrain. Widths cannot be zero, but heights can be.

                int square_height = 75 + s_hr, square_width = 10 + s_wr;
                double square = ( ( square_height ) * ( Math.Sin( ( double ) 0.01 / square_width * 2 * pi * i ) ) ) + ( ( square_height / 3 ) * ( Math.Sin( ( double ) 0.01 / square_width * 6 * pi * i ) ) ) + ( ( square_height / 5 ) * ( Math.Sin( ( double ) 0.01 / square_width * 10 * pi * i ) ) ) + ( ( square_height / 7 ) * ( Math.Sin( ( double ) 0.01 / square_width * 14 * pi * i ) ) ) + ( ( square_height / 9 ) * ( Math.Sin( ( double ) 0.01 / square_width * 18 * pi * i ) ) );

                int triangle_height = 100 + t_hr, triangle_width = 8 + t_wr;
                double triangle = ( ( ( triangle_height ) * ( Math.Sin( ( double ) ( 0.01 / triangle_width ) * 2 * pi * i ) ) ) - ( ( triangle_height / 9 ) * ( Math.Sin( ( double ) ( 0.01 / triangle_width ) * 6 * pi * i ) ) ) + ( ( triangle_height / 25 ) * ( Math.Sin( ( double ) ( 0.01 / triangle_width ) * 10 * pi * i ) ) ) );

                int sine_height = -50 + n1_hr, sine_width = 40 + n1_wr;
                double sine = ( ( sine_height ) * ( Math.Sin( ( double ) ( 0.01 / sine_width ) * 2 * pi * i ) ) );

                int noise_height = -25 + n2_hr, noise_width = 20 + n2_wr;
                double noise = ( ( noise_height ) * ( Math.Cos( ( double ) ( 0.1 / noise_width ) * 2 * pi * i ) ) );

                double piece6 = 1000;
                double temp = square + triangle + sine + noise + piece6;


                Heights[i - offset - EdgeOffset] = ( int ) temp;
            }
        }

        public int this[int x]
        {
            get
            {
                int TexWidth = ( BackgroundLeft.Width + BackgroundRight.Width ) - ( 2 * Terrain.EdgeOffset );
                if ( x >= TexWidth )
                {
                    x = TexWidth - 1;
                }
                else if ( x <= 0 )
                {
                    x = 1;
                }
                return Heights[x];
            }
        }

        public void DeformTerrain( Vector2 Position, int radius )
        {

        }

    }

}
