using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using Xamarin.Forms;

namespace GraphyPCL
{
    public static class DatabaseManager
    {
        public static SQLiteConnection DbConnection
        {
            get;
            private set;
        }

        static DatabaseManager()
        {
            DbConnection = DependencyService.Get<ISQLite>().GetConnection();

            // Turn on Foreign Key support
//            var foreignKeyOn = "PRAGMA foreign_keys = ON";
//            DbConnection.Execute(foreignKeyOn);

            // Create tables using SQL commands
            var createContact = "CREATE TABLE Contact (Id INTEGER PRIMARY KEY NOT NULL, FirstName VARCHAR, MiddleName VARCHAR, LastName VARCHAR, Organization VARCHAR, ImageName VARCHAR, Birthday DATETIME, Favourite BOOL DEFAULT 0)";
            DbConnection.Execute(createContact);
            var createPhoneNumber = "CREATE TABLE PhoneNumber (Id INTEGER PRIMARY KEY NOT NULL, Type VARCHAR, Number VARCHAR, ContactId INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
            DbConnection.Execute(createPhoneNumber);
            var createAddress = "CREATE TABLE Address (Id INTEGER PRIMARY KEY NOT NULL, Type VARCHAR, StreetLine1 VARCHAR, StreetLine2 VARCHAR, City VARCHAR, Province VARCHAR, PostalCode VARCHAR, Country VARCHAR, ContactId INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
            DbConnection.Execute(createAddress);
            var createEmail = "CREATE TABLE Email (Id INTEGER PRIMARY KEY NOT NULL, Type VARCHAR, Address VARCHAR, ContactId INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
            DbConnection.Execute(createEmail);
            var createSpecialDate = "CREATE TABLE SpecialDate (Id INTEGER PRIMARY KEY NOT NULL , Type VARCHAR, Date DATETIME, ContactId INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
            DbConnection.Execute(createSpecialDate);
            var createUrl = "CREATE TABLE Url (Id INTEGER PRIMARY KEY NOT NULL, Type VARCHAR, Link VARCHAR, ContactId INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
            DbConnection.Execute(createUrl);
            var createInstantMessage = "CREATE TABLE InstantMessage (Id INTEGER PRIMARY KEY NOT NULL, Type VARCHAR, Nickname VARCHAR, ContactId INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
            DbConnection.Execute(createInstantMessage);
            var createTag = "CREATE TABLE Tag (Id INTEGER PRIMARY KEY NOT NULL, Name VARCHAR, Detail VARCHAR)";
            DbConnection.Execute(createTag);
            var createContactTagMap = "CREATE TABLE ContactTagMap (Id INTEGER PRIMARY KEY NOT NULL, ContactId INTEGER, TagId INTEGER, FOREIGN KEY(ContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(TagId) REFERENCES Tag(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
            DbConnection.Execute(createContactTagMap);
            var createRelationshipType = "CREATE TABLE RelationshipType (Id INTEGER PRIMARY KEY NOT NULL, Name VARCHAR)";
            DbConnection.Execute(createRelationshipType);
            var createRelationship = "CREATE TABLE Relationship (Id INTEGER PRIMARY KEY NOT NULL, ExtraInfo VARCHAR, FromContactId INTEGER, ToContactId INTEGER, RelationshipTypeId INTEGER, FOREIGN KEY(FromContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(ToContactId) REFERENCES Contact(Id) ON DELETE CASCADE ON UPDATE CASCADE, FOREIGN KEY(RelationshipTypeId) REFERENCES RelationshipType(Id) ON DELETE CASCADE ON UPDATE CASCADE)";
            DbConnection.Execute(createRelationship);

            // ## For tests
            CreateDummyData();
        }

