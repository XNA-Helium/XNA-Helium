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
using Engine.GameState;
using Engine;

#if WINDOWS_PHONE
using Microsoft.Phone.Shell;
#endif

namespace Engine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public abstract class EngineGame : Microsoft.Xna.Framework.Game
    {
        protected GraphicsDeviceManager graphics;
        protected SpriteBatch spriteBatch;

        protected bool SaveOnExit = false;

        internal static EngineGame CurrentGame = null;

        protected EngineGame()
        {
#if WINDOWS_PHONE
            //@@ Do we want the game to pause if the screen is locked?
            //@@ Do we want a lack of touches to cause the screen to lock?
            // Not in the immediate term for debugging purposes
            //PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
#endif
#if DEBUG
            Engine.DebugHelper.MinDebugLevel = Engine.DebugHelper.DebugLevels.Trivial;
            Engine.LoggingSystem.Instance.Log("Begin Logging", LoggingSystem.LoggingLevels.Important);
#else
            Engine.DebugHelper.MinDebugLevel = Engine.DebugHelper.DebugLevels.IgnoreAll;
#endif
            CurrentGame = this;

            Engine.FileMananger fm = FileMananger.Instance;
#if WINDOWS_PHONE
            Engine.Core.IsoViewer.GetIsolatedStorageView("*");
#endif
            Engine.Core.IDManager.GenerateIDs();
            graphics = new GraphicsDeviceManager(this);


            this.TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);
            this.IsFixedTimeStep = true;

#if WINDOWS || XBOX
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = false;
#elif ZUNE
            graphics.PreferredBackBufferWidth = 272; //320;
            graphics.PreferredBackBufferHeight = 480; // 480;
#else 
            // Default to portrate gamec
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = false;  //Go full screen to hide the battery icon, and get back like 10 Pixels up top...
            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            graphics.ApplyChanges();
#endif
            Content.RootDirectory = ".";


#if DEBUG   //On windows the graphics device is not available in the constructor.
            if (graphics != null && graphics.GraphicsDevice != null)
            {
                Engine.LoggingSystem.Instance.Log("Width=" + graphics.GraphicsDevice.DisplayMode.Width + " Height=" + graphics.GraphicsDevice.DisplayMode.Height, LoggingSystem.LoggingLevels.Important);
                Engine.LoggingSystem.Instance.Log("TimeStep=" + TargetElapsedTime.Milliseconds, LoggingSystem.LoggingLevels.Important);
            }
#endif
            
            Engine.ContentManager.SetupContentManager(Content,128);

        }

        private bool exit = false;
        public static void EndGame()
        {
#if DEBUG
            Engine.LoggingSystem.Instance.Log("EndGame() ", LoggingSystem.LoggingLevels.Important);
#endif
            CurrentGame.exit = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
#if DEBUG && WINDOWS //On windows the graphics device is not available in the constructor. It should exist here
            if (graphics != null && graphics.GraphicsDevice != null)
            {
                Engine.LoggingSystem.Instance.Log("Width=" + graphics.GraphicsDevice.DisplayMode.Width + " Height=" + graphics.GraphicsDevice.DisplayMode.Height, LoggingSystem.LoggingLevels.Important);
                Engine.LoggingSystem.Instance.Log("TimeStep=" + TargetElapsedTime.Milliseconds, LoggingSystem.LoggingLevels.Important);
            }
#endif
#if DEBUG
            Engine.LoggingSystem.Instance.Log("Initialize() ", LoggingSystem.LoggingLevels.Important);
#endif
            base.Initialize();
        }
        
        public static void GetScreenResolution(out DisplayOrientation orientation, out int width, out int height, out bool IsFullscreen)
        {
            CurrentGame.GetResolution(out orientation, out width, out height, out IsFullscreen);
        }
        
        public void GetResolution(out DisplayOrientation orientation, out int width, out int height, out bool IsFullscreen)
        {
            orientation = graphics.SupportedOrientations;
            width = graphics.GraphicsDevice.DisplayMode.Width;
            height = graphics.GraphicsDevice.DisplayMode.Height;
            IsFullscreen = graphics.IsFullScreen;
        }

        public static void SwitchToResolution(DisplayOrientation orientation, int width, int height, bool IsFullScreen)
        {
            CurrentGame.SwitchResolution(orientation, width, height, IsFullScreen);
        }

        public virtual void SwitchResolution(DisplayOrientation orientation, int width, int height, bool IsFullScreen)
        {
            graphics.SupportedOrientations = orientation;
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
            graphics.IsFullScreen = IsFullScreen;
            graphics.ApplyChanges();
            CameraManager.Instance.Current.ReSize(width, height);
#if DEBUG
            Engine.LoggingSystem.Instance.Log("Width=" + graphics.GraphicsDevice.DisplayMode.Width + " Height=" + graphics.GraphicsDevice.DisplayMode.Height, LoggingSystem.LoggingLevels.Important);
#endif
        }

        protected override void EndRun()
        {
#if DEBUG
            Engine.LoggingSystem.Instance.Log("EndRun() ", LoggingSystem.LoggingLevels.Important);
#endif
            SaveGameState();
#if DEBUG
            Engine.LoggingSystem.Instance.Log("The following were not loaded: \n" + Engine.ContentManager.FN, LoggingSystem.LoggingLevels.Explosive);
#endif
            base.EndRun();
        }


        /// <summary>
        /// This function sets up your game
        /// </summary>
        protected abstract void LoadGameContent( );

        /// <summary>
        /// This function setups your initial menu
        /// </summary>
        /// <returns>Your initial Mneu</returns>
        protected abstract Engine.UI.BaseMenu FirstMenu();

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
#if DEBUG
            Engine.LoggingSystem.Instance.Log("LoadContent() ", LoggingSystem.LoggingLevels.Important);
