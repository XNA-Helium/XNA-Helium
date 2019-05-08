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
using Engine.Core;

using Name.GameState;
using Name.Menus;

namespace Name
{
    public class BaseAIFSM
    {
        protected enum FiniteStateMachine { MoveTowardsHealthPack, PickTarget, MoveTowardsTarget, JetPack, FindHigherGround, SelectWeapon, Deploy, AimBegin, AimEnd, FireRight, FireLeft, Run, EndTurn };

        protected NameTeam myteam;

        protected PlayerObject Me;
        protected PlayerObject Enemy;

        protected FiniteStateMachine state;
        
        System.Random rand = new Random();

        protected int maxShootDistanceOffset = 0;
        protected int healthPackCount = 0;
        protected float NextEventAt = 0;

        protected Vector2 directionrun = Vector2.Zero;
        protected Vector2 TargetPosition = new Vector2();
        protected Vector2 jetPackVector = new Vector2();
        protected Vector2 position = new Vector2();

        protected bool useCluster = false;
        protected bool useJetPack = false;
        protected bool useNuke = false;
        protected bool getPack = false;
        protected bool AnimationPicked = false;
        protected bool ClassicMode = false;
        GameOptions Options;

        protected Vector2 CurrentHitPoint = Vector2.Zero;

        public float IsShooting
        {
            get
            {
                return currentFiringVelocity;
            }
        }

        public BaseAIFSM( PlayerObject po, NameTeam team, GameOptions options )
        {
            ThisFrame = false;
            Options = options;
            if ( Options.ShootMode == ShootMode.Classic )
            {
                ClassicMode = true;
            }

            PlayingState ps = Engine.GameState.GameStateSystem.Instance.GetCurrentState as PlayingState;

            Me = po;
            myteam = team;

            Vector2 position = Me.Position;
            state = FiniteStateMachine.PickTarget;

            NameTeam target;
            {// Pick which team to attack
                CircularObjectList<NameTeam> pms = ps.NameTeams;
                if (pms.First.Value != myteam)
                {
                    target = pms.First.Value;
                }
                else
                {
                    target = pms.Last.Value;
                }
                int currentMax = target.HP;
                foreach (NameTeam utp in pms)
                {
                    if (utp.HP >= currentMax && utp != myteam)
                    {
                        target = utp;
                        // Since AI should always be the last player index
                        // AI will attack other AI if all HP are equal
                        // This lets our players "win" more often which players like
                        // to change this make it > currentMax not >= currentMax
                    }
                }
            }
            {// Pick which avatar to attack
                float distance = 8000;
                foreach (PlayerObject enemy in target.TeamMembers)
                {
                    if ( enemy.InWorld )
                    {
                        Vector2 enemyposition = enemy.Position;
                        float newdistance = ( enemyposition - position ).X;
                        if ( Math.Abs( newdistance ) < Math.Abs( distance ) )
                        {
                            distance = newdistance;
                            TargetPosition = enemyposition;
                        }
                    }
                }
            }
        }


        float currentFiringVelocity = 0.0f;
        bool wasTouched;
        bool ThisFrame;

