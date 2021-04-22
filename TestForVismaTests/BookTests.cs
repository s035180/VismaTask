using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestForVisma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace TestForVisma.Tests
{
    [TestClass()]
    public class BookTests
    {

        [TestMethod()]
        public void TestaddBook()
        {
            Book book = new Book();
            book.loadBooks();
            int bookNum = book.getBookList().Count();
            DateTime date = new DateTime(1999,9,9);
            book.addBook("BookName", "Author", "Category", "191911", "Russian", date);
            Assert.AreEqual(bookNum + 1, book.getBookList().Count());            
        }
        [TestMethod()]
        public void TestaddBookWhenIncorrectDate()
        {
            Book book = new Book();
            book.loadBooks();
            int bookNum = book.getBookList().Count();
            DateTime date = new DateTime(1999, 45, 9);
            try
            {
                book.addBook("BookName", "Author", "Category", "191911", "Russian", date);
            }
            catch
            {

            }
            Assert.AreNotEqual(bookNum + 1, book.getBookList().Count());
        }
        [TestMethod()]
        public void TestTakeBookAfterAddingOneWhenDataIsCorrect()
        {
            Book book = new Book();
            book.loadBooks();
            int bookNum = book.getBookList().Count();
            DateTime date = new DateTime(1999, 9, 9);
            DateTime date2 = new DateTime(2021, 5, 9);
            book.addBook("BookName", "Author", "Category", "191911", "Russian", date);
            book.takeBook(0, "Samurai", date2);
            Assert.AreEqual(bookNum, book.getBookList().Count());

        }
    }
}