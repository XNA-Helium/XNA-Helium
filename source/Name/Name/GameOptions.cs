using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Name
{
    public enum DisplayMode : int {UserChoice = 0, Normal = 1, Landscape = 2 };
    public enum ShootMode : int {UserChoice = 0, Normal = 1, Classic = 2};
    
    public class GameOptions
    {

        public DisplayMode DisplayMode = DisplayMode.Normal;
        public ShootMode ShootMode = ShootMode.Normal;

        // Turn Length  - 15, 30, 45, 60
        public int TurnLength = 30;

        // Number Players - 1,2,3,4, 5, 6
        public int NumberOfPlayers = 4;
        
        // Default HP - 50, 100, 150, 200
        public int NumHitPoints = 100;

        // Weapon Loadouts  - Standard, Customize 

        public static string TL = "TurnLength";
        public static string NP = "NumPlayers";
        public static string DM = "DisplayMode";
        public static string SM = "ShootMode";
        public static string NH = "NumHitPoints";

        public GameOptions( )
        {

        }

        public GameOptions( bool load )
        {
            if ( load )
            {
                System.IO.StreamReader sr = Engine.FileMananger.Instance.ReadFile( Engine.FileMananger.OptionsFile );
                Load( new System.IO.BinaryReader( sr.BaseStream ) );
                sr.Close();
            }
        }

        public string DisplayModeString
        {
            get
            {
                if ( DisplayMode == Name.DisplayMode.Normal )
                    return "Upright";
                if ( DisplayMode == Name.DisplayMode.Landscape )
                    return "LandScape";
                else
                    return "User Choice";
            }
        }
        public string ShootModeString
        {
            get
            {
                if ( ShootMode == Name.ShootMode.Normal )
                    return "Standard";
                if ( ShootMode == Name.ShootMode.Classic )
                    return "Classic";
                else
                    return "User Choice";
            }
        }
        public void Save( System.IO.BinaryWriter bw )
        {
            bw.Write( DM );
            bw.Write( ( int ) DisplayMode );


            bw.Write( SM );
            bw.Write( ( int ) ShootMode );

            bw.Write( TL );
            bw.Write( TurnLength );

            bw.Write( NP );
            bw.Write( NumberOfPlayers );

            bw.Write( NH );
            bw.Write( NumHitPoints );

        }

        public void Load( System.IO.BinaryReader br )
        {
            while ( br.BaseStream.Position < br.BaseStream.Length )
            {
                String s = br.ReadString();
                if ( s == DM )
                {
                    DisplayMode = ( DisplayMode ) br.ReadInt32();
                    if ( DisplayMode == DisplayMode.UserChoice )
                    {
                        DisplayMode = Name.DisplayMode.Normal;
                    }
                }
                else if ( s == SM )
                {
                    ShootMode = ( ShootMode ) br.ReadInt32();
                    if ( ShootMode == ShootMode.UserChoice )
                    {
                        ShootMode = Name.ShootMode.Normal;
                    }
                }
                else if ( s == TL )
                {
                    TurnLength = br.ReadInt32();
                    if ( TurnLength <= 0 )
                    {
                        TurnLength = 30;
                    }
                }
                else if ( s == NP )
                {
                    NumberOfPlayers = br.ReadInt32();
                    if ( NumberOfPlayers < 1 || NumberOfPlayers > 8 )
                    {
                        NumberOfPlayers = 4;
                    }
                }
                else if ( s == NH )
                {
                    NumHitPoints = br.ReadInt32();
                    if ( NumHitPoints < 1 || NumHitPoints > 1000 )
                    {
                        NumHitPoints = 100;
                    }
                }
            }
        }
    }
}
