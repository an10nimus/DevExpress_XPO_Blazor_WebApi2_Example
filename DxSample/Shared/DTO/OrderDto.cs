using System;

namespace DxSample.Shared.DTO {
    public class OrderDto {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Freight { get; set; }
        public string Customer { get; set; }
    }
}
