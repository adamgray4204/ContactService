using ContactService.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace ContactService.Repositories
{
    public class ContactRepository
    { 
        #region Members
        public async Task<Contact> Find(int Id)
        {
            using var db = new LiteDatabase(ContactsLiteDB);
            var contacts = Contacts(db);
            return await Task.FromResult(contacts.Find(c => c.Id == Id).FirstOrDefault());
        }

        public async Task<IEnumerable<Contact>> List()
        {
            using var db = new LiteDatabase(ContactsLiteDB);
            var contacts = Contacts(db);
            return await Task.FromResult(contacts.FindAll().ToList());
        }

        public async Task<int> Insert(ContactPutPost contactPutPost)
        {
            using var db = new LiteDatabase(ContactsLiteDB);
            var contacts = Contacts(db);
            var contact = contactPutPost.Contact;
            contacts.Insert(contact);
            return await Task.FromResult(contact.Id);
        }

        public async Task<bool> Delete(int Id)
        {
            using var db = new LiteDatabase(ContactsLiteDB);
            var contacts = Contacts(db);
            return await Task.FromResult(contacts.Delete(Id));
        }

        public async Task<bool> Update(int Id, ContactPutPost contactPutPost)
        {
            var contact = contactPutPost.Contact;
            contact.Id = Id;
            return await Task.FromResult(UpdateContact(contact));
        }

        private bool UpdateContact(Contact contact)
        {
            using var db = new LiteDatabase(ContactsLiteDB);
            var contacts = Contacts(db);
            return contacts.Update(contact);
        }

        private ILiteCollection<Contact> Contacts(LiteDatabase db)
        {
            return db.GetCollection<Contact>(CollectionName);
        }
        #endregion

        #region Constants
        private const string ContactsLiteDB = "ContactsLite.DB";
        private const string CollectionName = "contacts";
        #endregion
    }
}