        // ## For tests
        public static void CreateDummyData()
        {
            Debug.WriteLine("start adding data");

            // Contacts
            var contact1 = new Contact();
            contact1.Id = 1;
            contact1.FirstName = "Andy";
            contact1.LastName = "Rubin";
            DbConnection.Insert(contact1);
                
            var contact2 = new Contact();
            contact2.Id = 2;
            contact2.FirstName = "Bill";
            contact2.MiddleName = "Henry";
            contact2.LastName = "Gates";
            contact2.Organization = "Microsoft";
            contact2.Birthday = new DateTime(1955, 11, 28);
            contact2.ImageName = "Bill.jpg";
            contact2.Favourite = true;
            DbConnection.Insert(contact2);

            var contact3 = new Contact();
            contact3.Id = 3;
            contact3.FirstName = "ben";
            contact3.LastName = "Afflect";
            DbConnection.Insert(contact3);

            var contact4 = new Contact();
            contact4.Id = 4;
            contact4.FirstName = "Satya";
            contact4.LastName = "Nandela";
            contact4.Birthday = new DateTime(1911, 11, 11);
            DbConnection.Insert(contact4);

            var contact5 = new Contact();
            contact5.Id = 5;
            contact5.FirstName = "Zelda";
            DbConnection.Insert(contact5);

            var contact6 = new Contact();
            contact6.Id = 6;
            contact6.FirstName = "Jennifer";
            contact6.LastName = "Gates";
            DbConnection.Insert(contact6);

            var contact7 = new Contact();
            contact7.Id = 7;
            contact7.FirstName = "Maple";
            DbConnection.Insert(contact7);

            var contact8 = new Contact();
            contact8.Id = 8;
            contact8.FirstName = "George";
            DbConnection.Insert(contact8);

            var contact9 = new Contact();
            contact9.Id = 9;
            contact9.Organization = "Null Org.";
            contact9.Birthday = new DateTime(1999, 9, 9);
            DbConnection.Insert(contact9);

            var contact10 = new Contact();
            contact10.Id = 10;
            contact10.FirstName = "";
            DbConnection.Insert(contact10);

            // Phone numbers
            var number1 = new PhoneNumber();
            number1.Id = 1;
            number1.ContactId = 1;
            number1.Number = "111";
            number1.Type = "Office";
            DbConnection.Insert(number1);
                                    
            var number2 = new PhoneNumber();
            number2.Id = 2;
            number2.ContactId = 2;
            number2.Number = "0123456789";
            number2.Type = "Office";
            DbConnection.Insert(number2);
                                    
            var number3 = new PhoneNumber();
            number3.Id = 3;
            number3.ContactId = 2;
            number3.Number = "0987654321";
            number3.Type = "Home";
            DbConnection.Insert(number3);

            // Address
            var address1 = new Address();
            address1.Id = 1;
            address1.Type = "Home";
            address1.StreetLine1 = "1 Capitol Hill";
            address1.StreetLine2 = "Apt 1";
            address1.City = "Seattle";
            address1.Province = "WA";
            address1.Country = "United States";
            address1.ContactId = 2;
            DbConnection.Insert(address1);

            var address2 = new Address();
            address2.Id = 2;
            address2.Type = "Work";
            address2.StreetLine1 = "1 Microsoft Way";
            address2.City = "Redmond";
            address2.ContactId = 2;
            DbConnection.Insert(address2);

            // Email
            var email1 = new Email();
            email1.Id = 2;
            email1.Address = "bill@microsoft.com";
            email1.Type = "Work";
            email1.ContactId = 2;
            DbConnection.Insert(email1);

            // Url
            var url1 = new Url();
            url1.Id = 1;
            url1.Type = "Blog";
            url1.Link = "billgates.com";
            url1.ContactId = 2;
            DbConnection.Insert(url1);

            var url2 = new Url();
            url2.Id = 2;
            url2.Type = "Philanthropy";
            url2.Link = "billandmelinda.org";
            url2.ContactId = 2;
            DbConnection.Insert(url2);

            // IMs
            var im1 = new InstantMessage();
            im1.Id = 1;
            im1.Type = "Skype";
            im1.Nickname = "billgates";
            im1.ContactId = 2;
            DbConnection.Insert(im1);

            // Special Dates
            var date1 = new SpecialDate();
            date1.Id = 1;
            date1.Type = "Founded Microsoft";
            date1.Date = new DateTime(1975, 4, 4);
            date1.ContactId = 2;
            DbConnection.Insert(date1);

            // Tags
//                var tag2 = new Tag()
//                { 
//                    Id = 2,
//                    Name = "Important",
//                };
//                db.Insert(tag2);

            var tag1 = new Tag()
            {
                Id = 1,
                Name = "",
                Detail = "Chairman of Microsoft",
            };
            DbConnection.Insert(tag1);

            var tagMap1 = new ContactTagMap()
            {
                Id = 1,
                ContactId = 2,
                TagId = 1,
            };
            DbConnection.Insert(tagMap1);

            var tagMap2 = new ContactTagMap()
            {
                Id = 2,
                ContactId = 2,
                TagId = 2,
            };
            DbConnection.Insert(tagMap2);

            // Relationship
            var connType1 = new RelationshipType()
            {
                Id = 1,
                Name = "Advisor",
            };
            DbConnection.Insert(connType1);
            var connType2 = new RelationshipType()
            { 
                Id = 2,
                Name = "Daughter", 
            };
            DbConnection.Insert(connType2);

            var conn1 = new Relationship()
            {
                Id = 1,
                FromContactId = 2,
                ToContactId = 4,
                RelationshipTypeId = 1,
                ExtraInfo = "Bill will advise Satya with his new CEO role.",
            };
            DbConnection.Insert(conn1);
            var conn2 = new Relationship()
            {
                Id = 2,
                FromContactId = 6,
                ToContactId = 2,
                RelationshipTypeId = 2,
            };
            DbConnection.Insert(conn2);
//                var conn3 = new Relationship()
//                {
//                    Id = 3,
//                    FromContactId = 2,
//                    ToContactId = 8,
//                    RelationshipTypeId = 1,
//                };
//                db.Insert(conn3);

            Debug.WriteLine("stop adding data");
        }