        public virtual void Update(float ElapsedTime)
        {
            if (! Me.InWorld )
            {
                return;
            }
            Vector2 position = Me.Position;
            float distance = (TargetPosition - position).X;

            switch(state)
            {
                case FiniteStateMachine.PickTarget:
                    {
                        if (ElapsedTime > 2.0f)
                        {
                            state = FiniteStateMachine.MoveTowardsTarget;
                            NextEventAt = ElapsedTime;
                        }
                    }
                    break;
                case FiniteStateMachine.MoveTowardsTarget:
                    {
                        const int MaxShotDistance = 375;
                        const int MinShot = 32;
                        if (distance > MaxShotDistance)
                        {
                            Right();
                        }
                        else if (distance < -MaxShotDistance)
                        {
                            Left();
                        }
                        else if (distance > -MinShot && distance < MinShot)
                        {
                            if (distance > 0)
                            {
                                if ( position.X < 2500 )
                                {

                                    Left();
                                }
                                else
                                {
                                    Right();
                                }

                            }
                            else if (distance <= 0)
                            {
                                if (position.X > 0)
                                {
                                    Right();
                                }
                                else
                                {
                                    Left();
                                }
                            }
                        }
                        else
                        {
                            state = FiniteStateMachine.FindHigherGround;
                            NextEventAt = ElapsedTime + 0.2f;
                        }
                    }
                    break;
                case FiniteStateMachine.FindHigherGround:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            state = FiniteStateMachine.SelectWeapon;
                            NextEventAt = ElapsedTime + 0.1f;
                        }
                        else
                        {
                            if (distance > 0.0f)
                            {
                                Right();
                            }
                            else if (distance < 0.0f)
                            {
                                Left();
                            }
                        }
                    }
                    break;
                case FiniteStateMachine.SelectWeapon:
                    {
                        AvailableWeaponList weapons =  myteam.WeaponList;
                        state = FiniteStateMachine.Deploy;
                        Me.SwapWeapon( new ClusterLauncher( Me ) );
                    }
                    break;
                case FiniteStateMachine.Deploy:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            Me.DeployAndShot();
                            NextEventAt = ElapsedTime + 0.8f;
                            state = FiniteStateMachine.AimBegin;
                        }
                    }
                    break;
                case FiniteStateMachine.AimBegin:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            NextEventAt = ElapsedTime + 0.2125f;
                            state = FiniteStateMachine.AimEnd;
                        }
                    }
                    break;
                case FiniteStateMachine.AimEnd:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            HoldShoot();
                            NextEventAt = ElapsedTime + 7.5f;
                            if (Me.Facing == Facing.Left)
                            {
                                state = FiniteStateMachine.FireLeft;
                            }
                            else if(Me.Facing == Facing.Right)
                            {
                                state = FiniteStateMachine.FireRight;
                            }else{
                                //Now what?
                            }
                        }
                        else
                        {
                            Up();
                        }
                    }
                    break;
                case FiniteStateMachine.FireRight:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            HoldShoot();
                            state = FiniteStateMachine.Run;
                            NextEventAt = ElapsedTime + 4.0f;
                        }
                        else
                        {
                            float distance2 = TargetPosition.X - ( CurrentHitPoint.X );
                            if (distance2 > -0.5f)
                            {
                                HoldShoot();
                            }
                            else
                            {
                                state = FiniteStateMachine.Run;
                                NextEventAt = ElapsedTime + 4.0f;
                            }
                        }
                    }
                    break;
                case FiniteStateMachine.FireLeft:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            HoldShoot();
                            state = FiniteStateMachine.Run;
                            NextEventAt = ElapsedTime + 4.0f;
                        }
                        else
                        {
                            float distance2 = TargetPosition.X - ( CurrentHitPoint.X );
                            if (distance2 < 0.5f )
                            {
                                HoldShoot();
                            }
                            else
                            {
                                state = FiniteStateMachine.Run;
                                NextEventAt = ElapsedTime + 4.0f;
                            }
                        }
                    }
                    break;
                case FiniteStateMachine.Run:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            //@@ always move 1 unit to re-center camera
                            if (distance > 0)
                            {
                                Right();
                            }
                            else if (distance < 0)
                            {
                                Left();
                            }
                            state = FiniteStateMachine.EndTurn;
                            NextEventAt = ElapsedTime + 4.0f;
                        }
                    }
                    break;
                case FiniteStateMachine.EndTurn:
                    {
                        Engine.MenuSystem.Instance.PopTopMenu();
                    }
                    break;
                default:
                    break;
            }


            if ( wasTouched && !ThisFrame ) //&& (state == FiniteStateMachine.FireLeft || state == FiniteStateMachine.FireRight) )
            {
                myteam.WeaponList.Decriment( Me.Weapon );
                IProjectile po = Me.Weapon.Fire( currentFiringVelocity );
                Engine.MenuSystem.Instance.PushMenu( new ProjectileTracker( po ) );
                wasTouched = false;
                Me.Retract();
            }
            if (!AnimationPicked )
            {
                Me.Stand();
            }
            AnimationPicked = false;
            ThisFrame = false;
        }

        
        public void NoLongerInWorld( )
        {
            Engine.MenuSystem.Instance.PopTopMenu();
        }

        public void Left( )
        {
            if ( !Me.InWorld )
            {
                NoLongerInWorld();
                return;
            }
            Me.Walk( Facing.Left ); AnimationPicked = true;
            ( Me[Physics2D.TypeStatic] as Physics2D ).AddVelocity =  -2 * Vector2.UnitX.ToVector3() ;
            Point point = Me.Position.ToPoint();
            CameraManager.Instance.Current.CenterOn(ref point );
        }

        public void Right( )
        {
            if ( !Me.InWorld )
            {
                NoLongerInWorld();
                return;
            }
            Me.Walk( Facing.Right ); AnimationPicked = true;
            ( Me[Physics2D.TypeStatic] as Physics2D ).AddVelocity = 2 * Vector2.UnitX.ToVector3();
            Point point = Me.Position.ToPoint();
            CameraManager.Instance.Current.CenterOn( ref point );
        }

        public static float RotValue = .10f;
        public static float MaxValue = 1.5f;
        public static float MinValue = -1.5f;
        void Up( )
        {
            if ( Me.Facing == Engine.Core.Facing.Left )
            {
                Me.Weapon.Rotation += RotValue;

                if ( Me.Weapon.Rotation >= MaxValue )
                {
                    Me.Weapon.Rotation = MaxValue;
                }
                else if ( Me.Weapon.Rotation <= MinValue )
                {
                    Me.Weapon.Rotation = MinValue;
                }

                if ( Me.Weapon is FlameThrower ) // to prevent torching self
                {
                    if ( Me.Weapon.Rotation >= Math.PI / 7.35f )
                    {
                        Me.Weapon.Rotation = ( float ) Math.PI / 7.35f;
                    }
                    else if ( Me.Weapon.Rotation <= -1.5f )
                    {
                        Me.Weapon.Rotation = -1.5f;
                    }
                }
            }
            else if ( Me.Facing == Engine.Core.Facing.Right )
            {
                Me.Weapon.Rotation -= RotValue;
                if ( Me.Weapon.Rotation >= MaxValue )
                {
                    Me.Weapon.Rotation = MaxValue;
                }
                else if ( Me.Weapon.Rotation <= MinValue )
                {
                    Me.Weapon.Rotation = MinValue;
                }

                if ( Me.Weapon is FlameThrower ) // to prevent torching self
                {
                    if ( Me.Weapon.Rotation >= Math.PI / 7.35f )
                    {
                        Me.Weapon.Rotation = ( float ) Math.PI / 7.35f;
                    }
                    else if ( Me.Weapon.Rotation <= Math.PI / -7.35f )
                    {
                        Me.Weapon.Rotation = ( float ) Math.PI / -7.35f;
                    }
                }
            }
        }

        public static float FiringVelocity = 0.15f;
        void Down( )
        {
            if ( Me.Facing == Engine.Core.Facing.Left )
            {
                Me.Weapon.Rotation -= RotValue;

                if ( Me.Weapon.Rotation >= MaxValue )
                {
                    Me.Weapon.Rotation = MaxValue;
                }
                else if ( Me.Weapon.Rotation <= MinValue )
                {
                    Me.Weapon.Rotation = MinValue;
                }

                if ( Me.Weapon is FlameThrower ) // to prevent torching self
                {
                    if ( Me.Weapon.Rotation >= Math.PI / 7.35f )
                    {
                        Me.Weapon.Rotation = ( float ) Math.PI / 7.35f;
                    }
                    else if ( Me.Weapon.Rotation <= Math.PI / -7.35f )
                    {
                        Me.Weapon.Rotation = ( float ) Math.PI / -7.35f;
                    }
                }
            }
            else if ( Me.Facing == Engine.Core.Facing.Right )
            {
                Me.Weapon.Rotation += RotValue;
                if ( Me.Weapon.Rotation >= MaxValue )
                {
                    Me.Weapon.Rotation = MaxValue;
                }
                else if ( Me.Weapon.Rotation <= MinValue )
                {
                    Me.Weapon.Rotation = MinValue;
                }

                if ( Me.Weapon is FlameThrower ) // to prevent torching self
                {
                    if ( Me.Weapon.Rotation >= Math.PI / 7.35f )
                    {
                        Me.Weapon.Rotation = ( float ) Math.PI / 7.35f;
                    }
                    else if ( Me.Weapon.Rotation <= MinValue )
                    {
                        Me.Weapon.Rotation = MinValue;
                    }
                }
            }
        }
        public void HoldShoot( )
        {
            if ( FiringVelocity >= 90.0f || Me.Weapon is FlameThrower )
            {
                SoundManager.Instance.Play("flamethrower");
                float jitter = ( float ) ( rand.NextDouble() * ( 3.0 * Utility.DegreeToRadian ) - ( 1.5 * Utility.DegreeToRadian ) );
                Me.Weapon.Rotation += jitter;
                ThisFrame = false;
                wasTouched = true;
                return;
            }

            ThisFrame = true;
            wasTouched = true;
            currentFiringVelocity += FiringVelocity;
            if ( currentFiringVelocity > 89.0f )
            {
                currentFiringVelocity = 89.0f;
            }
            {
                //If the launcher is not yet deployed this looks bad
                position = ( Me.Weapon[Placeable.TypeStatic] as Placeable ).Position.ToVector2();
                Terrain t = ( Engine.GameState.GameStateSystem.Instance.GetCurrentState as PlayingState ).Terrain;
                Vector2 t_gravity = Physic2DManagerWithTerrain.gravity.ToVector2();
                Vector2 t_power = Vector2.Zero;
                {
                    Facing t_f = Me.Facing;
                    float angle = Me.Weapon.Rotation;
                    if ( t_f == Facing.Right )
                    {
                        t_power = new Vector2( ( float ) currentFiringVelocity * ( float ) Math.Cos( angle ), ( float ) currentFiringVelocity * ( float ) Math.Sin( angle ) );
                    }
                    if ( t_f == Facing.Left )
                    {
                        t_power = new Vector2( ( float ) -currentFiringVelocity * ( float ) Math.Cos( angle ), ( float ) -currentFiringVelocity * ( float ) Math.Sin( angle ) );
                    }
                }

                while ( t[( int ) position.X] > position.Y )
                {
                    t_power += t_gravity;
                    position += t_power;
                }
                int total = 0;
                for ( int i = -10; i <= 10; ++i )
                {
                    int xthing = ( int ) ( i * 7.5 ) + ( int ) position.X;
                    total += t[xthing];
                }
                total /= 21;
                position.Y = total;
                CurrentHitPoint = position;
                if ( !ClassicMode )
                {
                    Point point = position.ToPoint();
                    CameraManager.Instance.Current.CenterOn( ref point );
                }
            }
        }
    }
}
