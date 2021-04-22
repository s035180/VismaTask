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
        

        //Loading books from the book.json file
        public void loadBooks()
        {
            filePath = "Book.json";
            if(!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            jsonData = System.IO.File.ReadAllText(filePath);
            bookList = JsonConvert.DeserializeObject<List<Book>>(jsonData) ?? new List<Book>();
            person.loadPersons();

        }
        //Adding book to the book.json file and adding new book to book list
        public void addBook(string _name, string _author, string _category, string _ISBN,
            string _language, DateTime _date)
        {
            try
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


                //Using write function to write new data to the file
                Write();
                Console.WriteLine("Book added to available book list");
            }
            catch
            {
                Console.WriteLine("Encountered problem, check if your input is correct");
            }
        }
        //Function for user to take a book
        public void takeBook(int _bookPosition, string _personName, DateTime date)
        {
            try
            {
                bool shouldDleteBook = person.checkPerson(_personName, bookList[_bookPosition], date);
                if (shouldDleteBook == true)
                {
                    deleteBook(_bookPosition);
                    Console.WriteLine("Book have been taken");
                }
                
            }
            catch
            {
                Console.WriteLine("Encountered problem, book ID doesn't exist");
            }
        }
        //Function to delete a book (can't delete book if it is taken by user)
        public void deleteBook(int _bookPosition)
        {
            try
            {
                bookList.RemoveAt(_bookPosition);
                refreshBooks();
                Console.WriteLine("Book deleted from available book list");
            } catch
            {
                Console.WriteLine("Encountered problem, book ID doesn't exist");
            }
        }
        //Refreshing Book.json file in order to make updates in it (sorry, I know there are much better solutions
        //but I'm kinda stupid)
        public void refreshBooks()
        {
            using (var fs = new FileStream(filePath, FileMode.Truncate))
            {
            }
            Write();
        }
        //Function to write in file (serializing list of books)
        public void Write()
        {
            jsonData = JsonConvert.SerializeObject(bookList, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(filePath, jsonData);
        }
        //Function to return book
        public void returnBook(int _bookPosition, string _personName)
        {
            try
            {
                person.removeBook(_bookPosition, _personName);
                Console.WriteLine("Returned book");
            } catch
            {
                Console.WriteLine("Encountered problem, person or book ID doesn't exist");
            }

        }
        //Function to show specific person books
        public void showPersonBook(string name)
        {
            try
            {
                List<Book> myBookList = new List<Book>();
                Person exactPerson = new Person();
                exactPerson = person.showPerson(name);
                myBookList = exactPerson.takenBooks;
                showBooks(myBookList, true);
            } catch
            {
                Console.WriteLine("Encountered problem, person doesn't exist");
            }
        }
        //Function to display books
        public void showBooks(List<Book> _bookList, bool personBooks)
        {
            try
            {
                int index = 0;
                foreach (Book book in _bookList)
                {
                    if (!personBooks)
                        index = bookList.IndexOf(book);
                    Console.WriteLine("ID of the book: " + index);
                    Console.WriteLine("Name: " + book.name);
                    Console.WriteLine("Author: " + book.author);
                    Console.WriteLine("Category: " + book.category);
                    Console.WriteLine("Language: " + book.language);
                    Console.WriteLine("ISBN: " + book.ISBN);
                    Console.WriteLine("pubDate: " + book.pubDate);
                    Console.WriteLine("-----------------------");
                    if (personBooks)
                        index++;

                }
            } catch
            {
                Console.WriteLine("Encountered problem, files are probably corrupted");
            }
        }
        //Filtering function to help display books
        public void filtering(string value, bool ifDate, int position)
        {
            try
            {
                List<Book> filteredBookList = new List<Book>();
                DateTime date = new DateTime();
                if (ifDate)
                {
                    date = Convert.ToDateTime(value);
                }
                if (position == 1)
                {
                    filteredBookList = bookList.Where(o => o.name == value).ToList();
                }
                else if (position == 2)
                {
                    filteredBookList = bookList.Where(o => o.author == value).ToList();
                }
                else if (position == 3)
                {
                    filteredBookList = bookList.Where(o => o.category == value).ToList();
                }
                else if (position == 4)
                {
                    filteredBookList = bookList.Where(o => o.language == value).ToList();
                }
                else if (position == 5)
                {
                    filteredBookList = bookList.Where(o => o.ISBN == value).ToList();
                }
                else if (position == 6)
                {
                    filteredBookList = bookList.Where(o => o.pubDate == date).ToList();
                }
                showBooks(filteredBookList, false);
            } catch
            {
                Console.WriteLine("Encountered problem, I don't even know how you've managed to break the filter");
            }

        }
        //Since bookList is private and is beeing used in person class, creating getBookList
        public List<Book> getBookList()
        {
            return bookList;
        }
        //Function to help display every person taken books
        public void takenBooks()
        {
            showBooks(person.getPersonsBookList(), true);

        }
        //Function to help display every single book
        public void showAllBooks()
        {
            List<Book> listToShow = new List<Book>();
            listToShow = bookList;
            foreach(Book book in person.getPersonsBookList())
            {
                listToShow.Add(book);
            }
            showBooks(listToShow, true);
        }


    }


}
