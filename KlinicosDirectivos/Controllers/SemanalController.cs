﻿using KlinicosDirectivos.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KlinicosDirectivos.Controllers
{
    public class SemanalController : Controller
    {
        // GET: Semanal
        public ActionResult Index()
        {

            Klinicos_BEntities entidades = Repositorio.CrearEntityFramework();
            List<Sectores> listaSectores = entidades.Sectores.ToList();
            List<SelectListItem> sectores = new List<SelectListItem>();
            foreach (var sector in listaSectores)
            {
                sectores.Add(new SelectListItem() { Text = sector.nombre, Value = sector.id.ToString() });
            }




            var establecimientosTotales = entidades.Establecimientos.ToList();
            Establecimientos nombreEsta = new Establecimientos();
            string lugar = string.Empty;
            int id = (int)Session["Establecimiento"];
            foreach (var item in establecimientosTotales)
            {
                if (item.id == id)
                {
                    nombreEsta = item;
                    lugar = nombreEsta.nombre;
                }
            }

            ViewBag.Sectores = sectores;
            ViewBag.Establecimiento = lugar;

            List<ProfesionalVM> listProfesionalesVM = new List<ProfesionalVM>();
            IEnumerable<Profesionales> listProfesionales = entidades.ProfesionalesDisponibles.Join(entidades.Profesionales, pd => pd.id, p => p.id, (pd, p) => p);
            //var EspecialidadesXProfesional = entidades.ProfesionalesEspecialidades.Join(entidades.Especialidades, pe => pe.idEspecialidad, e => e.id, (pe, e) => new { idProfesional = pe.idProfesional, especialidad = e });
            var EspecialidadesXProfesional = new List<Especialidades>();
            foreach (Profesionales profesional in listProfesionales)
            {
                int? idprof = profesional.id;
                var  listaresultado = entidades.SP_ESPECIALIDADES_X_PROFESIONAL(idprof).ToList();
                List<Especialidades> listaEspecialidades = listaresultado;
                ProfesionalVM profesionalVM = new ProfesionalVM(profesional, listaEspecialidades);
                listProfesionalesVM.Add(profesionalVM);
            }

            return View("Inicio", listProfesionalesVM);
        }


        public ActionResult IndexFiltrado(int idSector)
        {
            try
            {



                Klinicos_BEntities entidades = Repositorio.CrearEntityFramework();

                List<ProfesionalVM> listProfesionalesVM = new List<ProfesionalVM>();
                IEnumerable<Profesionales> listProfesionales = entidades.ProfesionalesDisponibles.Join(entidades.Profesionales, pd => pd.id, p => p.id, (pd, p) => p);
                //var EspecialidadesXProfesional = entidades.ProfesionalesEspecialidades.Join(entidades.Especialidades, pe => pe.idEspecialidad, e => e.id, (pe, e) => new { idProfesional = pe.idProfesional, especialidad = e });

                var listProfEnSector = entidades.UsuariosSectores.Where(x => x.Sectores.id == idSector).Select(x => x.Usuarios.idProfesional).ToList();

                foreach (Profesionales profesional in listProfesionales)
                {
                    if (listProfEnSector.Contains(profesional.id))
                    {
                        //List<Especialidades> listaEspecialidades = EspecialidadesXProfesional.Where(x => x.idProfesional == profesional.id).Select(exp => (Especialidades)exp.especialidad).ToList();
                        List<Especialidades> listaEspecialidades = entidades.SP_ESPECIALIDADES_X_PROFESIONAL(profesional.id).ToList();
                        ProfesionalVM profesionalVM = new ProfesionalVM(profesional, listaEspecialidades);
                        listProfesionalesVM.Add(profesionalVM);
                    }
                }

                return PartialView("_TablaProfesionales", listProfesionalesVM);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }



        public ActionResult SemanalSector(int idSector)
        {
            DateTime fechaDesde = DateTime.Now.AddDays(-7);
            DateTime fechaHasta = DateTime.Now;
            Klinicos_BEntities entities = Repositorio.CrearEntityFramework();
            //var AtencionesXProfesional = from profesional in entities.Profesionales
            //                             join ae in entities.AtencionesEstados
            //                             on new { profe = (int?)profesional.id , estable = Boolean.TrueString } equals new { profe = ae.idProfesionalDestino  , estable = ae.Evoluciones.Select(x => x.idSector).Contains(1).ToString() }
            //                             into pGroup
            //                             where pGroup.Count() > 10 
            //                             select new Semanal
            //                             {
            //                                 idProfesional = profesional.id,
            //                                 Apellido = profesional.primerApellido,
            //                                 Nombre =  profesional.primerNombre,
            //                                 cantidadAtenciones = pGroup.Count()

            //                             };

            //foreach (Semanal semanal in AtencionesXProfesional)
            //{
            //    Console.WriteLine("El profesional {0} tiene {1} Atenciones", semanal.NomyAp(), semanal.cantidadAtenciones);
            //    semanal.cantidadEvoluciones = ObtenerEvolucionesPorProfesionales(semanal.idProfesional, fechaDesde, fechaHasta, entities);
            //}
            int idEstableciemiento = (int)Session["Establecimiento"];
            var atencionesXProfesional = entities.SP_OBTENER_SEMANAL_ATENCIONES("12", "2019", idSector, idEstableciemiento);
            var evolucionesXProfesional = entities.SP_OBTENER_SEMANAL_EVOLUCIONES("12", "2019", idSector, idEstableciemiento);
            List<Semanal> semanales = CastearASemanales(atencionesXProfesional.ToList(), evolucionesXProfesional.ToList());

            return View("Semanal", semanales.Take(10));
        }

        private List<Semanal> CastearASemanales(List<SP_OBTENER_SEMANAL_ATENCIONES_Result> listaAtenciones, List<SP_OBTENER_SEMANAL_EVOLUCIONES_Result> listaEvoluciones)
        {
            List<Semanal> listaSemanal = new List<Semanal>();
            foreach (var atencion in listaAtenciones)
            {
                Semanal semanal = new Semanal();
                semanal.Apellido = atencion.primerApellido;
                semanal.Nombre = atencion.primerNombre;
                semanal.idProfesional = (int)atencion.idprof;
                semanal.cantidadAtenciones = (int)atencion.cantAtenciones;
                semanal.cantidadEvoluciones = 0;
                if (listaEvoluciones.Exists(x => x.idProfesional == semanal.idProfesional))
                {
                    var evolucion = listaEvoluciones.FirstOrDefault(x => x.idProfesional == semanal.idProfesional);
                    semanal.cantidadEvoluciones = (int)evolucion.cantidad;

                }
                listaSemanal.Add(semanal);
            }
            return listaSemanal;
                
        }

        private int ObtenerEvolucionesPorProfesionales(int idProfesional, DateTime fechaDesde, DateTime fechaHasta, Klinicos_BEntities entities)
        {
            int cantidad;
            cantidad = entities.Evoluciones.Where(x => x.idProfesional == idProfesional).Count();
            return cantidad;
        }




    }
}

