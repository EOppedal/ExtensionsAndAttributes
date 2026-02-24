using System.Linq;

namespace Consensus {
    public class MajorityInFavourConsensus : Consensus {
        public override bool ConsensusRule() => scriptVotePairs.Select(scriptVotePair => scriptVotePair.vote).Count() >
                                                scriptVotePairs.Count / 2;
    }
}