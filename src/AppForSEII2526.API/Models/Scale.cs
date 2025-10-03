namespace AppForSEII2526.API.Models
{
    public class Scale
    {//Creacion de la clase Scale con id como llave primaria y name como string
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Repair> Repairs { get; set; }
    }
}
