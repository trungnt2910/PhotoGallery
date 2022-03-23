namespace PhotoGallery
{
    public static class Helpers
    {
        public static string Truncate(this string original, int maxLength)
        {
            if (string.IsNullOrEmpty(original))
            {
                return string.Empty;
            }

            if (original.Length <= maxLength)
            {
                return original;
            }
            return original.Substring(0, maxLength - 3) + "...";
        }

        public static string ToFileSize(this long value)
        {
            // Get absolute value
            long value_abs = (value < 0 ? -value : value);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (value_abs >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (value >> 50);
            }
            else if (value_abs >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (value >> 40);
            }
            else if (value_abs >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (value >> 30);
            }
            else if (value_abs >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (value >> 20);
            }
            else if (value_abs >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (value >> 10);
            }
            else if (value_abs >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = value;
            }
            else
            {
                return value.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.### ") + suffix;
        }
    }
}
