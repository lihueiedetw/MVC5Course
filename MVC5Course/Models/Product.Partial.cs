namespace MVC5Course.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.ProductId == default(int))
            {
                //新增
            }
            else
            {
                //修改
            }            
            

            if (this.Stock > 10 && this.Price < 100)
            {
                yield return new ValidationResult("價格有點太低了哦!!", new string[] { "Price"});
            }

            if (this.Stock < 10)
            {
                //有指定欄位
                yield return new ValidationResult("庫存過低，請注意一下哦!", new string[] { "Stock" });
            }

            if (this.Stock < 5 || Price < 10)
            {
                //不指定欄位
                yield return new ValidationResult("庫存<5 或者 價格< 10，請注意一下!!");
            }
        }
    }

    public partial class ProductMetaData
    {
        [Required]
        public int ProductId { get; set; }
        
        [StringLength(80, ErrorMessage="欄位長度不得大於 80 個字元")]
        [驗證兩個空白(ErrorMessage = "欄位驗證需有兩個空白")]
        public string ProductName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<decimal> Stock { get; set; }
    
        public virtual ICollection<OrderLine> OrderLines { get; set; }
    }
}
