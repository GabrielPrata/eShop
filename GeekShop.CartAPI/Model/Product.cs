using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShop.CartAPI.Model
{
    //Uso a anotação para definir um nome para a tabela no banco, caso eu não queira usar o nome padrão
    [Table("product")]
    public class Product
    {
        //Essa opção faz com que o Entity Framework não gere um ID automaticamente, mas sim que esse valor seja passado
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name")]
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Column("price")]
        [Required]
        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Column ("description")]
        [StringLength(500)]
        public string Description { get; set; }

        //Poderia mapear corretamente no banco
        [Column("category_name")]
        [StringLength(50)]
        public string CategoryName { get; set; }

        //Essa informação futuramente virá de outro microserviço
        [Column("image_url")]
        [StringLength(300)]
        public string ImageURL { get; set; }

    }
}
