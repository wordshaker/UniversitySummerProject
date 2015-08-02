﻿using System;
using Accord.Statistics.Distributions.Univariate;
using Framework.Data;
using Framework.Observation;

namespace Framework.Belief_State
{
    public class BeliefStateForMapAnalysis : IBeliefStateForMap
    {
        private readonly IBubbleDataRecorder _beliefStateDataRecorder;
        private NormalDistribution _foveaPeripheryOperatingCharacteristic;
        private int _numberOfFixation;
        public double[] State { get; private set; }
        
        public BeliefStateForMapAnalysis(IBubbleDataRecorder beliefStateDataRecorder)
        {
            _beliefStateDataRecorder = beliefStateDataRecorder;
            _numberOfFixation = 0;
        }
       
        public void Initialise()
        {
            const double oneSeventh = 1d/7d;
            State = new[] {oneSeventh, oneSeventh, oneSeventh, oneSeventh, oneSeventh, oneSeventh, oneSeventh};
            _foveaPeripheryOperatingCharacteristic = NormalDistribution.Standard;
        }

        public double[] CalculateState(double[] activation, int fixation)
        {
            ++_numberOfFixation;
            for (var i = 0; i < 7; i++)
            {
                var discriminability =
                    new ObservationGenerationModel(_foveaPeripheryOperatingCharacteristic, fixation, i)
                        .GenerateDiscriminabilityValue();
                State[i] = State[i]*Math.Exp(activation[i]*discriminability);
            }
            _beliefStateDataRecorder.Insert(_numberOfFixation, State);
            return State;
        }
    }
}