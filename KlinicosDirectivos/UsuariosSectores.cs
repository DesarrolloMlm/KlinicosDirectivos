
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
    
public partial class UsuariosSectores
{

    public int id { get; set; }

    public int idSector { get; set; }

    public int idUsuario { get; set; }

    public string usuarioCrea { get; set; }

    public System.DateTime fechaCrea { get; set; }

    public string usuarioModi { get; set; }

    public System.DateTime fechaModi { get; set; }

    public string roles { get; set; }



    public virtual Usuarios Usuarios { get; set; }

    public virtual Sectores Sectores { get; set; }

}

}
