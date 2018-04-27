namespace Projekt
{
    /// <summary>
    ///     Helper functions for <see cref="IconType" />
    /// </summary>
    public static class IconTypeExtensions
    {
        /// <summary>
        ///     Converts <see cref="IconType" /> to a FontAwesome string
        /// </summary>
        /// <param name="type">The type to convert</param>
        /// <returns></returns>
        public static string ToFontAwesome(this IconType type)
        {
            // Return a FontAwesome string based on the icon type
            switch (type)
            {
                case IconType.Save:
                    return "\uf0c7";

                case IconType.Cancel:
                    return "\uf05e";

                case IconType.Drive:
                    return "\uf0a0";

                // If none found, return null
                default:
                    return null;
            }
        }
    }
}