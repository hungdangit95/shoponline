using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShopOnlineApp.Data.Enums;
using ShopOnlineApp.Data.Interfaces;
using ShopOnlineApp.Infrastructure.SharedKernel;

namespace ShopOnlineApp.Data.Entities
{
    [Table("Bills")]
    public class Bill : DomainEntity<int>, ISwitchable, IDateTracking
    {

        public Bill(string customerName, string customerAddress, string customerMobile, string customerMessage,
            BillStatus billStatus, PaymentMethod paymentMethod, Status status, Guid? customerId)
        {
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerMobile = customerMobile;
            CustomerMessage = customerMessage;
            BillStatus = billStatus;
            PaymentMethod = paymentMethod;
            Status = status;
            CustomerId = customerId;
            _billDetails = new List<BillDetail>();
        }

        public Bill(int id, string customerName, string customerAddress, string customerMobile, string customerMessage,
           BillStatus billStatus, PaymentMethod paymentMethod, Status status, Guid? customerId)
        {
            Id = id;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerMobile = customerMobile;
            CustomerMessage = customerMessage;
            BillStatus = billStatus;
            PaymentMethod = paymentMethod;
            Status = status;
            CustomerId = customerId;
            _billDetails = new List<BillDetail>();
        }
        [Required]
        [MaxLength(256)]
        public string CustomerName { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerAddress { set; get; }

        [Required]
        [MaxLength(50)]
        public string CustomerMobile { set; get; }

        [Required]
        [MaxLength(256)]
        public string CustomerMessage { set; get; }

        public PaymentMethod PaymentMethod { set; get; }

        public BillStatus BillStatus { set; get; }

        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }

        [DefaultValue(Status.InActive)]
        public Status Status { set; get; } = Status.InActive;

        public Guid? CustomerId { set; get; }

        [ForeignKey("CustomerId")]
        public virtual AppUser User { set; get; }

        private List<BillDetail> _billDetails = new List<BillDetail>();
        public virtual IReadOnlyList<BillDetail> BillDetails => _billDetails.AsReadOnly();

        public void AddBillDetails(IList<BillDetail> billDetails)
        {
            _billDetails.AddRange(billDetails);
        }

        public void ClearBillDetail()
        {
            _billDetails.Clear();
        }
    }
}