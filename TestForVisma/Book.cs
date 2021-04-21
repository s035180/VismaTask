using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForVisma
{
    class Book
    {
        public string name { get; set; }
        public string author { get; set; }
        public string category { get; set; }
        public string language { get; set; }
        public DateTime pubDate { get; set; }
        public string ISBN { get; set; }

        private List<Book> bookList;
        private string jsonData;
        private string filePath;
        Person person = new Person();
        


        public void loadBooks()
        {
            filePath = "Book.json";
            if(!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            jsonData = System.IO.File.ReadAllText(filePath);
            bookList = JsonConvert.DeserializeObject<List<Book>>(jsonData) ?? new List<Book>();
            person.loadPersons();

        }
        public void addBook(string _name, string _author, string _category, string _ISBN,
            string _language, DateTime _date)
        {

                bookList.Add(new Book()
                {
                    name = _name,
                    author = _author,
                    category = _category,
                    ISBN = _ISBN,
                    language = _language,
                    pubDate = _date
                });
            

            Write();
        }
        public void takeBook(int _bookPosition, string _personName)
        {
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            bool shouldDleteBook = person.checkPerson(_personName, bookList[_bookPosition], date);
            if (shouldDleteBook == true)
            {
                deleteBook(_bookPosition);
            }
        }
        public void deleteBook(int _bookPosition)
        {
            bookList.RemoveAt(_bookPosition);
            refreshBooks();
            //probably don't need that
            //loadBooks();
        }
        public void refreshBooks()
        {
            using (var fs = new FileStream(filePath, FileMode.Truncate))
            {
            }
            Write();
        }
        public void Write()
        {
            jsonData = JsonConvert.SerializeObject(bookList, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(filePath, jsonData);
        }
        public void returnBook(int _bookPosition, string _personName)
        {
            person.removeBook(_bookPosition, _personName);
        }
        public void showPersonBook(string name)
        {
            List<Book> myBookList = new List<Book>();
            Person exactPerson = new Person();
            exactPerson = person.showPerson(name);
            myBookList = exactPerson.takenBooks;
            showBooks(myBookList, true);
        }
        public void showBooks(List<Book> _bookList, bool personBooks)
        {
            int index = 0;
            foreach (Book book in _bookList)
            {
                if (!personBooks)
                    index = bookList.IndexOf(book);
                else
                    index++;
                Console.WriteLine("ID of the book: " + index);
                Console.WriteLine("Name: " + book.name);
                Console.WriteLine("Author: " + book.author);
                Console.WriteLine("Category: " + book.category);
                Console.WriteLine("Language: " + book.language);
                Console.WriteLine("ISBN: " + book.ISBN);
                Console.WriteLine("pubDate: " + book.pubDate);
                Console.WriteLine("-----------------------");

            }
        }
        public void filtering(string value, bool ifDate, int position)
        {
            List<Book> filteredBookList = new List<Book>();
            DateTime date = new DateTime();
            if(ifDate)
            {
                date = Convert.ToDateTime(value);
            }
            if (position == 1) {
                 filteredBookList = bookList.Where(o => o.name == value).ToList();
            } else if (position == 2) {
                filteredBookList = bookList.Where(o => o.author == value).ToList();
            } else if (position == 3) {
                filteredBookList = bookList.Where(o => o.category == value).ToList();
            } else if (position == 4) {
                filteredBookList = bookList.Where(o => o.language == value).ToList();
            } else if (position == 5) {
               filteredBookList = bookList.Where(o => o.ISBN == value).ToList();
            } else if (position == 6)
            {
               filteredBookList = bookList.Where(o => o.pubDate == date).ToList();
            }
            showBooks(filteredBookList, false);

        }
        public List<Book> getBookList()
        {
            return bookList;
        }

    }


}
