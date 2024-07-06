namespace Shopweb.Models
{
    using Shopweb.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            Accounts = new HashSet<Account>();
            Carts = new HashSet<Cart>();
            Payments = new HashSet<Payment>();
            Shipments = new HashSet<Shipment>();
            WishLists = new HashSet<WishList>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Mã khách hàng:")]
        public int customer_id { get; set; }

        [StringLength(100)]
        [DisplayName("Tên khách hàng:")]
        public string name { get; set; }

        [Required(ErrorMessage = "Email khach hang khong duoc de trong")]
        [StringLength(100)]
        public string email { get; set; }

        [StringLength(11)]
        [DisplayName("Điện thoại:")]
        public string phone { get; set; }

        [StringLength(200)]
        [DisplayName("Địa chỉ:")]
        public string address { get; set; }

        [StringLength(200)]
        [DisplayName("Hình ảnh:")]
        public string image { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Account> Accounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shipment> Shipments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
