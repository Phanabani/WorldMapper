using System;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HarmonyLib;
using SharpGL.WPF;

namespace WorldMapper
{
    /// <summary>
    /// A class which patches in transparency support to OpenGLControl objects.
    /// This is done by changing the bitmap pixel format from 24-bit RGB to
    /// 32-bit BGRA. The FBO (frame buffer object) writes its data to this bitmap
    /// which is displayed in the window.
    /// </summary>
    public class OpenGLTransparencyPatcher
    {
        public static void DoPatching()
        {
            var harmony = new Harmony("WorldMapper.OpenGLOverlayPatch");

            var flags = BindingFlags.NonPublic | BindingFlags.Static;
            var mOriginal = typeof(OpenGLControl).GetMethod("GetFormatedBitmapSource", flags);
            var mPrefix = typeof(OpenGLTransparencyPatcher).GetMethod("Prefix", flags);

            harmony.Patch(mOriginal, new HarmonyMethod(mPrefix));
        }

        private static bool Prefix(IntPtr hBitmap, ref FormatConvertedBitmap __result)
        {
            var newFormatedBitmapSource = new FormatConvertedBitmap();
            newFormatedBitmapSource.BeginInit();
            newFormatedBitmapSource.Source = BitmapConversion.HBitmapToBitmapSource(hBitmap);
            // Marvel at the magic line which adds transparency......
            newFormatedBitmapSource.DestinationFormat = PixelFormats.Bgra32;
            newFormatedBitmapSource.EndInit();

            __result = newFormatedBitmapSource;
            return false;
        }
    }
}