#endif
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MenuSystem.SetupMenuSystem(graphics);
            CameraManager.SetupCameraManager( graphics );

            LoadGameContent();

            MenuSystem.Instance.AddMenu( FirstMenu() );
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
#if DEBUG
            Engine.LoggingSystem.Instance.Log("UnloadContent() ", LoggingSystem.LoggingLevels.Important);
#endif
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (exit)
            {
#if DEBUG
                Engine.LoggingSystem.Instance.Log("Exit() ", LoggingSystem.LoggingLevels.Informative);
#endif

#if WINDOWS_PHONE
                Engine.Core.IsoViewer.GetIsolatedStorageView("*");
#endif
                Exit();
                return;
            }
#if DEBUG
            Engine.LoggingSystem.Instance.Log("Update() ", LoggingSystem.LoggingLevels.Informative);
#endif
            base.Update(gameTime);
            EventManager.Instance.Update( gameTime );
            InputSystem.Instance.Update(gameTime);
            MenuSystem.Instance.Update(gameTime);
            GameStateSystem.Instance.Update( gameTime );
        }

        protected override void OnActivated(object sender, EventArgs args)
        { // No Breakpoints here the emulator terminates the application if it takes longer than 10 seconds to reload
#if DEBUG
            Engine.LoggingSystem.Instance.Log("OnActivated() ", LoggingSystem.LoggingLevels.Informative);
#endif
            // Windows Phone Lifecycle Event
            // Occurs when the application is being made active after previously being tombstoned.

            //@@ Load Saved Data Here

            base.OnActivated(sender, args);
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
#if DEBUG
            Engine.LoggingSystem.Instance.Log("OnDeactivated() ", LoggingSystem.LoggingLevels.Informative);
#endif
            // Windows Phone Lifecycle Event
            // Occurs when the application is being deactivated and tombstoned.

            // Application now tombsonted, but not destroyed

            //@@ Save data here  (keep old save or do away w/ it?)

            base.OnDeactivated(sender, args);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
#if DEBUG
            Engine.LoggingSystem.Instance.Log("OnExiting() ", LoggingSystem.LoggingLevels.Informative);
#endif
            // Windows Phone Lifecycle Event
            // Occurs when the application is exiting.

            base.OnExiting(sender, args);
        }

        public static void Save()
        {
            CurrentGame.SaveGameState();
        }

        public virtual void SaveGameState()
        {
#if !DEBUG  //@@ Make this not debug only
            try
            {
#endif
                if (SaveOnExit && Engine.GameState.GameStateSystem.Instance.Count > 0)
                {
#if DEBUG
                    Engine.LoggingSystem.Instance.Log("Started - SavingGameState() ", LoggingSystem.LoggingLevels.Important);
#endif
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(32000);

                    Engine.Persistence.PersistenceManager pm = new Engine.Persistence.PersistenceManager(ms, Engine.Persistence.Mode.Write);

                    // Save Game State
                    Engine.GameState.GameStateSystem.Instance.GetCurrentState.Save(pm);
                    // Save Menu State
                    MenuSystem.Instance.Save(pm, 1); // Don't save back out the MainMenu

                    {
                        System.IO.FileStream sw = FileMananger.Instance.OpenStreamForOverWrite(FileMananger.ExitQuickSave);
                        ms.WriteTo((System.IO.Stream) sw);
                        sw.Flush();
                        sw.Close();
                    }
#if DEBUG
                    Engine.LoggingSystem.Instance.Log("Finished - SavingGameState() ", LoggingSystem.LoggingLevels.Important);
#endif
                }
#if !DEBUG  //@@ Make this not debug only
            }
            catch (Exception ex)
            {
                DebugHelper.Break(ex, DebugHelper.DebugLevels.Explosive);
            }
#endif
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
#if DEBUG
            Engine.LoggingSystem.Instance.Log("Draw() ", LoggingSystem.LoggingLevels.Informative);
#endif

            Camera3D Camera = CameraManager.Instance.Current as Camera3D;
            if(Camera != null)
                Camera.UpdateMatricies();

            GraphicsDevice.Clear(Color.Black);

            GameStateSystem.Instance.Draw( gameTime, spriteBatch );
            MenuSystem.Instance.Draw(gameTime, spriteBatch);
#if DEBUG
            GameStateSystem.Instance.DebugDraw( gameTime, spriteBatch );
            InputSystem.Instance.DebugDraw( gameTime, spriteBatch );
            MenuSystem.Instance.DebugDraw( gameTime, spriteBatch );
#endif
            base.Draw(gameTime);
        }
    }
}
