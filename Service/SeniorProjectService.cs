using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using LabWeb.models;
using Microsoft.Data.SqlClient;

namespace LabWeb.Service
{
    public class SeniorProjectService
    {
        private readonly SqlConnection conn;

        public SeniorProjectService(SqlConnection connection)
        {
            conn = connection;
        }

        public IEnumerable<SeniorProject> GetAllData()
        {
            string sql = $@"SELECT * FROM SeniorProject WHERE is_delete = 0;";
            var DataList = new List<SeniorProject>();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    SeniorProject Data = new SeniorProject();
                    Data.seniorproject_id = (Guid)dr["seniorproject_id"];
                    Data.senior_title = dr["senior_title"].ToString();
                    var filename = dr["senior_image"].ToString();
                    Data.senior_image = $"Image/{filename}";
                    Data.senior_content = dr["senior_content"].ToString();
                    Data.senior_year = Convert.ToInt32(dr["senior_year"]);
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

        public void InsertSeniorProject(SeniorProject newData)
        {
            string sql = $@"INSERT INTO SeniorProject
                            (seniorproject_id,senior_title,senior_content,senior_image,senior_year,
                            create_time,create_id,update_time,update_id,is_delete) 
                            VALUES
                            (@seniorproject_id, @senior_title, @senior_content,@senior_image, 
                            @senior_year, @create_time, @create_id, @update_time, @update_id, 0);";
            
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.seniorproject_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@seniorproject_id", newData.seniorproject_id);
                cmd.Parameters.AddWithValue("@senior_title", newData.senior_title);
                cmd.Parameters.AddWithValue("@senior_content", newData.senior_content);
                cmd.Parameters.AddWithValue("@senior_image", newData.senior_image);
                cmd.Parameters.AddWithValue("@senior_year", newData.senior_year);
                cmd.Parameters.AddWithValue("@create_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@create_id", newData.create_id);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", newData.update_id);

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

        public SeniorProject GetDataById(Guid Id)
        {
            string sql = $@"SELECT * FROM SeniorProject 
                            WHERE seniorproject_id = @Id AND is_delete=0;";

            SeniorProject Data = new SeniorProject();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.seniorproject_id = (Guid)dr["seniorproject_id"];
                Data.senior_title = dr["senior_title"].ToString();
                var filename = dr["senior_image"].ToString();
                Data.senior_image = $"Image/{filename}";
                Data.senior_content = dr["senior_content"].ToString();
                Data.senior_year = Convert.ToInt32(dr["senior_year"]);

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

        public void UpdateSeniorProject(SeniorProject updateData)
        {
            string sql = $@"UPDATE SeniorProject 
                            SET 
                            senior_title = @senior_title,senior_image = @senior_image,senior_content = @senior_content, 
                            senior_year = @senior_year,update_time = @update_time,update_id = @update_id 
                            WHERE 
                            seniorproject_id = @Id;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.seniorproject_id);
                cmd.Parameters.AddWithValue("@senior_title", updateData.senior_title);
                cmd.Parameters.AddWithValue("@senior_image", updateData.senior_image);
                cmd.Parameters.AddWithValue("@senior_content", updateData.senior_content);
                cmd.Parameters.AddWithValue("@senior_year", updateData.senior_year);
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

        public void SoftDeleteSeniorProjectById(Guid id)
        {
            string sql = $@"UPDATE SeniorProject SET is_delete = 1 WHERE seniorproject_id = @Id;";

            try
            {
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