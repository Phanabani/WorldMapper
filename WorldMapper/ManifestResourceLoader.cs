using System.IO;
using System.Reflection;

namespace WorldMapper
{
    /// <summary>
    /// A small helper class to load manifest resource files.
    /// Source: https://github.com/dwmkerr/sharpgl/blob/master/source/SharpGL/Samples/WinForms/ModernOpenGLSample/ManifestResourceLoader.cs
    /// </summary>
    public static class ManifestResourceLoader
    {
        /// <summary>
        /// Loads the named manifest resource as a text string.
        /// </summary>
        /// <param name="textFileName">Name of the text file.</param>
        /// <returns>The contents of the manifest resource.</returns>
        public static string LoadTextFile(string textFileName)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var pathToDots = textFileName.Replace("\\", ".");
            var location = $"{executingAssembly.GetName().Name}.{pathToDots}";

            using (var stream = executingAssembly.GetManifestResourceStream(location))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
