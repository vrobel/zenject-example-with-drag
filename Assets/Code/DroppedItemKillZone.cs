using System.Collections.Generic;
using Zenject;

namespace Code
{
    public class DroppedItemKillZone: ITickable
    {
        private readonly SceneModel _sceneModel;
        private readonly float _yKillZone;
        private readonly List<SceneItem> _toKill = new List<SceneItem>();

        public DroppedItemKillZone(SceneModel sceneModel, float yKillZone)
        {
            _sceneModel = sceneModel;
            _yKillZone = yKillZone;
        }

        public void Tick()
        {
            _toKill.Clear();
            foreach (var sceneItem in _sceneModel.SceneItemsReadonly)
            {
                if (sceneItem.transform.position.y < _yKillZone)
                {
                    _toKill.Add(sceneItem);
                }
            }

            if (_toKill.Count > 0)
            {
                foreach (var sceneItem in _toKill)
                {
                    _sceneModel.DestroySceneItem(sceneItem);
                }
                _toKill.Clear();
            }
        }
    }
}