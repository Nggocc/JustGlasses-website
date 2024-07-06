namespace Shopweb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Cart_Details = new HashSet<Cart_Details>();
            Order_Details = new HashSet<Order_Details>();
            WishLists = new HashSet<WishList>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Mã sản phẩm:")]
        public int product_id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [DisplayName("Tên sản phẩm:")]
        [StringLength(1000)]
        public string name { get; set; }

        [Required(ErrorMessage = "SKU sản phẩm không được để trống")]
        [StringLength(100)]
        public string SKU { get; set; }

        [Required(ErrorMessage = "Mô tả sản phẩm không được để trống")]
        [DisplayName("Mô tả:")]
        [StringLength(2000)]
        public string description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm không được để trống")]
        [DisplayName("Giá:")]
        public decimal price { get; set; }

        [Required(ErrorMessage = "Hàng còn trong kho không được để trống")]
        [DisplayName("Stock:")]
        public int stock { get; set; }

        [Required(ErrorMessage = "Nguyên liệu sản phẩm không được để trống")]
        [DisplayName("Chất liệu:")]
        [StringLength(100)]
        public string material { get; set; }

        [Required(ErrorMessage = "Giói tính không được để trống")]
        [DisplayName("Giới tính:")]
        [StringLength(10)]
        public string gender { get; set; }

        [Required(ErrorMessage = "Hình dáng sản phẩm không được để trống")]
        [DisplayName("Hình dáng:")]
        [StringLength(100)]
        public string shape { get; set; }

        [Required(ErrorMessage = "Màu sắc sản phẩm không được để trống")]
        [DisplayName("Màu sắc:")]
        [StringLength(100)]
        public string color { get; set; }

        [DisplayName("Hình ảnh:")]
        [StringLength(20)]
        public string image { get; set; }

        [DisplayName("Ảnh mô tả:")]
        [StringLength(100)]
        public string gallery { get; set; }

        [Required(ErrorMessage = "Chiều rộng sản phẩm không được để trống")]
        [DisplayName("Width:")]
        public double eye_width { get; set; }

        [Required(ErrorMessage = "Chiều dài sản phẩm không được để trống")]
        [DisplayName("Legth:")]
        public double eye_lenth { get; set; }

        [DisplayName("Ngày nhập kho:")]
        public DateTime? InventoryDate { get; set; }

        [DisplayName("Giá nhập:")]
        public double? entryPrice { get; set; }

        [Required(ErrorMessage = "Danh mục sản phẩm không được để trống")]
        [DisplayName("Mã danh mục:")]
        public int category_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart_Details> Cart_Details { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order_Details> Order_Details { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WishList> WishLists { get; set; }
    }
}
