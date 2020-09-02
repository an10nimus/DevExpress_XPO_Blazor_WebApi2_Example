using DevExpress.Xpo;

namespace DxSample.Shared.Model {
    public class Customer :XPObject {
        public Customer(Session session) : base(session) { }

        private string fContactName;
        [Indexed(Unique = true)]
        public string ContactName {
            get { return fContactName; }
            set { SetPropertyValue(nameof(ContactName), ref fContactName, value); }
        }
    }
}
