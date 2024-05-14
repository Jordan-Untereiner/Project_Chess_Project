using Chess;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private Mesh _meshFilter2;
    [SerializeField] private Material _material;
    [SerializeField] private Material[] _piecesMaterial;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) { SetPiece(1); }
        if(Input.GetKeyDown(KeyCode.X)) {
            _meshFilter.mesh = null;
        }
    }
    public void SetPiece(int piece)
    {
        //_meshFilter = _meshFilter2;
        GetComponent<MeshFilter>().mesh = _meshFilter2;
        //_material = _piecesMaterial[piece];

    }
}
