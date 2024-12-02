using System;

namespace KamerConnect.View.MAUI.Utils;

public class MauiFileUtils
{
    public static readonly Dictionary<DevicePlatform, IEnumerable<string>> ImageFileTypes = new()
            {
                { DevicePlatform.Android, new[] { "image/*" } },
                { DevicePlatform.iOS, new[] { "public.image" } },
                { DevicePlatform.MacCatalyst, new[] { "public.image" } },
                { DevicePlatform.WinUI, new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" } }
            };

}
