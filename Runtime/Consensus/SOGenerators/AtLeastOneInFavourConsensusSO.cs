using System.Linq;
using UnityEngine;

namespace Consensus {
    [CreateAssetMenu(
        fileName = nameof(AtLeastOneInFavourConsensusSO),
        menuName = "Scriptable Objects/GlobalData/Consensus/Create " + nameof(AtLeastOneInFavourConsensusSO))]
    public class AtLeastOneInFavourConsensusSO : ConsensusSO {
        public override bool ConsensusRule() => scriptVotePairs.Any(scriptVotePair => scriptVotePair.vote);
    }
}