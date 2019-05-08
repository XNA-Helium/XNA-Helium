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

namespace Engine
{
    public class ContentManager
    {
        public bool LogFileNames = false;
        System.Collections.Generic.Dictionary<string, object> _ht;
        System.Collections.Generic.Dictionary<object, string> ReverseLookUp;
        
        private Microsoft.Xna.Framework.Content.ContentManager contentManager;

        private static ContentManager instance;
        public static ContentManager Instance
        {
            get
            {
                return instance;
            }
        }
        public static void SetupContentManager(Microsoft.Xna.Framework.Content.ContentManager ContentManager, int Capacity)
        {
            instance = new ContentManager(ContentManager, Capacity);
        }
        protected ContentManager(Microsoft.Xna.Framework.Content.ContentManager ContentManager,  int Capacity)
        {
            contentManager = ContentManager;
            _ht = new System.Collections.Generic.Dictionary<string, object>(Capacity);
            ReverseLookUp = new Dictionary<object, string>(Capacity);
        }

        public void SaveContent<T>(T Object, string Name)
        {
            _ht[Name] = Object;
            ReverseLookUp[Object] = Name;
        }

        public void Load<T>(string Name,string Identifier)
        {
            Load<T>(Name);
            _ht[Identifier] = _ht[Name];
            ReverseLookUp[( (object) _ht[Name] )] = Name;
        }

        Queue<String> ModelList = new Queue<String>();
        public void DelayLoadModel( String Name )
        {
            ModelList.Enqueue( Name );
        }
        Queue<String> TextureList = new Queue<String>();
        public void DelayLoadTexture( String Name )
        {
            TextureList.Enqueue( Name );
        }
        Queue<String> SoundList = new Queue<String>();
        public void DelayLoadSound( String Name )
        {
            SoundList.Enqueue( Name );
        }

        public int LoadCount( int count )
        {
            bool temp = false;
            if ( LogFileNames )
            {
                temp = true;
                LogFileNames = false;
            }

            int RunStart = 0;
            for ( int i = 0; i < count; )
            {
                RunStart = i;
                if (SoundList.Count > 0 && i < count)
                {
                    String sound = SoundList.Dequeue();
                    Engine.Core.SoundManager.Instance.AddSound(sound, Engine.ContentManager.Instance.GetObject<SoundEffect>(@"Content\sounds\" + sound));
                    ++i;
                }
                if (ModelList.Count > 0 && i < count)
                {
                    Load<Model>( ModelList.Dequeue() );
                    ++i;
                }
                if (TextureList.Count > 0 && i < count)
                {
                    Load<Texture2D>( TextureList.Dequeue() );
                    ++i;
                }
                if (RunStart == i && i < count)
                {
                    LogFileNames = temp;
                    return 0;
                }
            }
            LogFileNames = temp;
            return ModelList.Count + TextureList.Count + SoundList.Count;
        }

        public static string FN;
        public void Load<T>(string Name)
        {
            if ( LogFileNames )
            {
                switch ( typeof( T ).ToString() )
                {
                    case "Microsoft.Xna.Framework.Graphics.Texture2D":
                        FN += "Engine.ContentManager.Instance.DelayLoadTexture(\"" + Name + "\");\n";
                        break;
                    case "Microsoft.Xna.Framework.Audio.SoundEffect":
                        FN += "Engine.ContentManager.Instance.DelayLoadSound(\"" + Name + "\");\n";
                        break;
                    case "Model":
                        FN += "Engine.ContentManager.Instance.DelayLoadModel(\"" + Name + "\");\n";
                        break;
                    default:
                        FN += "Engine.ContentManager.Instance.Load<" + typeof( T ) + ">(\"" + Name + "\");\n";
                        break;
                }
            }
#if DEBUG
            Engine.LoggingSystem.Instance.Log("Loading " + Name + " " + typeof(T).ToString() , LoggingSystem.LoggingLevels.Informative);
#endif
            _ht[Name] = contentManager.Load<T>(Name);

            ReverseLookUp[ _ht[Name] ] = Name;
        }

        public T GetObject<T>(string Name)
        {
            if (_ht.ContainsKey(Name))
            {
                return ((T)_ht[Name]);
            }
            else
            {
                Load<T>(Name);
                if (_ht.ContainsKey(Name))
                {
                    return ((T)_ht[Name]);
                }
                else
                {
                    Engine.DebugHelper.Break( "Write the TextureNotFound Texture", DebugHelper.DebugLevels.Curious );
                    return ((T)_ht[Name]);
                }
            }
        }

        public string ReturnName(object o)
        {
            if (_ht.ContainsValue(o))
            {
                return ReverseLookUp[o];
            }
                DebugHelper.Break(DebugHelper.DebugLevels.Explosive);
#if DEBUG
                LoggingSystem.Instance.Log("Can't Find : " + o.ToString(), LoggingSystem.LoggingLevels.Explosive);
#endif
                return "Not Found";
        }
    }
}
