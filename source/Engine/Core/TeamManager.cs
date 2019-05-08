using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Core
{
    public class TeamManager
    {
        System.Collections.Generic.Dictionary<String, List<BaseObject> > ht = new Dictionary<string,List<BaseObject>>();

        private static TeamManager instance;
        public static TeamManager Instance
        {
            get
            {
                if ( instance == null )
                {
                    instance = new TeamManager();
                }
                return instance;
            }
        }

        public void AddToTeam(String Team, BaseObject baseObject)
        {
            if(! ht.ContainsKey(Team) )
            {
                ht[Team] = new List<BaseObject>();
            }
            ht[Team].Add(baseObject);
        }

        public List<BaseObject> GetTeamMembers(String Team)
        {
            return ht[Team];
        }
    }
}
