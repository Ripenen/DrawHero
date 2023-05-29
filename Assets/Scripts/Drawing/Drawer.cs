using System;
using System.Collections.Generic;
using Data;
using Leveling;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Drawing
{
    public class Drawer : IDisposable
    {
        private readonly LineRendererDrawer _lineDrawer;
        private readonly DrawPositionsProvider _positionProvider;
    
        private IDisposable _subscribe;
        private AudioSource _drawSound;

        public Drawer(Camera camera, DrawerConfig config)
        {
            _lineDrawer = new LineRendererDrawer(config);
            _positionProvider = new DrawPositionsProvider(camera, config.DrawDistance);
            _lineDrawer.Create();
        }

        public void CreateLineMesh(Level parent)
        {
            var lineMesh = new LineMesh(_lineDrawer.GetMesh(), _lineDrawer.GetDrawPoints());
        
            lineMesh.Create(_lineDrawer.Material, _lineDrawer.Width, parent.transform);
        }

        public void Start(Action onDrawEnd, IEnumerable<Rect> drawZones, DrawAsset drawAsset)
        {
            _positionProvider.SetDrawZone(drawZones);
            _lineDrawer.SetDrawAsset(drawAsset);

            _subscribe = Observable.EveryUpdate().Subscribe(_ =>
            {
                _positionProvider.Tick();
            
                if (Input.GetMouseButton(0))
                {
                    if (_positionProvider.CanProvide)
                    {
                        _lineDrawer.Draw(_positionProvider.Provide());
                        
                        if(!_drawSound)
                            _drawSound = Sound.Sound.PlayDraw();
                    }
                }

                if (Input.GetMouseButtonUp(0) && _positionProvider.Provided)
                {
                    _positionProvider.Reset();
                    onDrawEnd?.Invoke();
                    Object.Destroy(_drawSound);
                }
                
            });
        }

        public void Dispose()
        {
            _subscribe.Dispose();
            Object.Destroy(_drawSound);
        }

        public void Clear()
        {
            _lineDrawer.Clear();
        }
    }
}