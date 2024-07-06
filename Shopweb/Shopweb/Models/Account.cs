namespace Shopweb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        [Key]
        [StringLength(30)]
        [Required(ErrorMessage = "Login name khong duoc de trong")]
        [DisplayName("Tên đăng nhập")]

        public string loginName { get; set; }

       
        [StringLength(30)]
        [Required(ErrorMessage = "Password khong duoc de trong")]
        [DisplayName("Mật khẩu")]

        public string password { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Quyền")]

        public string role { get; set; }

        [DisplayName("Mã khách hàng:")]

        public int customer_id { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
