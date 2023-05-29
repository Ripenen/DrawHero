using Data;
using UI;
using UnityEngine;

namespace Drawing
{
    public class LineRendererDrawer
    {
        private readonly DrawerConfig _config;
    
        private LineRenderer _drawer;
        private ParticleSystem _trail;

        public float Width => _config.LineWidth;
        public Material Material => _drawer.material;

        public LineRendererDrawer(DrawerConfig config)
        {
            _config = config;
        }

        public void Create()
        {
            _drawer = new GameObject("LineRendererDrawer").AddComponent<LineRenderer>();

            _drawer.startColor = _config.Color;
            _drawer.endColor = _config.Color;
            _drawer.material = _config.Material;
            _drawer.numCapVertices = 5;
            _drawer.positionCount = 0;
            _drawer.widthMultiplier = _config.LineWidth;
        }
    
        public void Draw(Vector3 data)
        {
            _drawer.positionCount += 1;
        
            _drawer.SetPosition(_drawer.positionCount - 1, data);
            
            if(_trail)
            {
                _trail.transform.position = data;
                
                if(!_trail.isPlaying)
                    _trail.Play();
            }
        }

        public Mesh GetMesh()
        {
            var mesh = new Mesh();
        
            _drawer.BakeMesh(mesh);
        
            mesh.Optimize();
            mesh.OptimizeIndexBuffers();
            mesh.OptimizeReorderVertexBuffer();
        
            return mesh;
        }

        public Vector3[] GetDrawPoints()
        {
            var points = new Vector3[_drawer.positionCount];

            _drawer.GetPositions(points);

            return points;
        }

        public void Clear()
        {
            _drawer.positionCount = 0;
        }

        public void SetDrawAsset(DrawAsset drawAsset)
        {
            _drawer.material = drawAsset.Material;

            if(_trail)
                Object.Destroy(_trail.gameObject);
            
            if(drawAsset.Trail)
            {
                _trail = Object.Instantiate(drawAsset.Trail, _drawer.transform);
                _trail.Stop();
            }
        }
    }
}