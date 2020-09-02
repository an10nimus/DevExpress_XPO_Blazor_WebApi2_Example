using DevExpress.Xpo;
using DxSample.Shared.DTO;
using DxSample.Shared.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DxSample.Shared.Repository.Impl {
    public sealed class CustomerRepository : ICustomerRepository {
        private UnitOfWork uow;
        public CustomerRepository(UnitOfWork uow) {
            this.uow = uow;
        }
        private CustomerDto CreateDto(Customer model) {
            return new CustomerDto() {
                Id = model.Oid,
                ContactName = model.ContactName
            };
        }
        private void ApplyDto(CustomerDto source, Customer dest) {
            dest.ContactName = source.ContactName;
        }
        async Task<CustomerDto> IBaseRepostory<CustomerDto>.Create(CustomerDto data) {
            Customer model = new Customer(uow);
            ApplyDto(data, model);
            await uow.CommitChangesAsync();
            return CreateDto(model);
        }

        async Task IBaseRepostory<CustomerDto>.Delete(int id) {
            Customer model = await uow.GetObjectByKeyAsync<Customer>(id);
            uow.Delete(model);
            await uow.CommitChangesAsync();
        }

        Task<List<CustomerDto>> IBaseRepostory<CustomerDto>.Get() {
            return uow.Query<Customer>()
                .Select(c => new CustomerDto() { Id = c.Oid, ContactName = c.ContactName })
                .ToListAsync();
        }

        async Task<CustomerDto> IBaseRepostory<CustomerDto>.Get(int id) {
            Customer model = await uow.GetObjectByKeyAsync<Customer>(id);
            return CreateDto(model);
        }

        async Task<CustomerDto> IBaseRepostory<CustomerDto>.Update(CustomerDto data) {
            Customer model = await uow.GetObjectByKeyAsync<Customer>(data.Id);
            ApplyDto(data, model);
            await uow.CommitChangesAsync();
            return CreateDto(model);
        }
    }
}
