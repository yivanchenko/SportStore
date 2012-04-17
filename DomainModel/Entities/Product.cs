namespace DomainModel.Entities
{
    using System;
    using System.Data.Linq.Mapping;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    [Table(Name = "Products")]
    public class Product
    {
        [Column(IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.OnInsert)]
        public int ProductId { get; set; }

        [Column] 
        [Required(ErrorMessage = "Please enter a product name")]
        public string Name { get; set; }

        [Column]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a description")]
        public string Description { get; set; }
        
        [Column]
        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Price must not be negative")]        
        public decimal Price { get; set; }
        
        [Column]
        [Required(ErrorMessage = "Please enter a category")]
        public string Category { get; set; }      

        [Column]
        public byte[] ImageData { get; set; }

        [Column]
        public string ImageMimeType { get; set; }
        
        [Column]
        public DateTime CreateDate { get; set; }        
    }
}
