using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ContactService.Models
{
    public class ContactPutPost
    {
        public ContactName Name { get; set; }
        public List<ContactPhone> Phone { get; set; }
        public string Email { get; set; }
        public ContactAddress Address { get; set; }

        public Contact Contact
        {
            get
            {
                return new Contact() { Address = Address, Email = Email, Name = Name, Phone = Phone };
            }
        }
    }

    public class Contact
    {
        public int Id { get; set; }
        public ContactName Name { get; set; }
        public List<ContactPhone> Phone { get; set; }
        public string Email { get; set; }
        public ContactAddress Address { get; set; }
    }

    public class ContactName
    {
        public string First { get; set; }
        public string Middle { get; set; }
        public string Last { get; set; }
    }

    public class ContactPhone
    {
        public string Number { get; set; }

        public string Type
        {
            get
            {
                return _type.ToString();
            }
            set
            {
                _type = (PhoneType)Enum.Parse(typeof(PhoneType), value, true);
            }
        }

        private PhoneType _type;
    }

    public enum PhoneType 
    { 
        Home,
        work,
        Mobile
    }

    public class ContactAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

}
