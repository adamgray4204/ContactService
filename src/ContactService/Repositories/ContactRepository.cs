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
            Contact result = null;
            using (var db = new LiteDatabase(ContactsLiteDB))
            {
                var contacts = Contacts(db);
                result = contacts.Find(c => c.Id == Id).FirstOrDefault();
            }

            return await Task.FromResult(result);
        }

        public async Task<IEnumerable<Contact>> List()
        {
            IEnumerable<Contact> result;
            using (var db = new LiteDatabase(ContactsLiteDB))
            {
                var contacts = Contacts(db);
                result = contacts.FindAll().ToList();
            }

            return await Task.FromResult(result);
        }

        public async Task<int> Insert(Contact contact)
        {
            using (var db = new LiteDatabase(ContactsLiteDB))
            {
                var contacts = Contacts(db);
                contacts.Insert(contact);
                return await Task.FromResult(contact.Id);
            }
        }

        public async Task<bool> Delete(int Id)
        {
            bool result = false;
            using (var db = new LiteDatabase(ContactsLiteDB))
            {
                var contacts = Contacts(db);
                result = contacts.Delete(Id);
            }

            return await Task.FromResult(result);
        }

        public async Task<bool> Update(int Id, Contact contact)
        {
            var update = await Find(Id);
            if (update != null)
            {
                update.Name = contact.Name;
                update.Phone = contact.Phone;
                update.Email = contact.Email;
                return await Task.FromResult(UpdateContact(update));
            }
            else
            {
                return await Task.FromResult(false);
            }
        }

        private bool UpdateContact(Contact contact)
        {
            using (var db = new LiteDatabase(ContactsLiteDB))
            {
                var contacts = Contacts(db);
                return contacts.Update(contact);
            }
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
