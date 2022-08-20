namespace UnemeAPI.Models.Sesion
{
    public class vmInfoDispositivo
    {
        public string? id { get; set; }//en mongo el id es de tipo string
        public string? evento { get; set; }
        public string? mac_address { get; set; }
        public string? so { get; set; }
        public string? version_so { get; set; }
        public string? nombre_app { get; set; }
        public string? login { get; set; }
        public DateTime? fecha { get; set; }
        public vmInfoDispositivo()
        {
            fecha = DateTime.UtcNow;
        }
    }
    //public class vmInfoDispositivoAdd
    //{
    //    [BsonId]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string? id { get; set; }//en mongo el id es de tipo string
    //    public string? evento { get; set; }
    //    public string? mac_address { get; set; }
    //    public string? so { get; set; }
    //    public string? version_so { get; set; }
    //    public string? nombre_app { get; set; }
    //    public string? login { get; set; }
    //    [BsonRepresentation(BsonType.DateTime)]
    //    public DateTime? fecha { get; set; }
    //    public vmInfoDispositivoAdd()
    //    {
    //        fecha = DateTime.UtcNow;
    //    }
    //}
}
