﻿using System;
using System.Collections.Generic;
using Framework.Actors;
using Framework.Data;
using Framework.Observation;

namespace Framework.TrialRunners
{
    public class RandomBeliefTrialRunner : ITrialRunner
    {
        private readonly Func<IActor> _actorProvider;
        private readonly IObservableModel _observableModel;
        private readonly IDataRecorder _recorder;

        public RandomBeliefTrialRunner(IObservableModel observableModel, Func<IActor> actorProvider,
            IDataRecorder recorder)
        {
            _observableModel = observableModel;
            _actorProvider = actorProvider;
            _recorder = recorder;
        }

        public void Run()
        {
            _observableModel.Generate();
            var actor = _actorProvider();
            var fixations = 0;
            int fixationLocation;
            List<int> visited = new List<int>();
            
            do
            {
                fixationLocation = actor.Fixate();
                if (visited.Contains(fixationLocation)) continue;
                fixations++;
                visited.Add(fixationLocation);
            } while ( _observableModel.Update(fixationLocation) == false);

            _recorder.Insert(fixations);
        }
    }
}