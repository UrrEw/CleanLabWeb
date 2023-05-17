using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;

namespace LabWeb.Service
{
    public class ReplyService
    {
        private readonly SqlConnection conn;

        public ReplyService(SqlConnection connection)
        {
            conn = connection;
        }

        public IEnumerable<Reply> GetAllData()
        {
            string sql = $@"SELECT * FROM Reply WHERE is_delete = 0;";
            var DataList = new List<Reply>();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while(dr.Read())
                {
                    Reply Data = new Reply();
                    Data.reply_id = (Guid)dr["reply_id"];
                    Data.reply_content = dr["reply_content"].ToString();
                    var filename = dr["reply_image"].ToString();
                    var hosturl = "http://localhost:5229/";
                Data.reply_image = hosturl+$"Image/{filename}";
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

        public void InsertReply(Reply newData)
        {
            string sql = $@"INSERT INTO Reply
                            (reply_id,reply_content,reply_image,create_time,create_id,
                            update_time,update_id,messageboard_id,is_delete) 
                            VALUES
                            (@reply_id, @reply_content, @reply_image, 
                            @create_time, @create_id, @update_time, @update_id,@messageboard_id,0);";
            
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.reply_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@reply_id", newData.reply_id);
                cmd.Parameters.AddWithValue("@reply_content", newData.reply_content);
                cmd.Parameters.AddWithValue("@reply_image", newData.reply_image);
                cmd.Parameters.AddWithValue("@create_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@create_id", newData.create_id);
                cmd.Parameters.AddWithValue("@update_time", DateTime.Now);
                cmd.Parameters.AddWithValue("@update_id", newData.update_id);
                cmd.Parameters.AddWithValue("@messageboard_id", newData.messageboard_id);

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

        public Reply GetDataById(Guid Id)
        {
            string sql = $@"SELECT m.*,r.* FROM Reply m
                            INNER JOIN MessageBoard r ON m.messageboard_id = r.messageboard_id
                            INNER JOIN Members d ON m.create_id = d.members_id 
                            WHERE m.reply_id = @Id AND m.is_delete=0;";

            Reply Data = new Reply();

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                SqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                Data.reply_id = (Guid)dr["reply_id"];
                Data.reply_content = dr["reply_content"].ToString();
                var filename = dr["reply_image"].ToString();
                var hosturl = "http://localhost:5229/";
                Data.reply_image = hosturl+$"Image/{filename}";
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

        public void UpdateReply(Reply updateData)
        {
            string sql = $@"UPDATE Reply 
                            SET 
                            reply_content = @reply_content,reply_image = @reply_image,
                            update_time = @update_time,update_id = @update_id 
                            WHERE 
                            reply_id = @Id;";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.reply_id);
                cmd.Parameters.AddWithValue("@reply_content", updateData.reply_content);
                cmd.Parameters.AddWithValue("@reply_image", updateData.reply_image);
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

        public void SoftDeleteReplyById(Guid id)
        {
            string sql = $@"UPDATE Reply SET is_delete = 1 WHERE reply_id = @Id;";

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