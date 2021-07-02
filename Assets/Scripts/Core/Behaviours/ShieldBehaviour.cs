using System.Collections.Generic;
using UnityEngine;

namespace Core.Behaviours
{
    public class ShieldBehaviour : MonoBehaviour
    {
        [Header("Runtime Data")]
        public int Tolerance;

        private IEnumerable<ShieldPieceBehaviour> _shieldPieces;

        public void Initialize()
        {
            _shieldPieces = GetComponentsInChildren<ShieldPieceBehaviour>();
            foreach (var shieldPiece in _shieldPieces)
            {
                shieldPiece.Initialize(Tolerance);
            }
        }
        
    }
}
