﻿using System;
using Accord.Statistics.Distributions.Univariate;
using Framework.Observation;

namespace Framework.Belief_State
{
    public class BeliefState : IBeliefState
    {
        private NormalDistribution _foveaPeripheryOperatingCharacteristic;
        public double[] State { get; private set; }

        public void Initialise()
        {
            State = new double[60];
            for (var i = 0; i < State.Length; i++)
            {
                State[i] = 1d / 60d;
            };
            _foveaPeripheryOperatingCharacteristic = NormalDistribution.Standard;
        }

        public double[] CalculateState(double[] activation, int fixation)
        {
            for (var i = 0; i < activation.Length; i++)
            {
                var discriminability =
                    new ObservationGenerationModel(_foveaPeripheryOperatingCharacteristic, fixation, i)
                        .GenerateDiscriminabilityValue();
                State[i] = State[i]*Math.Exp(activation[i]*discriminability);
            }
            return State;
        }
    }
}