        /// <summary>
        /// Get all rows from a table
        /// </summary>
        /// <returns>The rows.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static IList<T> GetRows<T>() where T : new()
        {
            return DbConnection.Table<T>().ToList();
        }

        /// <summary>
        /// Get a row according to its primary key
        /// </summary>
        /// <returns>The row.</returns>
        /// <param name="id">Identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetRow<T>(int id) where T : IIdContainer, new()
        {
            return DbConnection.Table<T>().Where(x => x.Id == id).FirstOrDefault();
        }

        public static IList<T> GetRows<T>(IList<int> idList) where T : IIdContainer, new()
        {
            return DbConnection.Table<T>().Where(x => idList.Contains(x.Id)).ToList();
        }

        public static IList<T> GetRowsRelatedToContact<T>(int contactId) where T : IContactIdRelated, new()
        {
            return DbConnection.Table<T>().Where(x => x.ContactId == contactId).ToList();
        }

        /// <summary>
        /// Gets the relationships start from a contact to other contacts
        /// </summary>
        /// <returns>The relationships from contact.</returns>
        /// <param name="contactId">Contact identifier.</param>
        public static IList<Relationship> GetRelationshipsFromContact(int contactId)
        {
            return DbConnection.Table<Relationship>().Where(x => x.FromContactId == contactId).ToList();
        }

        public static IList<Relationship> GetRelationshipsToContact(int contactId)
        {
            return DbConnection.Table<Relationship>().Where(x => x.ToContactId == contactId).ToList();
        }

        /// <summary>
        /// Creates a new contact.
        /// Demo implementation. Should change later.
        /// </summary>
        public static void AddNewContact(Contact contact)
        {
            var contacts = GetRows<Contact>();
            var contactsCount = contacts.Count;

            var id = contactsCount;
            contact.Id = id + 1;

            DbConnection.Insert(contact);
        }
    }
}