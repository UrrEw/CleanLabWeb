using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LabWeb.Service
{
    public class ProfessorService
    {
        private readonly SqlConnection conn;

        public ProfessorService(SqlConnection connection)
        {
            conn = connection;
        }

        public IEnumerable<Professor> GetAllData()
        {
            string sql = $@"SELECT * FROM Professor;";
            var DataList = new List<Professor>();

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
                    Professor Data = new Professor();
                    Data.professor_id = (Guid)dr["professor_id"];
                    Data.professor_name = dr["professor_name"].ToString();
                    Data.professor_position = dr["professor_position"].ToString();
                    Data.professor_school = dr["professor_school"].ToString();
                    Data.professor_study = dr["professor_study"].ToString();
                    Data.professor_major = dr["professor_major"].ToString();
                    Data.professor_email = dr["professor_email"].ToString();
                    Data.professor_tel = dr["professor_tel"].ToString();
                    Data.professor_office = dr["professor_office"].ToString();
                    var filename = dr["professor_image"].ToString();
                    var hosturl = "http://localhost:5229/";
                    Data.professor_image = hosturl+$"Image/{filename}";
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

        public void InsertProfessor(Professor newData)
        {
            string sql = $@"INSERT INTO Professor
                            (professor_id,professor_name,professor_position,professor_school,professor_study,
                            professor_major,professor_email,professor_tel,professor_office,professor_image) 
                            VALUES
                            (@professor_id, @professor_name, @professor_position, @professor_school,@professor_study,
                            @professor_major, @professor_email, @professor_tel, @professor_office, @professor_image);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.professor_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@professor_id", newData.professor_id);
                cmd.Parameters.AddWithValue("@professor_name", newData.professor_name);
                cmd.Parameters.AddWithValue("@professor_position", newData.professor_position);
                cmd.Parameters.AddWithValue("@professor_school", newData.professor_school);
                cmd.Parameters.AddWithValue("@professor_study", newData.professor_study);
                cmd.Parameters.AddWithValue("@professor_major", newData.professor_major);
                cmd.Parameters.AddWithValue("@professor_email", newData.professor_email);
                cmd.Parameters.AddWithValue("@professor_tel", newData.professor_tel);
                cmd.Parameters.AddWithValue("@professor_office", newData.professor_office);
                cmd.Parameters.AddWithValue("@professor_image", newData.professor_image);

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

        public Professor GetDataById(Guid Id)
        {
            string sql = $@"SELECT * FROM Professor WHERE professor_id = @Id;";
            Professor? Data = new Professor();

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
                Data.professor_id = (Guid)dr["professor_id"];
                Data.professor_name = dr["professor_name"].ToString();
                Data.professor_position = dr["professor_position"].ToString();
                Data.professor_school = dr["professor_school"].ToString();
                Data.professor_study = dr["professor_study"].ToString();
                Data.professor_major = dr["professor_major"].ToString();
                Data.professor_email = dr["professor_email"].ToString();
                Data.professor_tel = dr["professor_tel"].ToString();
                Data.professor_office = dr["professor_office"].ToString();
                var filename = dr["professor_image"].ToString();
                var hosturl = "http://localhost:5229/";
                Data.professor_image = hosturl+$"Image/{filename}";
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

        public void UpdateProfessor(Professor updateData)
        {
            string sql = $@"UPDATE Professor 
                            SET 
                            professor_name = @professor_name,professor_position = @professor_position,professor_school = @professor_school,
                            professor_study = @professor_study,professor_major = @professor_major,professor_email = @professor_email,
                            professor_tel = @professor_tel,professor_office = @professor_office,professor_image = @professor_image
                            WHERE 
                            professor_id = @Id;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.professor_id);
                cmd.Parameters.AddWithValue("@professor_name", updateData.professor_name);
                cmd.Parameters.AddWithValue("@professor_position", updateData.professor_position);
                cmd.Parameters.AddWithValue("@professor_school", updateData.professor_school);
                cmd.Parameters.AddWithValue("@professor_study", updateData.professor_study);
                cmd.Parameters.AddWithValue("@professor_major", updateData.professor_major);
                cmd.Parameters.AddWithValue("@professor_email", updateData.professor_email);
                cmd.Parameters.AddWithValue("@professor_tel", updateData.professor_tel);
                cmd.Parameters.AddWithValue("@professor_office", updateData.professor_office);
                cmd.Parameters.AddWithValue("@professor_image", updateData.professor_image);
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

        public void DeleteProfessorById(Guid id)
        {
            string sql = $@"DELETE FROM Professor WHERE professor_id = @Id;";

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