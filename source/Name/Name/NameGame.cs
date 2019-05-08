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
using Engine.Core;

namespace Name
{

#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (EngineGame game = new NameGame())
            {
                game.Run();
            }
        }
    }
#endif

    public class NameGame : EngineGame
    {
        protected override void LoadGameContent( )
        {
#if DEBUG
            Engine.DebugHelper.MinDebugLevel = Engine.DebugHelper.DebugLevels.Trivial;
#endif
            SaveOnExit = true;
            MenuSystem.PointIsCenter = false;

            FileMananger.Instance.ValidateFile(FileMananger.OptionsFile);
            

            // UPRIGHT MENU ONLY. ONLY GAMEPLAY WORKS IN LANDSCAPE

            {
                // New Type system - Need to register thigns
                Name.WeaponHolder.Initialize<Name.WeaponHolder>( Name.WeaponHolder.ReturnNew );
                Name.ProjectileHolder.Initialize<Name.ProjectileHolder>( Name.ProjectileHolder.ReturnNew );
            }
            
            {
                Name.CarpetBomb.Initialize(CarpetBomb.ReturnNew);
                Name.CarpetBombSingle.Initialize(CarpetBombSingle.ReturnNew);
                Name.CarpetBombLauncher.Initialize(CarpetBombLauncher.ReturnNew);
                Name.ClusterLauncher.Initialize(ClusterLauncher.ReturnNew);
                Name.Flame.Initialize(Flame.ReturnNew);
                Name.FlameExplosion.Initialize( FlameExplosion.ReturnNew );
                Name.FlameThrower.Initialize( FlameThrower.ReturnNew );
                Name.Grenade.Initialize( Grenade.ReturnNew );
                Name.GrenadeCluster.Initialize( GrenadeCluster.ReturnNew );
                Name.GrenadeClusterMask.Initialize( GrenadeClusterMask.ReturnNew );
                Name.GrenadeLauncher.Initialize( GrenadeLauncher.ReturnNew );
                Name.GrenadeSingle.Initialize( GrenadeSingle.ReturnNew );
                Name.HealthPack.Initialize( HealthPack.ReturnNew );
                Name.JetPack.Initialize( JetPack.ReturnNew );
                Name.LandMine.Initialize( LandMine.ReturnNew );
                Name.LandMineLauncher.Initialize( LandMineLauncher.ReturnNew );
                Name.LargeExplosion.Initialize( LargeExplosion.ReturnNew );
                Name.LaunchingRod.Initialize( LaunchingRod.ReturnNew );
                Name.MineExplosion.Initialize( MineExplosion.ReturnNew );
                Name.Napalm.Initialize( Napalm.ReturnNew );
                Name.PlayerHitpoints.Initialize( PlayerHitpoints.ReturnNew );
                Name.PlayerObject.Initialize( PlayerObject.ReturnNew );
                Name.Rocket.Initialize( Rocket.ReturnNew );
                Name.RocketLauncher.Initialize( RocketLauncher.ReturnNew );
                Name.SmallExplosion.Initialize( SmallExplosion.ReturnNew );
                Name.SpiderMine.Initialize( SpiderMine.ReturnNew );
                Name.SpiderMineLauncher.Initialize( SpiderMineLauncher.ReturnNew );
                Name.WeaponExplosion.Initialize( WeaponExplosion.ReturnNew );
            }


            Engine.ContentManager.Instance.Load<SpriteFont>(@"Content\Menu\TitleFont");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Sprites\Projectiles\Grenade"); // We spin this for the loading screen
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Menu\Exit_active");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Menu\Exit_inactive");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Menu\Credits_active");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Menu\Credits_inactive");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Menu\Options_active");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Menu\Options_inactive");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Menu\Start_active");
            Engine.ContentManager.Instance.Load<Texture2D>(@"Content\Menu\Start_inactive");

            Engine.ContentManager.Instance.Load<SpriteFont>(@"SharedContent\TextboxFont");
            Engine.ContentManager.Instance.Load<SpriteFont>(@"SharedContent\TitleFont");

            Engine.ContentManager.Instance.Load<Texture2D>(@"SharedContent\Title_Background");
            Engine.ContentManager.Instance.Load<Texture2D>(@"SharedContent\Background");
            Engine.ContentManager.Instance.Load<Texture2D>(@"SharedContent\Pointer");

            Engine.ContentManager.Instance.DelayLoadSound("button");
            Engine.ContentManager.Instance.DelayLoadSound("charge");
            Engine.ContentManager.Instance.DelayLoadSound("deploy");
            Engine.ContentManager.Instance.DelayLoadSound("expl");
            Engine.ContentManager.Instance.DelayLoadSound("explosion");
            Engine.ContentManager.Instance.DelayLoadSound("flamethrower");
            Engine.ContentManager.Instance.DelayLoadSound("jetpack");
            Engine.ContentManager.Instance.DelayLoadSound("jump");
            Engine.ContentManager.Instance.DelayLoadSound("land");
            Engine.ContentManager.Instance.DelayLoadSound("napalm");
            Engine.ContentManager.Instance.DelayLoadSound("napalm2");
            Engine.ContentManager.Instance.DelayLoadSound("napalm3");
            Engine.ContentManager.Instance.DelayLoadSound("napalm4");
            Engine.ContentManager.Instance.DelayLoadSound("napalm5");
            Engine.ContentManager.Instance.DelayLoadSound("pickup");
            Engine.ContentManager.Instance.DelayLoadSound("shooting");
            Engine.ContentManager.Instance.DelayLoadSound("shooting2");
            Engine.ContentManager.Instance.DelayLoadSound("shooting3");
            Engine.ContentManager.Instance.DelayLoadSound("step");

            Engine.ContentManager.Instance.DelayLoadTexture(@"SharedContent\Credits_Background");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Back_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Back_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"SharedContent\button_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"SharedContent\button_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Players");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\0p_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\0p_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\1p_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\1p_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\2p_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\2p_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\3p_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\3p_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\4p_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\4p_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\AIplayers");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\MiniMap\MiniMap");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\GenerateLevel_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\GenerateLevel_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\MiniMap\BackgroundL");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\MiniMap\BackgroundR");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Previous_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Previous_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Next_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Next_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\MiniMap\TerrainPiece");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\RocketLauncherLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\RocketLauncherRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\StandLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\StandRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\WalkLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\WalkRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\AimLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\AimRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\ShootLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\ShootRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\RodLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\RodRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\RodExtendedLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\RodExtendedRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\BottomBackground");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\RocketActive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\RocketInactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Reticle_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Reticle_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\MiniMap\MiniMapYou");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\MiniMap\HealthPack");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\MiniMap\fire");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\MiniMap\mine");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\MiniMap\WhiteDot");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\infinity");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\ClusterActive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\ClusterInactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\GrenadeActive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\GrenadeInactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\JetPackActive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\JetPackInactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\NapalmActive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\NapalmInactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\SpiderMineActive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\SpiderMineInactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\FlameThrowerActive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\FlameThrowerInactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\MineActive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\MineInactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\ShootingCone");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Reticle");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Up_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Up_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Down_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\Down_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\BackSmall_active");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\BackSmall_inactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\RocketLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\RocketRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Explosions\Explosion2");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\ClusterLauncherLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\ClusterLauncherRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\ClusterGrenade");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\ClusterGrenadeShine");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\Cluster");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Explosions\Explosion3");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\GrenadeLauncherLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\GrenadeLauncherRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\Grenade");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Explosions\Explosion1");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\JetPackActiveLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\JetPackActiveRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\JetPackInactiveLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Gir\JetPackInactiveRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\NapalmLauncherLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\NapalmLauncherRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\Napalm");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\NapalmPiece");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Explosions\NapalmFireBig");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Explosions\NapalmFireMed");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Explosions\NapalmFireSmall");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\SpiderMineLauncherLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\SpiderMineLauncherRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\SpiderMine");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\SpiderMineRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\SpiderMineLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\FlamethrowerLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\FlamethrowerRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\FlamethrowerActive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Menu\FlamethrowerInactive");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Explosions\FlameLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Explosions\FlameRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Items\HealthPack");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\MineLauncherLeft");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Weapons\MineLauncherRight");
            Engine.ContentManager.Instance.DelayLoadTexture(@"Content\Sprites\Projectiles\Mine");
#if DEBUG
            Engine.ContentManager.Instance.LogFileNames = true; // We will get a message if anything is loaded after this point
#endif
        }

        protected override Engine.UI.BaseMenu FirstMenu( )
        {
            EngineGame.SwitchToResolution(DisplayOrientation.Portrait, 480, 800, false);
            return new Name.Menus.MainMenu();
        }

    }
}
