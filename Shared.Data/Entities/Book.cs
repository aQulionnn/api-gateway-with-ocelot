using Shared.Data.Common;
using Shared.Data.Enums;

namespace Shared.Data.Entities;

public class Book
{
    public BookId Id { get; set; }
    public string Title { get; set; }
    public int FormatValue  { get; private set; }
    public FormatType Format
    {
        get => FormatType.FromValue(FormatValue) ?? throw new InvalidOperationException("Invalid format");
        set => FormatValue = value.Value;
    }

    public AuthorId AuthorId { get; set; }
    public Author Author { get; set; } 
}