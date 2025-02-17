using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaDeWebApi.Models;
using System.Diagnostics.Metrics;

namespace PracticaDeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : Controller
    {
        private readonly bibliotecaContext _bibliotecaContexto;
        public LibroController(bibliotecaContext bibliotecaContext)
        {
            _bibliotecaContexto = bibliotecaContext;
        }

        [HttpGet]
        [Route("GetAll")]

        // Obtener todos los libros.
        public IActionResult Get()
        {
            // Consulta Get normal
            List<Libro> listadoLibro = (from e in _bibliotecaContexto.Libro
                                        select e).ToList();

            // Agregando paginación
            List<Libro> listadoPaginacion = (from e in _bibliotecaContexto.Libro
                                             select e).Skip(10).Take(10).ToList();

            if (listadoLibro.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoLibro);
        }

        // Obtener un libro por su Id, incluyendo el nombre del autor.
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            var libro = (from l in _bibliotecaContexto.Libro
                         join Autor a in _bibliotecaContexto.Autor
                             on l.id_autor equals a.id_autor
                        where l.id_libro == id
                         select new
                         {
                             l.titulo,
                             l.anioPublicacion,
                             autor = a.nombre,
                             l.id_categoria,
                             l.resumen
                         }).ToList();

            if (libro == null)
            {
                return NotFound();
            }

            return Ok(libro);
        }

        // Crear libro
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarLibro([FromBody] Libro libro)
        {
            try
            {
                _bibliotecaContexto.Libro.Add(libro);
                _bibliotecaContexto.SaveChanges();
                return Ok(libro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // actualizar libro
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarLibro(int id, [FromBody] Libro libroModificar)
        {
            Libro? libroActual = (from e in _bibliotecaContexto.Libro
                                  where e.id_libro == id
                                  select e).FirstOrDefault();
            if (libroActual == null)
            {
                return NotFound();
            }
            // Título, AñoPublicación, AutorId, CategoriaId, Resumen
            libroActual.titulo = libroModificar.titulo;
            libroActual.anioPublicacion = libroModificar.anioPublicacion;
            libroActual.id_autor = libroModificar.id_autor;
            libroActual.id_categoria = libroModificar.id_categoria;
            libroActual.resumen = libroModificar.resumen;

            _bibliotecaContexto.Entry(libroActual).State = EntityState.Modified;
            _bibliotecaContexto.SaveChanges();

            return Ok(libroActual);
        }

        // eliminar un libro.
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarLibro(int id)
        {
            Libro? libro = (from e in _bibliotecaContexto.Libro
                            where e.id_libro == id
                            select e).FirstOrDefault();

            if (libro == null) { return NotFound(); }

            _bibliotecaContexto.Libro.Attach(libro);
            _bibliotecaContexto.Libro.Remove(libro);
            _bibliotecaContexto.SaveChanges();

            return Ok(libro);

        }

        // Implementa una consulta para obtener todos los libros publicados después del año 2000.
        [HttpGet]
        [Route("GetAnioPublicado")]
        public IActionResult GetAnioPublicado()
        {
            DateTime fechaComparacion = new DateTime(2000, 1, 1);

            List<Libro> listaLibro = (from l in _bibliotecaContexto.Libro
                                      where l.anioPublicacion > fechaComparacion select l).ToList();

            if (!listaLibro.Any())
            {
                return NotFound();
            }
            return Ok(listaLibro);
        }

        // Implementa una consulta para contar cuántos libros ha escrito un autor específico.
        [HttpGet]
        [Route("GetCantidadLibrosEscritos/{id}")]
        public IActionResult GetCantLibrosEscritos(int id)
        {
            var liborPorAutor = (from l in _bibliotecaContexto.Libro
                                 join a in _bibliotecaContexto.Autor
                                    on l.id_autor equals a.id_autor
                                    where l.id_autor == id
                                    group a by a.nombre into libro
                                 select new
                                 {
                                     NombreAutor = libro.Key,
                                     CantidadLibros = libro.Count()
                                 }).ToList();

            if(!liborPorAutor.Any())
            {
                return NotFound();
            }

            return Ok(liborPorAutor);
        }

        // Permite buscar libros por título.
        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByTitulo(string filtro)
        {
            Libro? libro = (from l in _bibliotecaContexto.Libro
                            where l.titulo.Contains(filtro)
                            select l).FirstOrDefault();

            if (libro == null)
            {
                return NotFound();
            }   

            return Ok(libro);
        }

    }
}
