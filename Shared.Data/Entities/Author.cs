using Shared.Data.Common;

namespace Shared.Data.Entities;

public class Author
{
    public AuthorId Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<Book> Books { get; set; }
}