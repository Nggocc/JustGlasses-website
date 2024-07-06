namespace Shopweb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Shipment")]
    public partial class Shipment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shipment()
        {
            Orders = new HashSet<Order>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Mã giao hàng")]
        public int shipment_id { get; set; }

        [StringLength(100)]
        [DisplayName("Địa chỉ giao hàng")]
        public string address { get; set; }

        [StringLength(50)]
        [DisplayName("Thành phố")]
        public string city { get; set; }

        [StringLength(100)]
        [DisplayName("Tên người nhận")]
        public string receiverName { get; set; }

        [StringLength(11)]
        [DisplayName("Điện thoại")]
        public string phone { get; set; }

        [DisplayName("Mã khách hàng:")]
        public int customer_id { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
