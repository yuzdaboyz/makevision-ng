using PhotoSauce.MagicScaler;

var ROGTAG = s2b("0021FE06524F47473031003B");

Console.WriteLine("Preparing ROG Vision GIF...");

static byte[] ResizeImageBytes(byte[] imageBytes, int newWidth, int newHeight)
{
    using (MemoryStream outStream = new())
    {
        ProcessImageSettings processImageSettings = new()
        {
            Width = newWidth,
            Height = newHeight,
            ResizeMode = CropScaleMode.Stretch,
            HybridMode = HybridScaleMode.FavorQuality
        };
        MagicImageProcessor.ProcessImage(imageBytes, outStream, processImageSettings);
        return outStream.ToArray();
    }
}
 static void MakeROGVision(string filename, byte[] bhexData)
{
    var fs = File.ReadAllBytes(filename);   
    using (var fileStream = new FileStream("~v-" + filename, FileMode.Append, FileAccess.Write, FileShare.None))
    using (var bw = new BinaryWriter(fileStream)) { bw.Write(ResizeImageBytes(fs, 256, 64)); bw.Write(bhexData); }
}
static byte[] s2b(string hex)
{
    int len = hex.Length / 2;
    byte[] result = new byte[len];
    for (int i = 0; i < len; i++)
    {
        result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
    }
    return result;
}
static bool isDone(string filename, byte[] hexData)
{
    var fs = File.ReadAllBytes(filename);

    byte[] last12 = new byte[12];
    Array.Copy(fs, fs.Length - 12, last12, 0, 12);

    if (!bcmp(last12, hexData)) { return false; } return true; 
}

static bool bcmp(byte[] a1, byte[] a2)
{
    if (a1 == null || a2 == null || a1.Length != a2.Length)
        return false;
    return a1.SequenceEqual(a2);
}

if (!isDone(args[0], ROGTAG))
{   
    Console.WriteLine("GIF is untagged...");
    MakeROGVision(args[0], ROGTAG);
} else {
    Console.WriteLine("GIF is tagged...");
};

