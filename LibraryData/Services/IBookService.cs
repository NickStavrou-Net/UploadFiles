using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData.Services
{
    public interface IBookService
    {
        IEnumerable<Book> GetAllBooks();
        IEnumerable<Book> GetByTitle(string title);
        //IEnumerable<Book> GetByAuthor(string author);
        Book GetBook(int id);
        void Add(Book newBook);
    }
}
