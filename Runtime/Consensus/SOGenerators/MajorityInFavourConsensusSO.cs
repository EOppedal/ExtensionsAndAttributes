using System.Linq;
using UnityEngine;

namespace Consensus {
    [CreateAssetMenu(
        fileName = nameof(MajorityInFavourConsensusSO),
        menuName = "Scriptable Objects/GlobalData/Consensus/Create " + nameof(MajorityInFavourConsensusSO))]
    public class MajorityInFavourConsensusSO : ConsensusSO {
        public override bool ConsensusRule() => scriptVotePairs.Select(scriptVotePair => scriptVotePair.vote).Count() >
                                                scriptVotePairs.Count / 2;
    }
}