using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace Flappymallku
{
    public class ScoreRecord
    {
        public string Nickname { get; set; }
        public int Puntaje { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class DatabaseManager
    {
   private string connectionString = @"Server=DESKTOP-E5MPDNC;Database=FlappyMallkuDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";

        // Guarda el score en la base de datos
        public void GuardarScore(string nickname, int puntaje)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Scores (Nickname, Puntaje) VALUES (@nickname, @puntaje)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nickname", nickname);
                    cmd.Parameters.AddWithValue("@puntaje", puntaje);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Lee todos los scores (los últimos 10 para no saturar)
        public List<ScoreRecord> LeerScores()
{
    var listaScores = new List<ScoreRecord>();

    try
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string sql = "SELECT TOP 10 Nickname, Puntaje FROM Scores ORDER BY Puntaje DESC";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaScores.Add(new ScoreRecord()
                        {
                            Nickname = reader.GetString(0),
                            Puntaje = reader.GetInt32(1),
                            
                            Fecha = DateTime.MinValue 
                        });
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error en LeerScores: " + ex.Message);
    }

    return listaScores;
}

    }
}
