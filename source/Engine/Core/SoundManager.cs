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

namespace Engine.Core
{
    public class SoundManager
    {
        protected float BaseVolume = 0.5f;
        public float Volume
        {
            get
            {
                return BaseVolume;
            }
            set
            {
                BaseVolume = value;
            }
        }

        protected static SoundManager instance;
        protected System.Collections.Generic.Dictionary<String, Object> sounds = new System.Collections.Generic.Dictionary<String, Object>();

        public Dictionary<String, SoundEffectInstance> CurrentlyPlaying = new Dictionary<string, SoundEffectInstance>();

        public static SoundManager Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = new SoundManager();
                }
                return instance;
            }
        }

        public bool Playing( string name )
        {
            if ( CurrentlyPlaying.ContainsKey( name ) )
            {
                if ( CurrentlyPlaying[name].State == SoundState.Playing )
                {
                    return true;
                }
            }
            return false;
        }
        
        public void AddSound( string name, SoundEffect effect )
        {
            sounds[name] = effect;
        }
        public void AddSound( string name, Song effect )
        {
            sounds[name] = effect;
        }
        
        /*
        public void AddSound( string name, SoundBank effect )
        {
            sounds[name] = effect;
        }
        */
        
        public void Update( GameTime gameTime )
        {
            //@@ need to find way to pause music

            List<String> RemoveUs = new List<string>();
            foreach ( String i in CurrentlyPlaying.Keys )
            {
                if ( CurrentlyPlaying[i].State == SoundState.Stopped )
                {
                    RemoveUs.Add( i );
                }
            }
            foreach ( String i in RemoveUs )
            {
                CurrentlyPlaying.Remove( i );
            }
        }

        /*
        public void PlayCue( String BankName, String CueName )
        {

            Object sound = sounds[BankName];
            if ( sound is SoundBank )
            {
                SoundBank s = ( SoundBank ) sound;
                Cue c = s.GetCue( CueName );
            }
        }
        */
        
        public void Play( string Name, float volume, float pitch, float pan, bool loop )
        {
            Object sound = sounds[Name];
            if ( sound is SoundEffect )
            {
                SoundEffect se = ( SoundEffect ) sound;
                SoundEffectInstance i = se.CreateInstance();
                i.Volume = volume * BaseVolume;
                i.Pitch = pitch;
                i.Pan = pan;
                i.Play();
                CurrentlyPlaying[Name] = i;

            }
            else if ( sound is Song )
            {
                Song s = ( Song ) sound;
                MediaPlayer.Volume = volume * BaseVolume;
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play( s );
            }
        }
        public void Play( string Name, float volume, float pitch, float pan )
        {
            Play( Name, volume, pitch, pan, false );
        }
        public void Play( string Name, float volume, float pitch )
        {
            Play( Name, volume, pitch, 0.0f, false );
        }
        public void Play( string Name, float volume )
        {
            Play( Name, volume, 0.0f, 0.0f, false );
        }
        public void Play( string Name )
        {
            Play( Name, BaseVolume, 0.0f, 0.0f, false );
        }
    }
}
