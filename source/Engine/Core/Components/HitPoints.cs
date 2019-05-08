using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Core
{
    public sealed class HitPoints : BaseComponent
    {

        public override void Save(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryWriter bw = sw.BinaryWriter;
            bw.Write( hp );
            bw.Flush();
        }
        public override void Load(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryReader br = sw.BinaryReader;
            hp = br.ReadInt32();
        }

        public static Object ReturnsNew( bool InWorld, float uID )
        {
            return new HitPoints(InWorld,uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }
        public static Type TypeStatic = typeof( HitPoints ); 

        private SetterValidatorDelegate<int,HitPoints> HitpointsValidator = null;
        private SafeList<SetterCallback<int, HitPoints>> HitpointsSetCallbacks = new SafeList<SetterCallback<int, HitPoints>>(1);
        public void InstallHitpointsSetCallback(SetterCallback<int, HitPoints> Callback)
        {
            HitpointsSetCallbacks.Add( Callback );
        }
        public void RemoveHitpointsSetCallback(SetterCallback<int, HitPoints> Callback)
        {
            HitpointsSetCallbacks.SafeRemove( Callback );
        }
        protected int hp = 0;
        public HitPoints( int HitPoints ):base(true, -1)
        {
            hp = HitPoints;
            UniqueClassID = TypeStatic;
        }
        public HitPoints( bool InWorld,float uID ):base(InWorld,uID)
        {
            UniqueClassID = TypeStatic;
        }

        public int Hitpoints
        {
            get
            {
                return hp;
            }
            set
            {
                if ( HitpointsValidator != null )
                {
                    int RequiredOffset;
                    if ( HitpointsValidator.Invoke( ref value, out RequiredOffset, null ) )
                    {
                        value += RequiredOffset;
                    }
                }
                {
                    HasChanged = true;
                    hp = value;
                }
                foreach(SetterCallback<int,HitPoints> hpsc in HitpointsSetCallbacks)
                {
                    hpsc.Invoke( ref hp, this );
                }
                HitpointsSetCallbacks.RemoveAll();
            }
        }
    }
}
