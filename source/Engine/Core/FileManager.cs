using System;
using System.IO.IsolatedStorage;
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

using System.Xml.Serialization;

namespace Engine
{
    public class FileMananger
    {
        public static string UserFileDir = @".\UserFiles";
        public static string SaveFileDir = @".\SaveFiles";
        public static string LevelDir = @".\Levels";
        public static string TutorialDir = @".\Tutorials";
        public static string ExceptionDir = @".\Exceptions";
        public static string PreviouslyPlayed = SaveFileDir + @"\PreviouslyPlayed";
        public static string ExitQuickSave = SaveFileDir + @"\ExitQuickSave";
        public static string OptionsFile = UserFileDir + @"\OptionsFile";
        public static string ExceptionFile = ExceptionDir + @"\ExceptionFile";

        public static long ENSURESPACEEXISTS = 1024 * 128;


#if WINDOWS_PHONE
        IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
#else
        public class FakeIsolatedStorageFile
        {
            public long AvailableFreeSpace
            {
                get
                {
                    return ENSURESPACEEXISTS * ENSURESPACEEXISTS;
                }
            }
            public long Quota
            {
                get
                {
                    return ENSURESPACEEXISTS * ENSURESPACEEXISTS;
                }
            }
            public bool IncreaseQuotaTo( long value )
            {
                return true;
            }

            public bool DirectoryExists( string path )
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo( path );
                return di.Exists;
            }
            public bool FileExists( string path )
            {
                System.IO.FileInfo fi = new System.IO.FileInfo( path );
                return fi.Exists;
            }
            public void CreateDirectory( string dir )
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo( dir );
                di.Create();
            }
            public System.IO.FileStream CreateFile( string path )
            {
                System.IO.FileInfo fi = new System.IO.FileInfo( path );
                return fi.Create();
            }
            public System.IO.FileStream OpenFile(string path, System.IO.FileMode mode)
            {
                return new System.IO.FileStream(path,mode);
            }
        }

        FakeIsolatedStorageFile isf = new FakeIsolatedStorageFile();
#endif
        protected FileMananger( )
        {
        
            Engine.DebugHelper.Break( isf.AvailableFreeSpace <= 1024 * 1024, DebugHelper.DebugLevels.Critical );
            ValidateDirectory( SaveFileDir );
            ValidateDirectory( LevelDir );
            ValidateDirectory( TutorialDir );
            ValidateDirectory( UserFileDir );
            ValidateDirectory( ExceptionDir );

            ValidateFile( PreviouslyPlayed );
            ValidateFile( ExitQuickSave );
            ValidateFile( ExceptionFile );

            ValidForUse();
        }
#if WINDOWS_PHONE
        internal IsolatedStorageFile GetFile
        {
            get
            {
                return isf;
            }
        }
#endif
        protected static FileMananger instance;
        public static FileMananger Instance
        {
            get
            {
                if ( instance == null )
                    instance = new FileMananger();
                Engine.DebugHelper.Break( !instance.ValidForUse(), DebugHelper.DebugLevels.Critical );
                return instance;
            }
        }
        public bool ValidForUse( )
        {
            return ValidForUse( ENSURESPACEEXISTS );
        }
        public bool ValidForUse(long bytes )
        {
            if ( isf.AvailableFreeSpace <= bytes)
            {
                return Engine.DebugHelper.Break( !isf.IncreaseQuotaTo( isf.Quota + bytes + 1024 ), DebugHelper.DebugLevels.Informative );
            }
            return true;
        }

        public void ValidateFile( string FileName )
        {
            if ( !isf.FileExists( FileName ) )
            {
                isf.CreateFile( FileName );
            }
            Engine.DebugHelper.Break( !isf.FileExists( FileName ), DebugHelper.DebugLevels.Critical );
        }

        public void ValidateDirectory( string directoryName )
        {
            if ( !isf.DirectoryExists( directoryName ) )
            {
                isf.CreateDirectory( directoryName );
            }
            Engine.DebugHelper.Break( !isf.DirectoryExists( directoryName ), DebugHelper.DebugLevels.Critical );
        }
#if WINDOWS_PHONE
        private System.IO.IsolatedStorage.IsolatedStorageFileStream  OpenFile( string FileName, System.IO.FileMode mode)
        {
            //return TitleContainer.OpenStream( FileName ); // Not using TitleContainer

            Engine.DebugHelper.Break( !isf.FileExists( FileName ), DebugHelper.DebugLevels.Critical );

            return  isf.OpenFile(FileName, mode);
        }
#else
        private System.IO.FileStream OpenFile( string FileName, System.IO.FileMode mode)
        {
            //return TitleContainer.OpenStream( FileName ); // Not using TitleContainer

            Engine.DebugHelper.Break( !isf.FileExists( FileName ), DebugHelper.DebugLevels.Critical );

            return (System.IO.FileStream) isf.OpenFile(FileName, mode);
        }
#endif
        public System.IO.FileStream ReadFileStream(string FileName)
        {
            return (System.IO.FileStream)OpenFile(FileName, System.IO.FileMode.Open);
        }
        public System.IO.StreamReader ReadFile( string FileName )
        {
            return new System.IO.StreamReader( OpenFile( FileName, System.IO.FileMode.Open ) );
        }
        public System.IO.FileStream OpenStreamForOverWrite(string FileName)
        {
            return (System.IO.FileStream) OpenFile(FileName, System.IO.FileMode.Create);
        }
        public System.IO.StreamWriter OpenFileForOverWrite( string FileName )
        {
            return new System.IO.StreamWriter( OpenFile( FileName, System.IO.FileMode.Create ) );
        }
        public System.IO.StreamReader OpenFileForRead(string FileName)
        {
            return new System.IO.StreamReader(OpenFile(FileName, System.IO.FileMode.Open));
        }
        public System.IO.StreamWriter OpenFileForWrite( string FileName )
        {
            return new System.IO.StreamWriter( OpenFile( FileName, System.IO.FileMode.Open ) );
        }
        public System.IO.StreamWriter OpenFileForAppend( string FileName )
        {
#if WINDOWS_PHONE
            System.IO.IsolatedStorage.IsolatedStorageFileStream fs = isf.OpenFile( FileName, System.IO.FileMode.Open );
#else
            System.IO.FileStream fs = OpenFile( FileName, System.IO.FileMode.Open );
#endif
            fs.Position = fs.Length;
            return new System.IO.StreamWriter( fs );
        }
        public void WriteException(Exception ex)
        {
            System.IO.StreamWriter sw = OpenFileForOverWrite(ExceptionFile);
            sw.Write("\n");
            sw.WriteLine(ex.Message.ToString());
            sw.WriteLine(ex.StackTrace.ToString());
            sw.WriteLine(ex.GetBaseException().Message);
        }
        public void WriteXML( string FileName, Object o )
        {
            XmlSerializer x = new XmlSerializer( o.GetType() );
            x.Serialize( OpenFileForWrite( FileName ), o );
        }
    }
}