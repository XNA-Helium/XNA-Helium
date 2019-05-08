using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Core
{
    public sealed class TeamMember : BaseComponent
    {
        public override void Save(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryWriter bw = sw.BinaryWriter;
            bw.Write( team );
            bw.Flush();
        }
        public override void Load(Engine.Persistence.PersistenceManager sw)
        {
            System.IO.BinaryReader br = sw.BinaryReader;
            team = br.ReadString();
        }

        public static Object ReturnsNew( bool InWorld, float uID )
        {
            return new TeamMember(InWorld, uID);
        }
        public static new ReturnsNew ReturnNew
        {
            get
            {
                return ReturnsNew;
            }
        }

        public static Type TypeStatic = typeof( TeamMember );
        public static string NotSet = "NotSet";
        protected string team = NotSet;
        public String Team
        {
            set
            {
                HasChanged = true;
                team = value;
            }
            get
            {
                return team;
            }
        }
        public TeamMember( String Team )
            : base(true, -1)
        {
            UniqueClassID = TypeStatic;
            team = Team;
        }
        public TeamMember(bool InWorld, float uID )
            : base(InWorld, uID)
        {
            UniqueClassID = TypeStatic;
        }

        public override void OnParentSet( BaseObject Parent )
        {
            DebugHelper.Break( team == NotSet, DebugHelper.DebugLevels.HighlyCritical );
            TeamManager.Instance.AddToTeam( team, Parent );
        } 
    }
}
