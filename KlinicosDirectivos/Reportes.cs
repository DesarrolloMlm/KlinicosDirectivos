
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------


namespace KlinicosDirectivos
{

using System;
    using System.Collections.Generic;
    
public partial class Reportes
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Reportes()
    {

        this.ReportesEspecialidades = new HashSet<ReportesEspecialidades>();

    }


    public int id { get; set; }

    public string grupo { get; set; }

    public string nombre { get; set; }

    public string defaultHtml { get; set; }

    public string defaultStoreProcedure { get; set; }

    public string roles { get; set; }

    public string pageOrientation { get; set; }

    public string pageSize { get; set; }

    public string pageMarginsLeft { get; set; }

    public string pageMarginsBottom { get; set; }

    public string pageMarginsRight { get; set; }

    public string pageMarginsTop { get; set; }

    public string divShown { get; set; }

    public string validateRules { get; set; }

    public string tipoSector { get; set; }

    public string tipoEspecialidad { get; set; }

    public string fomatosSalida { get; set; }

    public string usuarioCrea { get; set; }

    public System.DateTime fechaCrea { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ReportesEspecialidades> ReportesEspecialidades { get; set; }

}

}
