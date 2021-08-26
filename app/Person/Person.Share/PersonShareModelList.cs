using Person.Share.Model.Person;
using Person.Share.Model.PersonContact;
using System;
using System.Collections.Generic;

namespace Person.Share
{
    public static class PersonShareModelList
    {
        public static IEnumerable<Type> ShareModelList
        {
            get 
            {
                return new Type[] { typeof(PersonLookupModel), typeof(PersonModel), typeof(PersonContactModel) };
            }
        }
    }
}
