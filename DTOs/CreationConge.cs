namespace gestion_congeAPI.DTOs
{
    public class CreationConge
    {
        public int idConge { get; set; }
        public int IdEmploye { get; set; }
        public DateOnly DateDebut { get; set; }
        public DateOnly DateFin { get; set; }
        public string Libelle { get; set; }
        public string Type { get; set; } = string.Empty;

    }
}
