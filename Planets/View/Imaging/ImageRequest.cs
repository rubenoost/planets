namespace Planets.View.Imaging
{
    public struct ImageRequest
    {
        public readonly int No;
        public readonly int W;
        public readonly int H;
        public readonly int R;

        public ImageRequest(int index, int width, int height, int rotation)
        {
            No = index;
            W = width;
            H = height;
            R = rotation;
        }

        public override int GetHashCode()
        {
            int i = 23;
            i = i * 486187739 + No;
            i = i * 486187739 + W;
            i = i * 486187739 + H;
            i = i * 486187739 + R;
            return i;
        }

        public override bool Equals(object o)
        {
            if (!(o is ImageRequest))
                return false;

            var i = (ImageRequest)o;

            return No == i.No && W == i.W && H == i.H && R == i.R;
        }
    }
}