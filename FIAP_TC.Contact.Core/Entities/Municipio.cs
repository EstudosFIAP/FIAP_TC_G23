namespace FIAP_TC.Contact.Core.Entities;

public partial class Municipio
{
    public int CodigoIbge { get; set; }
    public string Nome { get; set; } = null!;
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public bool Capital { get; set; }
    public int CodigoUf { get; set; }
    public string SiafiId { get; set; } = null!;
    public int Ddd { get; set; }
    public string FusoHorario { get; set; } = null!;
    public virtual Estado CodigoUfNavigation { get; set; } = null!;
}