using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForVisma
{
    class Person
    {
        public string fullName { get; set; }
        public List<DateTime> takeDate { get; set; }
        public List<Book> takenBooks { get; set; }
        private List<Person> personList;
        private string filePath;
        private string jsonData;
        private List<Book> everyTakenBook { get; set; }
        //Reading TakenBooks.json file and adding persons to list
        public void loadPersons()
        {
            filePath = "TakenBooks.json";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            jsonData = System.IO.File.ReadAllText(filePath);
            personList = JsonConvert.DeserializeObject<List<Person>>(jsonData) ?? new List<Person>();
        }
        //adding a new person when he takes a book
        public void addPerson(string _name, Book _book, DateTime _takeDate)
        {

                personList.Add(new Person()
                {
                    fullName = _name,
                    takeDate = new List<DateTime>
                    {
                    _takeDate
                    },
                    takenBooks = new List<Book>
                    {
                        new Book
                        {
                        name = _book.name,
                        author = _book.author,
                        category = _book.category,
                        ISBN = _book.ISBN,
                        language = _book.language,
                        pubDate = _book.pubDate
                        }
                    }
                });


            Write();
        }
        //check if person exists
        public bool checkPerson(string _name, Book _book, DateTime _takeDate)
        {
            bool canTakeBook = true;
            bool personExists = false;
            Person existingPerson = new Person();
            foreach(Person person in personList)
            {
                if(person.fullName == _name)
                {
                    personExists = true;
                    existingPerson = person;
                    break;
                }
            }
            //if person exists checks if he has less than 3 books, if less than 3 books adds a book to his book list
            // if person doesn't exist makes a new person and adds it to a list
            if(personExists)
            {
                if (existingPerson.takenBooks.Count >= 3)
                {
                    canTakeBook = false;
                    Console.WriteLine("Sorry, but you have more than 3 books");
                }
                else
                {
                    existingPerson.takenBooks.Add(_book);
                    existingPerson.takeDate.Add(_takeDate);
                    refreshPersons();
                }
            }
            else
            {
                addPerson(_name, _book, _takeDate);
            }
            // returns bool to book class in order for it to understand if it can delete a book for available book list
            return canTakeBook;
        }
        //Refreshing TakenBooks.json file in order to make updates in it (sorry, I know there are much better solutions
        //but I'm kinda stupid)
        public void refreshPersons()
        {
            using (var fs = new FileStream(filePath, FileMode.Truncate))
            {
            }
            Write();
        }
        //writing function to json file
        public void Write()
        {
            jsonData = JsonConvert.SerializeObject(personList, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(filePath, jsonData);
        }
        //function to remove book from person book list and add it back to available book list in book class
        public void removeBook(int _bookPosition, string _personName)
        {
            Person existingPerson = new Person();
            bool personExists = false;
            foreach (Person person in personList)
            {
                if (person.fullName == _personName)
                {
                    personExists = true;
                    existingPerson = person;
                    break;
                }
            }
            if(personExists == true)
            {
                Book book = new Book();
                book.loadBooks();
                book.addBook(existingPerson.takenBooks[_bookPosition].name,
                    existingPerson.takenBooks[_bookPosition].author,
                    existingPerson.takenBooks[_bookPosition].category,
                    existingPerson.takenBooks[_bookPosition].ISBN,
                    existingPerson.takenBooks[_bookPosition].language,
                    existingPerson.takenBooks[_bookPosition].pubDate);
                DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                if (existingPerson.takeDate[_bookPosition] < date)
                {
                    Console.WriteLine("You've violated the law by not returning the book back in time");
                }
                existingPerson.takenBooks.RemoveAt(_bookPosition);
                existingPerson.takeDate.RemoveAt(_bookPosition);
                refreshPersons();
            }
            else
            {
                Console.WriteLine("Sorry, person not registered in our system");
            }
        }
        // returns a person object by his name
        public Person showPerson(string _name)
        {
            Person existingPerson = new Person();
            foreach (Person person in personList)
            {
                if (person.fullName == _name)
                {
                    existingPerson = person;
                    return existingPerson;
                }
            }
            return null;
        }
        //return all the books that are beeing taken
        public List<Book> getPersonsBookList()
        {
            List<Book> listToShow = new List<Book>();
            foreach (Person person in personList)
            {
                foreach (Book book in person.takenBooks)
                {
                    listToShow.Add(book);
                }

            }

            return listToShow;
        }
    }
}
