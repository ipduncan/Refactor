using System;
using System.Collections;
using System.Collections.Specialized;

namespace AimHealth.Safari.BulkPosting.Bus
{

//todo:  Interface does not have properties of the StringDictionary.  What do do?


    public interface IMapper
    {
        StringCollection GetPaymentProperties();
        void Add(string key, string value);
        void Clear();
        bool ContainsKey(string key);
        bool ContainsValue(string value);
        void CopyTo(Array array, int index);
        IEnumerator GetEnumerator();
        void Remove(string key);
        int Count { get; }
        bool IsSynchronized { get; }
        ICollection Keys { get; }
        object SyncRoot { get; }
        ICollection Values { get; }
        string this[string key] { get; set; }
    }

    public class Mapper : StringDictionary, IMapper
    {

        private readonly IPayment _payment;

        public Mapper(): this (new Payment())
        {
        }

        internal Mapper(IPayment payment)
        {
            _payment = payment;
        }

        public StringCollection GetPaymentProperties()
        {
            StringCollection props = _payment.GetListOfProperties();
            return props;
        }
    }
}
