using Shared.Data.Common;

namespace Shared.Data.Enums;

public abstract class FormatType : Enumeration<FormatType>
{
    public static readonly FormatType Hardcover = new HardcoverFormatType(); 
    public static readonly FormatType Paperback = new PaperbackFormatType();
    public static readonly FormatType EBook = new EBookFormatType();
    public static readonly FormatType Audiobook = new AudiobookFormatType();
    
    private FormatType(int value, string name) : base(value, name)
    {
        
    }
    
    private sealed class HardcoverFormatType() : FormatType(1, "Hardcover");
    private sealed class PaperbackFormatType() : FormatType(2, "Paperback");
    private sealed class EBookFormatType() : FormatType(3, "EBook");
    private sealed class AudiobookFormatType() : FormatType(4, "Audiobook");
}