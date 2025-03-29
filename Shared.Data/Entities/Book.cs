using Shared.Data.Common;

namespace Shared.Data.Entities;

public class Book
{
    public BookId Id { get; set; }
    public string Title { get; set; }

    public AuthorId AuthorId { get; set; }
    public Author Author { get; set; } 
}