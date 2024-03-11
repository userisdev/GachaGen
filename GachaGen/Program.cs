using SkiaSharp;

string? srcDirPath = args.FirstOrDefault();
string? dstDirPath = args.ElementAtOrDefault(1);
string code = args.ElementAtOrDefault(2) ?? "#2B2D31";

if (!Directory.Exists(srcDirPath))
{
    Console.WriteLine($"not found. [{srcDirPath}]");
    Environment.Exit(1);
}

if (string.IsNullOrWhiteSpace(dstDirPath))
{
    Console.WriteLine($"invalid args. [{dstDirPath}]");
    Environment.Exit(1);
}

DirectoryInfo srcDir = new(srcDirPath);
DirectoryInfo dstDir = Directory.CreateDirectory(dstDirPath);

FileInfo[] srcFiles = srcDir.GetFiles("*", SearchOption.TopDirectoryOnly);
foreach (FileInfo srcFile in srcFiles)
{
    try
    {
        string savePath = Path.Combine(dstDir.FullName, $"{Path.GetFileNameWithoutExtension(srcFile.Name)}.jpg");
        byte[] data = File.ReadAllBytes(srcFile.FullName);
        using SKBitmap bmp = SKBitmap.Decode(data);
        using SKSurface surface = SKSurface.Create(new SKImageInfo(bmp.Width, bmp.Height));
        using SKCanvas canvas = surface.Canvas;

        canvas.Clear(SKColor.Parse(code));
        canvas.DrawBitmap(bmp, 0, 0);

        using SKImage img = surface.Snapshot();
        using SKData tmp = img.Encode(SKEncodedImageFormat.Jpeg, 90);

        using FileStream stream = new(savePath, FileMode.CreateNew);
        tmp.SaveTo(stream);
        Console.WriteLine($"saved. [{savePath}]");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"exception:{ex.Message}");
    }
}

Environment.Exit(0);
