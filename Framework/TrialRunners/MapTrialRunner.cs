using System;
using System.Linq;
using Framework.Actors;
using Framework.Observation;

namespace Framework.TrialRunners
{
    public class MapTrialRunner : ITrialRunner, IMapTrialRunner 
    {
        private readonly Func<IActor> _actorProvider;
        private readonly IMapObservableModel _observableModel;
        private double[] _state;

        public MapTrialRunner(IMapObservableModel observableModel, Func<IActor> actorProvider)
        {
            _observableModel = observableModel;
            _actorProvider = actorProvider;
        }

        public void Run()
        {
            _observableModel.Generate();
            var intelligentActor = _actorProvider();
            var fixationLocation = intelligentActor.Fixate();
            _state = _observableModel.GetState(fixationLocation);

            while (_state.Any(s => s >= 0.9) == false)
            {
                _state = _observableModel.GetState(fixationLocation);
                fixationLocation = GetMaxBelief(_state);
                _state = _observableModel.GetState(fixationLocation);
            }
        }

        public int GetMaxBelief(double[] _state)
        {
            var indexOfMax = 0;
            for (var i = 0; i < _state.Length - 1; i++)
            {
                if (_state[i] > _state[++i])
                {
                    indexOfMax = i;
                }
            }
            return indexOfMax;
        }
    }
}