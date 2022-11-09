using System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

public class Drawer : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        var d = new LineRendererDrawer();
        d.Create();

        var tt = new DrawPositionsProvider(d, Camera.main, 5);
        tt.Start(Observable.EveryFixedUpdate());

        var y = tt.DrawEndObservable.Subscribe(_ =>
        {
            new LineMesh(d.GetMesh(Camera.main), d.GetDrawPoints()).Create(d.Material, 0.1f);
            d.Dispose();
        }); 
    }
}

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

    public void Create(Material material, float colliderSize)
    {
        _lineMesh = new GameObject("LineMesh");

        _lineMesh.AddComponent<MeshFilter>().mesh = _mesh;
        _lineMesh.AddComponent<MeshRenderer>().material = material;
        
        CreateColliders(_lineMesh.transform, colliderSize);

        _lineMesh.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX 
                                                          | RigidbodyConstraints.FreezeRotationY 
                                                          | RigidbodyConstraints.FreezePositionZ;
    }

    private void CreateColliders(Transform parent, float colliderSize)
    {
        var colliders = new GameObject("Colliders")
        {
            transform =
            {
                parent = parent
            }
        };
        
        var previousColliderPosition = Vector3.zero;

        foreach (var drawPoint in _drawPoints)
        {
            if ((previousColliderPosition - drawPoint).magnitude >= colliderSize)
            {
                var collider = colliders.AddComponent<BoxCollider>();
                collider.center = parent.InverseTransformPoint(drawPoint);
                collider.size = Vector3.one * colliderSize;
                previousColliderPosition = drawPoint;
            }
        }
    }
}

public class DrawPositionsProvider : IDisposable
{
    private readonly IDataHandler<Vector3> _drawer;
    private readonly Camera _camera;
    private readonly Vector3 _projectionDistance;
    
    private IDisposable _disposable;
    private Vector3 _previousFramePosition;

    public DrawPositionsProvider(IDataHandler<Vector3> drawer, Camera camera, float projectionDistance)
    {
        _drawer = drawer;
        _camera = camera;
        _projectionDistance = new Vector3(0, 0, projectionDistance);
    }
    
    public IObservable<long> DrawEndObservable { get; private set; }

    public IDisposable Start(IObservable<long> updateStream)
    {
        _disposable = updateStream
                .Where(_ => Input.GetMouseButton(0))
                .Subscribe(_ => ProvidePoint());

        DrawEndObservable = updateStream
            .Where(_ => Input.GetMouseButtonUp(0));

        return this;
    }

    private void ProvidePoint()
    {
        if((Input.mousePosition - _previousFramePosition).sqrMagnitude > 0.01f)
            _drawer.Handle(_camera.ScreenToWorldPoint(Input.mousePosition + _projectionDistance));
        
        _previousFramePosition = Input.mousePosition + _projectionDistance;
    }

    public void Dispose()
    {
        _disposable?.Dispose();
    }
}

public class LineRendererDrawer : IDataHandler<Vector3>, IDisposable
{
    private LineRenderer _drawer;
    private int _lastDrawnPointIndex;
    public Material Material { get; private set; }

    public void Create()
    {
        _drawer = new GameObject("LineRendererDrawer").AddComponent<LineRenderer>();
        Material = new Material(Shader.Find("Unlit/Color"))
        {
            color = Color.red
        };

        _drawer.startColor = Color.red;
        _drawer.endColor = Color.red;
        _drawer.material = Material;
        _drawer.numCapVertices = 90;
        _drawer.positionCount = 0;
        _drawer.widthMultiplier = 0.1f;
    }
    
    public void Handle(Vector3 data)
    {
        if (_drawer.positionCount <= _lastDrawnPointIndex)
            _drawer.positionCount += 1;
        
        _drawer.SetPosition(_lastDrawnPointIndex, data);
        _lastDrawnPointIndex++;
    }

    public Mesh GetMesh(Camera camera)
    {
        var mesh = new Mesh();
        _drawer.BakeMesh(mesh, camera, true);
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

    public void Dispose()
    {
        Object.Destroy(_drawer.gameObject);
    }
}

public interface IDataHandler<in T>
{
    public void Handle(T data);
}
