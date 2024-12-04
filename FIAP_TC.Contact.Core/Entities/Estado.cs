namespace FIAP_TC.Contact.Core.Entities;

public partial class Estado
{
    public int CodigoUf { get; set; }

    public string Uf { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public float Latitude { get; set; }

    public float Longitude { get; set; }

    public string Regiao { get; set; } = null!;

    public virtual ICollection<Municipio> Municipios { get; set; } = [];
}

