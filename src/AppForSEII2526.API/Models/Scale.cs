namespace AppForSEII2526.API.Models
{
    public class Scale
    {//Creacion de la clase Scale con id como llave primaria y name como string
        [Key]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.", MinimumLength = 4)]
        public string Name { get; set; }

        public IList<Repair> Repairs { get; set; }
    }
}
