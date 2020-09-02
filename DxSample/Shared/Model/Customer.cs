﻿using DevExpress.Xpo;

namespace DxSample.Shared.Model {
    //public class Customer :XPObject {
    //    public Customer(Session session) : base(session) { }

    //    private string fContactName;
    //    [Indexed(Unique = true)]
    //    public string ContactName {
    //        get { return fContactName; }
    //        set { SetPropertyValue(nameof(ContactName), ref fContactName, value); }
    //    }
    //}

    public class Customer : XPObject {
        public Customer(Session session) : base(session) { }
        private string fFirstName;
        public string FirstName {
            get { return fFirstName; }
            set { SetPropertyValue(nameof(FirstName), ref fFirstName, value); }
        }

        private string fLastName;
        public string LastName {
            get { return fLastName; }
            set { SetPropertyValue(nameof(LastName), ref fLastName, value); }
        }
        [PersistentAlias("Concat([FirstName], ' ', [LastName])")]
        public string ContactName {
            get { return (string)EvaluateAlias(nameof(ContactName)); }
        }
        [Association("CustomerOrders")]
        public XPCollection<Order> Orders {
            get { return GetCollection<Order>(nameof(Orders)); }
        }
    }

}