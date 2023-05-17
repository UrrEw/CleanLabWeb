using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using LabWeb.models;
using Microsoft.Data.SqlClient;
using System.Data;



namespace LabWeb.Service
{
    public class ThesisService
    {
        private readonly SqlConnection conn;

        public ThesisService(SqlConnection connection)
        {
            conn = connection;
        }

        public IEnumerable<Thesis> GetAllData()
        {
            string sql = $@"SELECT * FROM Thesis WHERE is_delete = 0;";
            var DataList = new List<Thesis>();

            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    Thesis Data = new Thesis();
                    Data.thesis_id = (Guid)dr["thesis_id"];
                    Data.thesis_title = dr["thesis_title"].ToString();
                    var filename = dr["thesis_image"].ToString();
                    var hosturl = "http://localhost:5229/";
                    Data.thesis_image = hosturl+$"Image/{filename}";
                    Data.thesis_abstract = dr["thesis_abstract"].ToString();
                    Data.thesis_year = Convert.ToInt32(dr["thesis_year"]);
                    DataList.Add(Data);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }

            return DataList;
        }

        public void InsertThesis(Thesis newData)
        {
            string sql = $@"INSERT INTO Thesis
                            (thesis_id,author_id,thesis_title,thesis_image,thesis_abstract,thesis_year,create_time,
                            create_id,update_time,update_id,is_delete) 
                            VALUES
                            (@thesis_id, @author_id, @thesis_title,@thesis_image, 
                            @thesis_abstract, @thesis_year, @create_time,
                            @create_id, @update_time, @update_id,0);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.thesis_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@thesis_id", newData.thesis_id);
                cmd.Parameters.AddWithValue("@author_id", newData.author_id);
                cmd.Parameters.AddWithValue("@thesis_title", newData.thesis_title);
                cmd.Parameters.AddWithValue("@thesis_image", newData.thesis_image);
                cmd.Parameters.AddWithValue("@thesis_abstract", newData.thesis_abstract);
                cmd.Parameters.AddWithValue("@thesis_year", newData.thesis_year);
                cmd.Parameters.AddWithValue("@create_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@create_id", newData.create_id);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", newData.update_id);

                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                Console.WriteLine($"SQL Error: {e.Message}");
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public Thesis GetDataById(Guid Id)
        {
            string sql = $@"SELECT m.*,d.name FROM Thesis m 
                            INNER JOIN Members d ON m.author_id = d.members_id 
                            WHERE thesis_id = @Id AND is_delete=0;";
            Thesis? Data = new Thesis();

            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.thesis_id = (Guid)dr["thesis_id"];
                Data.thesis_title = dr["thesis_title"].ToString();
                var filename = dr["thesis_image"].ToString();
                var hosturl = "http://localhost:5229/";
                Data.thesis_image = hosturl+$"Image/{filename}";
                Data.thesis_abstract = dr["thesis_abstract"].ToString();
                Data.thesis_year = Convert.ToInt32(dr["thesis_year"]);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Data = null;
            }
            finally
            {
                conn.Close();
            }
            return Data;
        }

        public void UpdateThesis(Thesis updateData)
        {
            string sql = $@"UPDATE Thesis 
                            SET 
                            thesis_title = @thesis_title,thesis_image = @thesis_image,thesis_abstract = @thesis_abstract, 
                            thesis_year = @thesis_year,update_time = @update_time,update_id = @update_id 
                            WHERE 
                            thesis_id = @Id;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.thesis_id);
                cmd.Parameters.AddWithValue("@thesis_title", updateData.thesis_title);
                cmd.Parameters.AddWithValue("@thesis_image", updateData.thesis_image);
                cmd.Parameters.AddWithValue("@thesis_abstract", updateData.thesis_abstract);
                cmd.Parameters.AddWithValue("@thesis_year", updateData.thesis_year);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", updateData.update_id);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

        public void SoftDeleteThesisById(Guid id)
        {
            string sql = $@"UPDATE Thesis SET is_delete = 1 WHERE thesis_id = @Id;";

            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }

    }
}