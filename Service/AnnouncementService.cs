using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;

namespace LabWeb.Service
{
    public class AnnouncementService
    {
        private readonly SqlConnection conn;

        public AnnouncementService(SqlConnection connection)
        {
            conn = connection;
        }
        
        public IEnumerable<Announcement> GetAllData()
        {
            string sql = $@"SELECT * FROM Announcement WHERE is_delete = 0;";
            var DataList = new List<Announcement>();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    Announcement Data = new Announcement();
                    Data.announce_id = (Guid)dr["announce_id"];
                    Data.announce_title = dr["announce_title"].ToString();
                    Data.announce_content = dr["announce_content"].ToString();
                    Data.create_time = DateTime.Parse(dr["create_time"].ToString());
                    Data.update_time = DateTime.Parse(dr["update_time"].ToString());
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

        public void InsertAnnouncement(Announcement newData)
        {
            string sql =$@"INSERT INTO Announcement
                        (announce_id,announce_title,announce_content,
                        create_time,create_id,update_time,update_id,is_delete) 
                        VALUES
                        (@announce_id, @announce_title, @announce_content, 
                        @create_time, @create_id, @update_time, @update_id,0);";
            
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.announce_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@announce_id", newData.announce_id);
                cmd.Parameters.AddWithValue("@announce_title", newData.announce_title);
                cmd.Parameters.AddWithValue("@announce_content", newData.announce_content);
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

        public Announcement GetDataById(Guid Id)
        {
            string sql = $@"SELECT * FROM Announcement WHERE announce_id = @Id AND is_delete=0;";
            Announcement? Data = new Announcement();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.announce_id = (Guid)dr["announce_id"];
                Data.announce_title = dr["announce_title"].ToString();
                Data.announce_content = dr["announce_content"].ToString();
                Data.create_time = DateTime.Parse(dr["create_time"].ToString());
                Data.update_time = DateTime.Parse(dr["update_time"].ToString());
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

        public void UpdateAnnouncement(Announcement updateData)
        {
            string sql = $@"UPDATE Announcement 
                            SET 
                            announce_title = @announce_title,announce_content = @announce_content,
                            update_time = @update_time,update_id = @update_id 
                            WHERE 
                            announce_id = @Id;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.announce_id);
                cmd.Parameters.AddWithValue("@announce_title", updateData.announce_title);
                cmd.Parameters.AddWithValue("@announce_content", updateData.announce_content);
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

        public void SoftDeleteAnnouncementById(Guid id)
        {
            string sql = $@"UPDATE Announcement SET is_delete = 1 WHERE announce_id = @Id;";

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