using UnityEngine;

namespace CardGameTemplate
{
    public class ICardGameMatchData
    {
        protected RuntimePlayerProfile[] _runtimePlayerProfiles;
        public virtual RuntimePlayerProfile[] RuntimePlayerProfiles => _runtimePlayerProfiles;


        public ICardGameMatchData(RuntimePlayerProfile[] runtimePlayerProfiles)
        {
            _runtimePlayerProfiles = runtimePlayerProfiles;
        }
    }
}
