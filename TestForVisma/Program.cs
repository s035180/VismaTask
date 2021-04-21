using System;

namespace TestForVisma
{
    class Program
    {
        static void Main(string[] args)
        {
            Book book = new Book();
            book.loadBooks();
            Console.WriteLine("Welcome, what would you like to do?");
            while (true)
            {            
                Console.WriteLine("In order to see commands write: !help");
                string answer;
                answer = Console.ReadLine();
                if(answer == "!help")
                {
                    Console.WriteLine("Type (insert) to insert new book");
                    Console.WriteLine("Type (delete) to delete a book");
                    Console.WriteLine("Type (take) to take a book");
                    Console.WriteLine("Type (return) to return a book");
                    Console.WriteLine("Type (show) to see all books");
                    Console.WriteLine("Type (showmy) to see all person books");
                } 
                else if(answer == "insert")
                {
                    Console.WriteLine("Enter book name: ");
                    string bookName = Console.ReadLine();
                    Console.WriteLine("Enter book author: ");
                    string author = Console.ReadLine();
                    Console.WriteLine("Enter book cathegory: ");
                    string cathegory = Console.ReadLine();
                    Console.WriteLine("Enter book language: ");
                    string language = Console.ReadLine();
                    Console.WriteLine("Enter book ISBN: ");
                    string ISBN = Console.ReadLine();
                    Console.WriteLine("Enter book publication date (yyyy, mm, dd): ");
                    string pubDate = Console.ReadLine();
                    DateTime date = new DateTime();
                    date = Convert.ToDateTime(pubDate);
                    book.addBook(bookName, author, cathegory, ISBN, language, date);


                }
                else if (answer == "delete")
                {
                    Console.WriteLine("Enter book ID to delete: ");
                    try
                    {
                        string bookid = Console.ReadLine();
                        book.deleteBook(Convert.ToInt32(bookid));
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong");
                    }
                }
                else if (answer == "take")
                {
                    Console.WriteLine("Enter your name: ");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter book ID you want to take");
                    string bookid = Console.ReadLine();
                    try
                    {
                        book.takeBook(Convert.ToInt32(bookid), name);
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong");
                    }


                }
                else if (answer == "return")
                {
                    Console.WriteLine("Enter your name: ");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter book ID you want to return");
                    string bookid = Console.ReadLine();
                    try
                    {
                        book.returnBook(Convert.ToInt32(bookid), name);
                    }
                    catch
                    {
                        Console.WriteLine("Something went wrong");
                    }
                }
                else if (answer == "show")
                {
                    Console.WriteLine("Would you like to use filter? (yes, no)");
                    string answer2 = Console.ReadLine();
                    if (answer2 == "yes")
                    {
                        Console.WriteLine("Enter 1 for name");
                        Console.WriteLine("Enter 2 for author");
                        Console.WriteLine("Enter 3 for category");
                        Console.WriteLine("Enter 4 for language");
                        Console.WriteLine("Enter 5 for ISBN");
                        Console.WriteLine("Enter 6 for pubDate");
                        string answer3 = Console.ReadLine();
                        int position = Convert.ToInt32(answer3);
                        if(position > 6 || position <  1)
                        {
                            Console.WriteLine("Number between 1-5");
                        } else
                        {
                            Console.WriteLine("Enter searching value: ");
                            if (position == 6)
                                Console.WriteLine("yyyy, mm, dd");
                            string answer4 = Console.ReadLine();
                            try
                            {
                                if (position == 6)
                                {
                                    DateTime date = new DateTime();
                                    date = Convert.ToDateTime(answer4);
                                    book.filtering(answer4, true, position);
                                }
                                else
                                {
                                    book.filtering(answer4, false, position);
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Something went wrong");
                            }
                        }
                    }
                    else if (answer2 == "no")
                    {
                        book.showBooks(book.getBookList(), false);
                    }
                    else Console.WriteLine("Bad answer");
                }
                else if (answer == "showmy")
                {
                    Console.WriteLine("Enter your name");
                    string answer2 = Console.ReadLine();
                    book.showPersonBook(answer2);

                }
            }
        }
    }
}
