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
        public void loadPersons()
        {
            filePath = "TakenBooks.json";
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
            jsonData = System.IO.File.ReadAllText(filePath);
            personList = JsonConvert.DeserializeObject<List<Person>>(jsonData) ?? new List<Person>();
        }
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
            return canTakeBook;
        }
        public void refreshPersons()
        {
            using (var fs = new FileStream(filePath, FileMode.Truncate))
            {
            }
            Write();
        }
        public void Write()
        {
            jsonData = JsonConvert.SerializeObject(personList, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(filePath, jsonData);
        }
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

                if (existingPerson.takeDate[_bookPosition] > date)
                {
                    Console.WriteLine("You've violated the law by not giving the book back in time");
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
    }
}
