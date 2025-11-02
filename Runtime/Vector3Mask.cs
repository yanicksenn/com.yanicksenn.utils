namespace YanickSenn.Utils
{
    [System.Serializable]
    public struct Vector3Mask {
        public static readonly Vector3Mask None = new(false, false, false);
        public static readonly Vector3Mask All = new(true, true, true);
    
        public static readonly Vector3Mask X = new(true, false, false);
        public static readonly Vector3Mask Y = new(false, true, false);
        public static readonly Vector3Mask Z = new(false, false, true);
        public static readonly Vector3Mask XY = new(true, true, false);
        public static readonly Vector3Mask XZ = new(true, false, true);
        public static readonly Vector3Mask YZ = new(false, true, true);
        
        public bool x;
        public bool y;
        public bool z;

        public Vector3Mask(bool x, bool y, bool z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3Mask Invert() {
            return new Vector3Mask(!x, !y, !z);
        }
    }
}