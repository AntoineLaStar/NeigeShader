using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrack : MonoBehaviour {
    public Shader _drawShader;
    public GameObject _terrain;
    public Transform[] _wheel;
    RaycastHit _gorundHit;
    int _layerMask;
    [Range(0, 4)]
    public float brushSize;
    [Range(0,1)]
    public float brushStrength;


    private RenderTexture _splatMap;
    private Material _snowMaterial;
    private Material _drawMaterial;


    void Start () {
        _layerMask = LayerMask.GetMask("Ground");
        _drawMaterial = new Material(_drawShader);
        _snowMaterial = _terrain.GetComponent<MeshRenderer>().material;
        _splatMap = new RenderTexture(1024,1024,0,RenderTextureFormat.ARGBFloat);
        _snowMaterial.SetTexture("_Splat", _splatMap);
	}
	
	void Update () {
		foreach(Transform wheel in _wheel)
        {
            if (Physics.Raycast(wheel.position, Vector3.down, out _gorundHit, 1f, _layerMask))
            {
                _drawMaterial.SetVector("_Coordinate", new Vector4(_gorundHit.textureCoord.x, _gorundHit.textureCoord.y,0,0));
                _drawMaterial.SetFloat("_Strength", brushStrength);
                _drawMaterial.SetFloat("_Size", brushSize);
                RenderTexture temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(_splatMap, temp);
                Graphics.Blit(temp,_splatMap,_drawMaterial);
                RenderTexture.ReleaseTemporary(temp);

            }
        }
	}
}
