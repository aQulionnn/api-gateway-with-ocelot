using Shared.Data.Entities;

namespace Books.Api.Specifications;

public class AuthorIdSpecification(int authorId) : Specification<Book>(book => Equals(book.AuthorId, authorId));