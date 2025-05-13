using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using AppMexaba.Models;
using System.Data;

public class LoginController : Controller
{
    private readonly IConfiguration _config;

    public LoginController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(Usuario model)
    {
        // Validación de campos vacíos
        if (string.IsNullOrWhiteSpace(model.Nombre) || string.IsNullOrWhiteSpace(model.Pwd))
        {
            ViewBag.Error = "Debes ingresar usuario y contraseña.";
            return View();
        }

        string connectionString = _config.GetConnectionString("ConexionSQL");

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            // Validación insensible a mayúsculas en usuario
            string query = @"
                SELECT nombre, pwd, nombre_lar, sucursal, perfil 
                FROM tcausrapp 
                WHERE UPPER(nombre) = @usuario AND pwd = @pwd";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@usuario", model.Nombre.Trim().ToUpper());
            cmd.Parameters.AddWithValue("@pwd", model.Pwd.Trim());

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                HttpContext.Session.SetString("Nombre", rdr["nombre_lar"].ToString());
                HttpContext.Session.SetString("Sucursal", rdr["sucursal"].ToString());
                HttpContext.Session.SetInt32("Perfil", Convert.ToInt32(rdr["perfil"]));

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                return View();
            }
        }
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
