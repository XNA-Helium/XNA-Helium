using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Core
{
    public class SafeObjectRef 
    {
        BaseObject Object 
        {
            get
            {
                return GameState.GameStateSystem.Instance.GetCurrentState[myID];
            }
        }

        protected float myID;

        public SafeObjectRef(BaseObject ID)
        {
            myID = ID.ID;
        }

        public SafeObjectRef(float ID)
        {
            myID = ID;
        }

        public void Save(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryWriter bw = sw.BinaryWriter;
            bw.Write( myID );
            bw.Flush();
        }

        public void Load(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryReader br = sw.BinaryReader;
            myID = br.ReadInt32();
        }
    }
}
