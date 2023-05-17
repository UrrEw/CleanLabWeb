using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;

namespace LabWeb.Service
{
    public class ActivityService
    {
        private readonly SqlConnection conn;

        public ActivityService(SqlConnection connection)
        {
            conn = connection;
        }
        
        public IEnumerable<Activity> GetAllData()
        {
            string sql = $@"SELECT * FROM Activity WHERE is_delete = 0;";
            var DataList = new List<Activity>();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    Activity Data = new Activity();
                    Data.activity_id = (Guid)dr["activity_id"];
                    Data.activity_title = dr["activity_title"].ToString();
                    Data.activity_content = dr["activity_content"].ToString();
                    var filename = dr["first_image"].ToString();
                    var hosturl = "http://localhost:5229/";
                    Data.first_image = hosturl+$"Image/{filename}";
                    if (dr["images"] != DBNull.Value)
                    {
                        var imagesStr = dr["images"].ToString();
                        if (!string.IsNullOrEmpty(imagesStr))
                        {
                            Data.images = imagesStr.Split(',').ToList();
                        }
                        else
                        {
                            Data.images = null;
                        }
                    }
                    else
                    {
                        Data.images = null;
                    }
                    
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

        public void InsertActivity(Activity newData)
        {
            string sql =$@"INSERT INTO Activity
                        (activity_id,activity_title,activity_content,
                        create_time,create_id,update_time,update_id,is_delete,first_image) 
                        VALUES
                        (@activity_id, @activity_title, @activity_content, 
                        @create_time, @create_id, @update_time, @update_id,0,@first_image);";
            
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.activity_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@activity_id", newData.activity_id);
                cmd.Parameters.AddWithValue("@activity_title", newData.activity_title);
                cmd.Parameters.AddWithValue("@activity_content", newData.activity_content);
                cmd.Parameters.AddWithValue("@create_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@create_id", newData.create_id);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", newData.update_id);
                cmd.Parameters.AddWithValue("@first_image", newData.first_image);
                //string imagesString = string.Join(",", newData.images);
                //cmd.Parameters.AddWithValue("@images", imagesString);

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

        public Activity GetDataById(Guid Id)
        {
            string sql = $@"SELECT * FROM Activity WHERE activity_id = @Id AND is_delete=0;";
            Activity? Data = new Activity();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.activity_id = (Guid)dr["activity_id"];
                Data.activity_title = dr["activity_title"].ToString();
                Data.activity_content = dr["activity_content"].ToString();
                var filename = dr["first_image"].ToString();
                var hosturl = "http://localhost:5229/";
                Data.first_image = hosturl+$"Image/{filename}";
                
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

        public void UpdateActivity(Activity updateData)
        {
            string sql = $@"UPDATE Activity 
                            SET 
                            activity_title = @activity_title,activity_content = @activity_content,
                            update_time = @update_time,update_id = @update_id, first_image = @first_image 
                            WHERE 
                            activity_id = @Id;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.activity_id);
                cmd.Parameters.AddWithValue("@activity_title", updateData.activity_title);
                cmd.Parameters.AddWithValue("@activity_content", updateData.activity_content);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", updateData.update_id);
                cmd.Parameters.AddWithValue("@first_image", updateData.first_image);
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

        public void SoftDeleteActivityById(Guid id)
        {
            string sql = $@"UPDATE Activity SET is_delete = 1 WHERE activity_id = @Id;";

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