using System;
using System.Collections.Generic;
using UnityEngine;

namespace Consensus {
    /// <summary>
    /// AddVote gets called by change vote if not already added manually
    /// </summary>
    [Serializable]
    public abstract class Consensus {
        [SerializeField] protected List<ScriptVotePair> scriptVotePairs = new();
        public bool consensus = true;

        public event Action<bool> OnVoteChange = delegate { };

        public void AddVote(ScriptVotePair scriptVotePair) {
            scriptVotePairs.Add(scriptVotePair);
            consensus = ConsensusRule();
            OnVoteChange.Invoke(consensus);
        }

        public void ChangeVote(ScriptVotePair scriptVotePair, bool vote) {
            scriptVotePair.vote = vote;

            if (!scriptVotePairs.Contains(scriptVotePair)) {
                AddVote(scriptVotePair);
                return;
            }

            consensus = ConsensusRule();
            OnVoteChange.Invoke(consensus);
        }

        public void DeleteVote(ScriptVotePair scriptVote) => scriptVotePairs.Remove(scriptVote);

        public abstract bool ConsensusRule();
    }
}