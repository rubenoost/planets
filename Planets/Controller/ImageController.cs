using System.Drawing;

namespace Planets.Controller
{
    class ImageController
    {
        /*/// <summary>
        /// Converts an SVG file to a Bitmap image.
        /// </summary>
        /// <param name="filePath">The full path of the SVG image.</param>
        /// <returns>Returns the converted Bitmap image.</returns>
        public static Bitmap GetBitmapFromSVG(string filePath, int width, int height)
        {
            SvgDocument document = GetSvgDocument(filePath, width, height);
            Bitmap bmp = document.Draw();
            return bmp;
        }

        /// <summary>
        /// Gets a SvgDocument for manipulation using the path provided.
        /// </summary>
        /// <param name="filePath">The path of the Bitmap image.</param>
        /// <returns>Returns the SVG Document.</returns>
        public static SvgDocument GetSvgDocument(string filePath, int width, int height)
        {
            SvgDocument returnDocument = SvgDocument.Open(filePath);
            returnDocument = AdjustSize(returnDocument, width, height);
            return returnDocument;
        }

        /// <summary>
        /// Adjusts the document to the specifeid width and height.
        /// </summary>
        /// <param name="document">The SVG document to resize.</param>
        /// <param name="width">Width to resize to</param>
        /// <returns>Returns a resized version of the original document.</returns>
        private static SvgDocument AdjustSize(SvgDocument document, int width, int height)
        {
            document.Width = width;
            document.Height = height;
            return document;
        }*/
    }
}