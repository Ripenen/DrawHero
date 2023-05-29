using Leveling.Collisions;
using UnityEngine;

namespace Drawing
{
    public class LineMesh
    {
        private readonly Mesh _mesh;
        private readonly Vector3[] _drawPoints;
    
        private GameObject _lineMesh;

        public LineMesh(Mesh mesh, Vector3[] drawPoints)
        {
            _mesh = mesh;
            _drawPoints = drawPoints;
        }

        public void Create(Material material, float colliderSize, Transform parent)
        {
            if(_drawPoints.Length < 2)
                return;
        
            _lineMesh = new GameObject("LineMesh");
            _lineMesh.transform.parent = parent;

            _lineMesh.AddComponent<MeshFilter>().mesh = _mesh;
            _lineMesh.AddComponent<MeshRenderer>().material = material;
        
            int createdColliders = CreateColliders(_lineMesh.transform, colliderSize);

            var rigidBody = _lineMesh.AddComponent<Rigidbody>();

            rigidBody.mass = createdColliders * 7.5f;
            
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX 
                                    | RigidbodyConstraints.FreezeRotationY 
                                    | RigidbodyConstraints.FreezePositionZ;
            
            rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
            rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            
            _lineMesh.AddComponent<CollisionProvider>();
        }

        private int CreateColliders(Transform parent, float colliderSize)
        {
            var colliders = new GameObject("Colliders");
            colliders.transform.parent = parent;

            var previousColliderPosition = _drawPoints[0];
            int createdColliders = 1;
        
            var tt = colliders.AddComponent<SphereCollider>();
            tt.center = previousColliderPosition + parent.position;
            tt.radius = colliderSize * 0.5f;
            previousColliderPosition = tt.center;
        
            foreach (var drawPoint in _drawPoints)
            {
                var direction = (previousColliderPosition - drawPoint);

                while (direction.magnitude >= 0.3f)
                {
                    var point = Vector3.Lerp(previousColliderPosition, drawPoint, 0.5f);
                
                    var collider = colliders.AddComponent<SphereCollider>();
                    collider.center = point + parent.position;
                    collider.radius = colliderSize * 0.5f;
                    previousColliderPosition = collider.center;

                    createdColliders++;
            
                    direction = (previousColliderPosition - drawPoint);
                }
            }

            return createdColliders;
        }
    }
}