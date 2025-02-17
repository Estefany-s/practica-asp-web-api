using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaDeWebApi.Models;

namespace PracticaDeWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : Controller
    {
        private readonly bibliotecaContext _bibliotecaContexto;

        public AutorController(bibliotecaContext bibliotecaContext)
        {
            _bibliotecaContexto = bibliotecaContext;
        }

        [HttpGet]
        [Route("GetAll")]

        // Obtener todos los autores.
        public IActionResult Get()
        {
            List<Autor> listadoAutor = (from e in _bibliotecaContexto.Autor
                                        select e).ToList();

            if (listadoAutor.Count() == 0)
            {
                return NotFound();
            }

            return Ok(listadoAutor);
        }

        //Obtener un autor por su Id, incluyendo sus libros.
        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult Get(int id)
        {
            var autor = (from b in _bibliotecaContexto.Autor
                            join Libro l in _bibliotecaContexto.Libro
                                on b.id_autor equals l.id_autor
                         where b.id_autor == id
                              select new
                              {
                                  b.nombre,
                                  b.nacionalidad,
                                  l.titulo
                              }).ToList();

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }

        //Crear autor
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarAutor([FromBody] Autor autor)
        {
            try
            {
                _bibliotecaContexto.Autor.Add(autor);
                _bibliotecaContexto.SaveChanges();
                return Ok(autor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // actualizar autor
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarAutor(int id, [FromBody] Autor autorModificar)
        {
            Autor? autorActual = (from e in _bibliotecaContexto.Autor
                                    where e.id_autor == id
                                    select e).FirstOrDefault();
            if (autorActual == null)
            {
                return NotFound();
            }

            autorActual.nombre = autorModificar.nombre;
            autorActual.nacionalidad = autorModificar.nacionalidad;

            _bibliotecaContexto.Entry(autorActual).State = EntityState.Modified;
            _bibliotecaContexto.SaveChanges();

            return Ok(autorModificar);
        }

        //eliminar un autor.
        // Metodo de eliminar
        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult EliminarAutor(int id)
        {
            Autor? autor = (from e in _bibliotecaContexto.Autor
                              where e.id_autor == id
                              select e).FirstOrDefault();

            if (autor == null) { return NotFound(); }

            _bibliotecaContexto.Autor.Attach(autor);
            _bibliotecaContexto.Autor.Remove(autor);
            _bibliotecaContexto.SaveChanges();

            return Ok(autor);

        }


    }
}
