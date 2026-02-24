using System.Linq;

namespace Consensus {
    public class AllInFavourConsensus : Consensus {
        public override bool ConsensusRule() => scriptVotePairs.All(b => b.vote);
    }
}