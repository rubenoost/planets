namespace Planets.View.Imaging
{
    public struct ImageRequest
    {
        public readonly int no;
        public readonly int w;
        public readonly int h;
        public readonly int r;

        public ImageRequest(int index, int width, int height, int rotation)
        {
            no = index;
            w = width;
            h = height;
            r = rotation;
        }

        public override int GetHashCode()
        {
            int i = 23;
            i = i * 486187739 + no;
            i = i * 486187739 + w;
            i = i * 486187739 + h;
            i = i * 486187739 + r;
            return i;
        }

        public override bool Equals(object o)
        {
            if (!(o is ImageRequest))
                return false;

            var i = (ImageRequest)o;

            return no == i.no && w == i.w && h == i.h && r == i.r;
        }
    }
}