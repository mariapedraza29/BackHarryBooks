using HarryBooks.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace HarryBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly string cadenaSql;
        public BookController (IConfiguration config)
        {
            cadenaSql = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult listar()
        {
            List<Book> lista = new();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    SqlCommand cmd = new ("sp_listBook", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Book
                            {
                                idBook = Convert.ToInt32(rd["idBook"]),
                                title = rd["title"].ToString(),
                                price = Convert.ToDecimal(rd["price"]),
                                quantity = Convert.ToInt32(rd["quantity"]),
                                imageBook =rd["imageBook"].ToString()
                            });

                        }
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = lista });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = lista });
            }
        }
        [HttpGet]
        [Route("Obtener/{idBook:int}")]
        public IActionResult obtener(int idBook )
        {
            List<Book> lista = new List<Book>();
            Book book = new Book();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listBook", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Book
                            {
                                idBook = Convert.ToInt32(rd["idBook"]),
                                title = rd["title"].ToString(),
                                price = Convert.ToDecimal(rd["price"]),
                                quantity = Convert.ToInt32(rd["quantity"]),
                                imageBook = rd["imageBook"].ToString()

                            });

                        }
                    }
                }
                book = lista.Where(item => item.idBook == idBook).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = book });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = book });
            }
        }
        [HttpPost]
        [Route("registrar")]
        public IActionResult Registrar([FromBody] Book objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_registerBook", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idBook", objeto.idBook);
                    cmd.Parameters.AddWithValue("title", objeto.title);
                    cmd.Parameters.AddWithValue("price", objeto.price);
                    cmd.Parameters.AddWithValue("quantity", objeto.quantity);
                    cmd.Parameters.AddWithValue("imageBook", objeto.imageBook);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El book ha sido registrada" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Book objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_updateBook", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idBook", objeto.idBook == 0 ? DBNull.Value : objeto.idBook);
                    cmd.Parameters.AddWithValue("title", objeto.title is null ? DBNull.Value : objeto.title);
                    cmd.Parameters.AddWithValue("price", objeto.price == 0 ? DBNull.Value : objeto.price);
                    cmd.Parameters.AddWithValue("quantity", objeto.quantity == 0 ? DBNull.Value : objeto.quantity);
                    cmd.Parameters.AddWithValue("imageBook", objeto.imageBook is null  ? DBNull.Value : objeto.imageBook);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El libro ha sido actualizado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("eliminar")]
        public IActionResult Editar(int idBook)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_deleteBook", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idBook", idBook);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "El libro ha sido eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}

