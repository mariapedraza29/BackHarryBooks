using HarryBooks.Modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace HarryBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyController : ControllerBase
    {
        private readonly string cadenaSql;
        public BuyController(IConfiguration config)
        {
            cadenaSql = config.GetConnectionString("CadenaSQL");
        }
        [HttpGet]
        [Route("Listar")]
        public IActionResult listar()
        {
            List<Buy> lista = new List<Buy>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listBuy", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Buy
                            {
                                idBuy = Convert.ToInt32(rd["idBuy"]),
                                date = Convert.ToDateTime(rd["date"]),
                                paimentMethod = rd["paimentMethod"].ToString()

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
        [Route("Obtener/{idBuy:int}")]
        public IActionResult obtener(int idBuy)
        {
            List<Buy> lista = new List<Buy>();
            Buy buy = new Buy();
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listBuy", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Buy
                            {
                                idBuy = Convert.ToInt32(rd["idBuy"]),
                                date = Convert.ToDateTime(rd["date"]),
                                paimentMethod = rd["paimentMethod"].ToString()

                            });

                        }
                    }
                }
                buy = lista.Where(item => item.idBuy == idBuy).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = buy });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = buy });
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
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "La compra    ha sido registrada" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Buy objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_updateBuy", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idBuy", objeto.idBuy == 0 ? DBNull.Value : objeto.idBuy);
                    cmd.Parameters.AddWithValue("date", objeto.date == default ? DBNull.Value : objeto.date);
                    cmd.Parameters.AddWithValue("paimentMethod", objeto.paimentMethod is null ? DBNull.Value : objeto.paimentMethod);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "la compra ha sido actualizado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("eliminar")]
        public IActionResult Editar(int idBuy)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSql))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_deleteBuy", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("idBuy", idBuy);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "La compra ha sido eliminado" });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }

}
