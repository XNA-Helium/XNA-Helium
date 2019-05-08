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

namespace Name
{
    public class SmartestAIFSM : BaseAIFSM
    {
        public SmartestAIFSM( PlayerObject po, NameTeam team, GameOptions options )
            : base( po, team, options )
        {
        }
        /*
        
        public SmartestAI(FakeGamePad gp, UserTeamPlayerManager UTPM, List<GameObject> sg)
            : base(gp, UTPM, sg)
        {
            float x = (float)rand.NextDouble();
            float y = (float)rand.NextDouble() + .4f;

            if (rand.Next() % 2 == 0)
                x = -x;

            jetPackVector = new Vector2(x, y);

            // determines if a nuke should be used
            if (UTPM.GetEnemyAvatars().Count != 0)
            {
                List<PlayerObject> sl = (new SortedList<PlayerObject>(UTPM.GetEnemyAvatars(), SmarterAI.GetDistance, SortedList<PlayerObject>.Less)).GetList();
                List<PlayerObject>.Enumerator em = sl.GetEnumerator();
                em.MoveNext(); // move to first node

                float currentDistance = Math.Abs(em.Current.Position().X);
                TargetPosition = em.Current.Position();
                em.MoveNext();

                for (int i = 1; i < sl.Count; i++)
                {
                    float newDistance = Math.Abs(em.Current.Position().X);

                    // enemies are within 80 pixels of each other, use a nuke
                    if (Math.Abs(currentDistance - newDistance) <= 80 && UTPM.Current.AvailableWeapons.GetAvailable(AvailableWeaponList.LauncherType.NukeLauncher) != 0)
                    {
                        int random = rand.Next(1, 100);
                        if (random <= 50) // 50% chance of using nuke
                        {
                            useNuke = true;
                            Enemy = em.Current;
                            TargetPosition = (TargetPosition + em.Current.Position()) / 2.0f;
                            break;
                        }
                        else if (Math.Abs(currentDistance - newDistance) <= 50)
                        {
                            useCluster = true;
                            Enemy = em.Current;
                            TargetPosition = (TargetPosition + em.Current.Position()) / 2.0f;
                            break;
                        }
                        else
                            break;
                    }
                    else
                    {
                        currentDistance = newDistance;
                        TargetPosition = em.Current.Position();
                        em.MoveNext();
                    }
                }
            }

            healthPackCount = Game1.Instance.GameManager._Collectables.Count;
            foreach (GameObject go in Game1.Instance.GameManager._Collectables)
            { // finds and moves towards health packs within 1500 units of avatar
                if (go is HealthPack && Math.Abs((go[ComponentType.Game_Position] as Game_Position).X - Me.Position().X) < 1500)
                {
                    TargetPosition = (go[ComponentType.Game_Position] as Game_Position).Position;
                    getPack = true;
                    state = FiniteStateMachine.MoveTowardsHealthPack;
                }
            } // end health pack

            if (!useNuke && !getPack)
            {
                FindTarget();
            }

            direction = Vector2.Zero;
            directionrun = Vector2.Zero;
        }

        public override void Update(float ElapsedTime)
        {
            if (maxShootDistanceOffset == 0)
            {
                maxShootDistanceOffset = rand.Next(0, 200);
            }

            Vector2 position = Me.Position();
            float distance = (TargetPosition - position).X;
            bool move = true, turn = true;
            WeaponObject currWeapon = (Me[ComponentType.Holder_Weapon] as Holder_Weapon).Weapon;

            switch (state)
            {
                case FiniteStateMachine.MoveTowardsHealthPack:
                    {
                        if (healthPackCount > Game1.Instance.GameManager._Collectables.Count)
                        {
                            move = false;
                            FindTarget();
                            state = FiniteStateMachine.MoveTowardsTarget;
                            NextEventAt = ElapsedTime + 17.0f;
                        }

                        foreach (GameObject go in Game1.Instance.GameManager._Collectables)
                        {
                            if (go is Napalm)
                            {
                                if (Math.Abs(((go[ComponentType.Game_Position] as Game_Position).X - position.X)) <= 40)
                                {
                                    move = false;
                                    FindTarget();
                                    state = FiniteStateMachine.MoveTowardsTarget;
                                    NextEventAt = ElapsedTime + 17.0f;
                                }
                            }
                            else if (go is LandMine)
                            {
                                Game_Position gp = (go[ComponentType.Game_Position] as Game_Position);

                                if (Math.Abs(gp.Position.X - position.X) <= 80)
                                {
                                    if (Math.Abs(gp.Position.X - position.X) <= 25)
                                    {
                                        int random = rand.Next(1, 100);
                                        if (random <= 80) // 20% chance of using jetpack
                                        {
                                            move = false;
                                            turn = false;
                                            useJetPack = true;
                                        }
                                        break;
                                    }
                                    else // shoot the mine
                                    {
                                        foundNewTarget = true;
                                        TargetPosition = gp.Position;
                                        state = FiniteStateMachine.SelectWeapon;
                                        NextEventAt = ElapsedTime + 2.0f;
                                        move = false;
                                    }
                                }
                            }
                        }

                        if (move)
                        {
                            if (distance > 5f)
                            {
                                gamepad.SetLeftStickState(new Vector2(1.0f, 0.0f));
                            }
                            else if (distance < -5f)
                            {
                                gamepad.SetLeftStickState(new Vector2(-1.0f, 0.0f));
                            }
                            else
                            {
                                FindTarget();
                                state = FiniteStateMachine.PickTarget;
                                NextEventAt = ElapsedTime + 0.2f;
                            }
                        }
                    }
                    break;
                case FiniteStateMachine.PickTarget:
                    {
                        if (ElapsedTime > 2.0f)
                        {
                            state = FiniteStateMachine.MoveTowardsTarget;
                            NextEventAt = ElapsedTime + 17.0f; ;
                        }
                    }
                    break;
                case FiniteStateMachine.MoveTowardsTarget:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            state = FiniteStateMachine.SelectWeapon;
                            NextEventAt = ElapsedTime + 0.2f;
                        }
                        else
                        {
                            foreach (GameObject go in Game1.Instance.GameManager._Collectables)
                            {
                                if (go is Napalm)
                                {
                                    if (Math.Abs(((go[ComponentType.Game_Position] as Game_Position).X - position.X)) <= 40)
                                    {
                                        int random = rand.Next(1, 100);
                                        if (random <= 80)
                                        {
                                            move = false;
                                            break;
                                        }
                                        else
                                            break;
                                    }
                                }
                                else if (go is LandMine)
                                {
                                    Game_Position gp = (go[ComponentType.Game_Position] as Game_Position);

                                    if (Math.Abs(gp.Position.X - position.X) <= 80)
                                    {
                                        if (Math.Abs(gp.Position.X - position.X) <= 25)
                                        {
                                            int random = rand.Next(1, 100);
                                            if (random <= 80) // 20% chance of blowing self up
                                            {
                                                move = false;
                                                turn = false;
                                                useJetPack = true;
                                            }
                                            break;
                                        }
                                        else // shoot the target
                                        {
                                            foundNewTarget = true;
                                            TargetPosition = gp.Position;
                                            state = FiniteStateMachine.SelectWeapon;
                                            NextEventAt = ElapsedTime + 2.0f;
                                            move = false;
                                        }
                                    }
                                }
                            }

                            if (move)
                            {
                                int MaxShotDistance = 375 + maxShootDistanceOffset;
                                const int MinShot = 32;
                                if (distance > MaxShotDistance)
                                {
                                    gamepad.SetLeftStickState(new Vector2(1.0f, 0.0f));
                                }
                                else if (distance < -MaxShotDistance)
                                {
                                    gamepad.SetLeftStickState(new Vector2(-1.0f, 0.0f));
                                }
                                else if (Math.Abs(distance) < MinShot && direction == Vector2.Zero)
                                {
                                    if (distance > 0)
                                    {
                                        if (position.X < 1870)
                                        {
                                            direction = new Vector2(1.0f, 0.0f);
                                            gamepad.SetLeftStickState(direction);
                                        }
                                        else
                                        {
                                            direction = new Vector2(-1.0f, 0.0f);
                                            gamepad.SetLeftStickState(direction);
                                        }

                                    }
                                    else if (distance <= 0)
                                    {
                                        if (position.X > 50)
                                        {
                                            direction = new Vector2(-1.0f, 0.0f);
                                            gamepad.SetLeftStickState(direction);
                                        }
                                        else
                                        {
                                            direction = new Vector2(1.0f, 0.0f);
                                            gamepad.SetLeftStickState(direction);
                                        }
                                    }
                                }
                                else if (distance > -MinShot && distance < MinShot && direction != Vector2.Zero)
                                {
                                    gamepad.SetLeftStickState(direction);
                                }
                                else
                                {
                                    state = FiniteStateMachine.FindHigherGround;
                                    NextEventAt = ElapsedTime + 0.2f;
                                }
                            }
                            else if (!useJetPack) // do not move, make sure facing target, move slightly to face target
                            {
                                if (distance > 0 && Me.GetFacing() == Facing.Left)
                                {
                                    if (turn)
                                    {
                                        direction = new Vector2(1.0f, 0.0f);
                                        gamepad.SetLeftStickState(direction);
                                        (Me[ComponentType.Game_Facing] as Game_Facing).Facing = Facing.Right;
                                    }
                                }
                                else if (distance < 0 && Me.GetFacing() == Facing.Right)
                                {
                                    if (turn)
                                    {
                                        direction = new Vector2(-1.0f, 0.0f);
                                        gamepad.SetLeftStickState(direction);
                                        (Me[ComponentType.Game_Facing] as Game_Facing).Facing = Facing.Left;
                                    }
                                }
                                else
                                {
                                    gamepad.SetLeftStickState(Vector2.Zero);
                                    state = FiniteStateMachine.SelectWeapon;
                                    NextEventAt = ElapsedTime + 0.2f;
                                }
                            }
                            else
                            {
                                state = FiniteStateMachine.SelectWeapon;
                                NextEventAt = ElapsedTime + 0.2f;
                            }
                        }
                    }
                    break;
                case FiniteStateMachine.JetPack:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            (gamepad.B as FakeButtonWrapper).ClearRegisterDown();
                            gamepad.SetState(Buttons.B, InputWrapper.ButtonState.Held);
                            NextEventAt = ElapsedTime + 2.0f;
                            state = FiniteStateMachine.Run;
                        }
                        else
                        {
                            gamepad.SetLeftStickState(jetPackVector);
                        }
                    }
                    break;
                case FiniteStateMachine.FindHigherGround:
                    {
                        base.Update(ElapsedTime);
                    }
                    break;
                case FiniteStateMachine.SelectWeapon:
                    {
                        AvailableWeaponList weapons = this.UTPM.Current.AvailableWeapons;

                        if (foundNewTarget)
                        {
                            state = SelectWeapon(ElapsedTime, (int)AvailableWeaponList.LauncherType.ClusterLauncher); // make spider mine launcher when fixed
                        }
                        else if (useJetPack)
                        {
                            state = SelectWeapon(ElapsedTime, (int)AvailableWeaponList.LauncherType.JetPackLauncher);
                        }
                        else if (useCluster)
                            state = SelectWeapon(ElapsedTime, (int)AvailableWeaponList.LauncherType.ClusterLauncher);

                        else if (useNuke && weapons.GetAvailable(AvailableWeaponList.LauncherType.NukeLauncher) != 0)
                            state = SelectWeapon(ElapsedTime, (int)AvailableWeaponList.LauncherType.NukeLauncher);

                        else if (Math.Abs(distance) <= 400)
                            state = SelectWeapon(ElapsedTime, (int)AvailableWeaponList.LauncherType.RocketLauncher);

                        else if (Math.Abs(distance) <= 650)
                            state = SelectWeapon(ElapsedTime, (int)AvailableWeaponList.LauncherType.ClusterLauncher);

                        else if (Math.Abs(distance) <= 1000 && weapons.GetAvailable(AvailableWeaponList.LauncherType.MineLauncher) != 0)
                            state = SelectWeapon(ElapsedTime, (int)AvailableWeaponList.LauncherType.MineLauncher);

                        else if (weapons.GetAvailable(AvailableWeaponList.LauncherType.NapalmLauncher) != 0)
                            state = SelectWeapon(ElapsedTime, (int)AvailableWeaponList.LauncherType.NapalmLauncher);

                        else
                            state = SelectWeapon(ElapsedTime, (int)AvailableWeaponList.LauncherType.ClusterLauncher);
                    }
                    break;
                case FiniteStateMachine.Deploy:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            (gamepad.A as FakeButtonWrapper).ClearRegisterDown();
                            gamepad.SetState(Buttons.A, InputWrapper.ButtonState.Held);

                            if (currWeapon is JetPack)
                            {
                                NextEventAt = ElapsedTime + 2.0f;
                                state = FiniteStateMachine.JetPack;
                            }
                            else
                            {
                                NextEventAt = ElapsedTime + 0.8f;
                                state = FiniteStateMachine.AimBegin;
                            }
                        }
                    }
                    break;
                case FiniteStateMachine.AimBegin:
                    {
                        base.Update(ElapsedTime);
                    }
                    break;
                case FiniteStateMachine.AimEnd:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            gamepad.SetState(Buttons.A, InputWrapper.ButtonState.Held);

                            NextEventAt = ElapsedTime + 7.5f;

                            if (Me.GetFacing() == Facing.Left)
                            {
                                state = FiniteStateMachine.FireLeft;
                            }
                            else if (Me.GetFacing() == Facing.Right)
                            {
                                state = FiniteStateMachine.FireRight;
                            }
                        }
                        else
                        {
                            gamepad.SetLeftStickState(new Vector2(0.0f, 1.0f));
                        }
                    }
                    break;
                case FiniteStateMachine.FireRight:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            gamepad.SetState(Buttons.A, InputWrapper.ButtonState.Held);
                            state = FiniteStateMachine.Run;
                            NextEventAt = ElapsedTime + 2.0f;
                        }
                        else
                        {
                            float distance2 = TargetPosition.X - (Game1.Instance.CurrentViewport.X + (0.5f * Game1.Instance.CurrentViewport.Width));
                            if (distance2 > 2.0f)
                            {
                                gamepad.SetState(Buttons.A, InputWrapper.ButtonState.Held);
                            }
                            else
                            {
                                state = FiniteStateMachine.Run;
                                NextEventAt = ElapsedTime + 2.0f;
                            }
                        }
                    }
                    break;
                case FiniteStateMachine.FireLeft:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            gamepad.SetState(Buttons.A, InputWrapper.ButtonState.Held);
                            state = FiniteStateMachine.Run;
                            NextEventAt = ElapsedTime + 2.0f;
                        }
                        else
                        {
                            float distance2 = TargetPosition.X - (Game1.Instance.CurrentViewport.X + (0.5f * Game1.Instance.CurrentViewport.Width));

                            if (distance2 < -2.0f)
                            {
                                gamepad.SetState(Buttons.A, InputWrapper.ButtonState.Held);
                            }
                            else
                            {
                                state = FiniteStateMachine.Run;
                                NextEventAt = ElapsedTime + 2.0f;
                            }
                       } 
                    }
                    break;
                case FiniteStateMachine.Run:
                    {
                        if (ElapsedTime > NextEventAt)
                        {
                            if (UTPM.GetAllAvatars().Count != 0 && move)
                            {
                                List<PlayerObject> sl = (new SortedList<PlayerObject>(UTPM.GetAllAvatars(), SmarterAI.GetDistance, SortedList<PlayerObject>.Less)).GetList();
                                List<PlayerObject>.Enumerator em = sl.GetEnumerator();
                                em.MoveNext(); // move to first non-empty node

                                bool[] stopMoving = new bool[sl.Count];

                                for (int i = 0; i < sl.Count; i++)
                                {
                                    float currentEnemyPosition = Math.Abs(em.Current.Position().X);
                                    float myPosition = Me.Position().X;

                                    if (Math.Abs(myPosition - currentEnemyPosition) > 80)
                                    {
                                        stopMoving[i] = true;
                                    }
                                    else
                                    {
                                        stopMoving[i] = false;
                                    }

                                    em.MoveNext();
                                }

                                int stopCount = 0;
                                for (int i = 0; i < sl.Count; i++)
                                {
                                    if (stopMoving[i])
                                        stopCount++;
                                }

                                if (rand.Next() % 2 == 0 && direction == Vector2.Zero)
                                    direction = new Vector2(1.0f, 0.0f);
                                else if (direction == Vector2.Zero)
                                    direction = new Vector2(-1.0f, 0.0f);

                                if (stopCount < sl.Count - 1)
                                {
                                    gamepad.SetLeftStickState(direction);
                                }
                                else
                                {
                                    state = FiniteStateMachine.EndTurn;
                                    NextEventAt = ElapsedTime;
                                }
                            }
                            else
                            {
                                state = FiniteStateMachine.EndTurn;
                                NextEventAt = ElapsedTime;
                            }
                        }
                        else
                        {
                            // wait for shot to end
                        }
                    }
                    break;
                case FiniteStateMachine.EndTurn:
                    {
                        base.Update(ElapsedTime);
                    }
                    break;
                default:
                    break;
            }
        }

        int upcount = 0;
        int rightcount = 0;
        int targetUpcount = 0;
        int targetRightcount = 0;
        bool launcher = false;
        protected override FiniteStateMachine SelectWeapon(float ElapsedTime, int target)
        {
            if (ElapsedTime >= NextEventAt)
            {
                if (upcount == 0 && targetUpcount == 0 && targetRightcount == 0 && !launcher)
                {
                    ((FakeButtonWrapper)gamepad.DPadUp).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.DPadRight).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.X).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.A).ClearRegisterDown();
                    gamepad.SetState(Buttons.X, InputWrapper.ButtonState.Held);
                    targetUpcount = target % 2;
                    targetRightcount = target / 2;

                    if (target == 0)
                        launcher = true;
                }
                else if ((upcount == targetUpcount && rightcount == targetRightcount) || launcher)
                {
                    launcher = false;
                    upcount = 0;
                    targetUpcount = 0;
                    rightcount = 0;
                    targetRightcount = 0;
                    NextEventAt = ElapsedTime + 0.5f;
                    ((FakeButtonWrapper)gamepad.DPadUp).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.DPadRight).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.X).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.A).ClearRegisterDown();
                    gamepad.SetState(Buttons.A, InputWrapper.ButtonState.Held);

                    return FiniteStateMachine.Deploy;
                }
                else if (upcount != targetUpcount)
                {
                    upcount++;
                    ((FakeButtonWrapper)gamepad.DPadUp).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.DPadRight).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.X).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.A).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.DPadUp).SetHeld();
                    NextEventAt = ElapsedTime + 1.0f;
                }
                else
                {
                    rightcount++;
                    ((FakeButtonWrapper)gamepad.DPadUp).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.DPadRight).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.X).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.A).ClearRegisterDown();
                    ((FakeButtonWrapper)gamepad.DPadRight).SetHeld();
                    NextEventAt = ElapsedTime + 1.0f;
                }

                NextEventAt = ElapsedTime + 0.5f;
            }
            return state;
        }

        void FindTarget()
        {
            getPack = false;

            // Pick which team to attack
            LinkedList<UserTeamPlayer> pms = UTPM.PlayerManagers;
            if (pms.First.Value.Team != myteam)
            {
                target = pms.First.Value;
            }
            else
            {
                target = pms.Last.Value;
            }

            int currentMax = target.TotalHP();

            foreach (UserTeamPlayer utp in pms)
            {
                if (utp.TotalHP() >= currentMax && utp.Team != myteam)
                {
                    target = utp;
                    // Since AI should always be the last player index
                    // AI will attack other AI if all HP are equal
                    // This lets our players "win" more often which players like
                    // to change this make it > currentMax not >= currentMax
                }
            }

            // Pick which avatar to attack -- original: closest -- new: least health
            float health = 1000;

            TargetPosition = new Vector2(0.0f, 0.0f);

            // finds a target with at most 40 health
            foreach (PlayerObject enemy in target.Avatars)
            {
                Vector2 enemyposition = enemy.Position();
                float newhealth = (enemy[ComponentType.Game_Hitpoint] as Game_Hitpoints).Hitpoints;
                float newdistance = (enemyposition - Me.Position()).X;

                if (newhealth <= health && newhealth <= 40)
                {
                    Enemy = enemy;
                    TargetPosition = enemyposition;
                    health = newhealth;
                }
            }

            // if one isn't found, randomly selects a target
            if (TargetPosition.Equals(new Vector2(0.0f,0.0f)))
            {
                int randomTarget = rand.Next(target.AvatarCount);
                
                LinkedList<PlayerObject>.Enumerator enu = target.Avatars.GetEnumerator();
                enu.MoveNext();

                for (int i = 0; i < randomTarget; i++)
                {
                    enu.MoveNext();
                }

                Enemy = enu.Current;
                TargetPosition = Enemy.Position();
            }
       }

        void FindBehindTarget()
        {
            if (UTPM.GetEnemyAvatars().Count != 0)
            {
                Facing myFacing = (Me[ComponentType.Game_Facing] as Game_Facing).Facing;

                List<PlayerObject> sl = (new SortedList<PlayerObject>(UTPM.GetEnemyAvatars(), SmarterAI.GetDistance, SortedList<PlayerObject>.Less)).GetList();
                List<PlayerObject>.Enumerator em = sl.GetEnumerator();
                em.MoveNext(); // move to first node

                float health = 1000;

                for (int i = 0; i < sl.Count; i++)
                {
                    Vector2 enemyposition = em.Current.Position();

                    // if the enemy is behind me, then mark it as a target
                    if (((Me.Position().X - enemyposition.X) > 0 && myFacing.Equals(Facing.Right)) || ((Me.Position().X - enemyposition.X) < 0 && myFacing.Equals(Facing.Left)))
                    {
                        float newhealth = (em.Current[ComponentType.Game_Hitpoint] as Game_Hitpoints).Hitpoints;

                        if (Math.Abs(newhealth) < Math.Abs(health))
                        {
                            Enemy = em.Current;
                            health = newhealth;
                            TargetPosition = enemyposition;
                        }
                    }
                }
            }
        }

        public static float GetDistance(PlayerObject po)
        {
            return po.Position().X;
        }
        */
    }
}
