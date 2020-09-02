using DevExpress.Xpo;
using DxSample.Shared.DTO;
using DxSample.Shared.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace DxSample.Shared.Repository.Impl {
    public sealed class OrderRepository : IOrderRepository {
        private UnitOfWork uow;
        public OrderRepository(UnitOfWork uow ) {
            this.uow = uow;
        }
        private OrderDto CreateDto(Order model) {
            return new OrderDto() {
                Id = model.Oid,
                ProductName = model.ProductName,
                OrderDate = model.OrderDate,
                Freight = model.Freight,
                Customer = model.Customer.ContactName
            };
        }
        private async Task ApplyDto(OrderDto source, Order dest) {
            dest.ProductName = source.ProductName;
            dest.OrderDate = source.OrderDate;
            dest.Freight = source.Freight;
            if (!string.IsNullOrWhiteSpace(source.Customer)) {
                dest.Customer = await uow.Query<Customer>()
                    .SingleAsync(c => c.ContactName == source.Customer);
            }
        }
        async Task<OrderDto> IBaseRepostory<OrderDto>.Create(OrderDto data) {
            Order model = new Order(uow);
            await ApplyDto(data, model);
            await uow.CommitChangesAsync();
            return CreateDto(model);
        }

        async Task IBaseRepostory<OrderDto>.Delete(int id) {
            Order order = await uow.GetObjectByKeyAsync<Order>(id);
            uow.Delete(order);
            await uow.CommitChangesAsync();
        }

        Task<List<OrderDto>> IBaseRepostory<OrderDto>.Get() {
            return uow.Query<Order>()
                .Select(o => new OrderDto() {
                    Id = o.Oid,
                    ProductName = o.ProductName,
                    OrderDate = o.OrderDate,
                    Freight = o.Freight,
                    Customer = o.Customer.ContactName
                }).ToListAsync();
        }

        async Task<OrderDto> IBaseRepostory<OrderDto>.Get(int id) {
            Order model = await uow.GetObjectByKeyAsync<Order>(id);
            return CreateDto(model);
        }

        async Task<OrderDto> IBaseRepostory<OrderDto>.Update(OrderDto data) {
            Order model = await uow.GetObjectByKeyAsync<Order>(data.Id);
            await ApplyDto(data, model);
            await uow.CommitChangesAsync();
            return CreateDto(model);
        }
    }
}
