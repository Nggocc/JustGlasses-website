namespace Shopweb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            Order_Details = new HashSet<Order_Details>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Mã đơn hàng:")]
        public int order_id { get; set; }

        [DisplayName("Ngày đặt")]
        public DateTime date { get; set; }

        [StringLength(100)]
        [DisplayName("Tình trạng")]
        public string status { get; set; }

        [DisplayName("Mã giao hàng")]
        public int shipment_id { get; set; }

        [DisplayName("Mã thanh toán")]
        public int payment_id { get; set; }

        public virtual Payment Payment { get; set; }

        public virtual Shipment Shipment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Details> Order_Details { get; set; }
    }
}
