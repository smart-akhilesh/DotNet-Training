using System;

namespace Assignment5
{
    public class Books
    {
        public string BookName { get; }
        public string AuthorName { get; }

        public Books(string bookName, string authorName)
        {
            BookName = bookName;
            AuthorName = authorName;
        }

        public void Display() =>
            Console.WriteLine($"Book Name: {BookName}\nAuthor Name: {AuthorName}");
    }

    public class BookShelf
    {
        private readonly Books[] books = new Books[5];

        public Books this[int index]
        {
            get => books[index];
            set => books[index] = value;
        }

        public void DisplayAllBooks()
        {
            for (int i = 0; i < books.Length; i++)
            {
                Console.WriteLine($"\nBook {i + 1} details:");
                books[i].Display();
            }
        }
    }

    public class Program
    {
        public static void Main()
        {
            var shelf = new BookShelf();

            for (int i = 0; i < 5; i++)
            {
                Console.Write($"\nEnter Book {i + 1} Name: ");
                string bookName = Console.ReadLine();

                Console.Write("Enter Author Name: ");
                string authorName = Console.ReadLine();

                shelf[i] = new Books(bookName, authorName);
            }

            shelf.DisplayAllBooks();
            Console.Read();
        }
    }
}
