using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabWeb.models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LabWeb.Service
{
    public class MessageBoardService
    {
        private readonly SqlConnection conn;

        public MessageBoardService(SqlConnection connection)
        {
            conn = connection;
        }

        public IEnumerable<MessageBoard> GetAllData()
        {
            string sql = $@"SELECT * FROM MessageBoard WHERE is_delete = 0;";
            var DataList = new List<MessageBoard>();

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
                    MessageBoard Data = new MessageBoard();
                    Data.messageboard_id = (Guid)dr["messageboard_id"];
                    Data.content = dr["content"].ToString();
                    var filename = dr["messageboard_image"].ToString();
                    var hosturl = "http://localhost:5229/";
                    Data.messageboard_image = hosturl+$"Image/{filename}";
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

        public void InsertMessageBoard(MessageBoard newData)
        {
            string sql = $@"INSERT INTO MessageBoard
                            (messageboard_id,content,messageboard_image,create_time,create_id,
                            update_time,update_id,is_delete) 
                            VALUES
                            (@messageboard_id, @content, @messageboard_image, 
                            @create_time, @create_id, @update_time, @update_id,0);";
            
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);

                newData.messageboard_id = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@messageboard_id", newData.messageboard_id);
                cmd.Parameters.AddWithValue("@content", newData.content);
                cmd.Parameters.AddWithValue("@messageboard_image", newData.messageboard_image);
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

        public MessageBoard GetDataById(Guid Id)
        {
            string sql = $@"SELECT m.* FROM MessageBoard m
                            INNER JOIN Members d ON m.create_id = d.members_id 
                            WHERE m.messageboard_id = @Id AND m.is_delete=0;";
            
            MessageBoard Data = new MessageBoard();

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
                Data.messageboard_id = (Guid)dr["messageboard_id"];
                Data.content = dr["content"].ToString();
                var filename = dr["messageboard_image"].ToString();
                var hosturl = "http://localhost:5229/";
                Data.messageboard_image = hosturl+$"Image/{filename}";

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

        public void UpdateMessageBoard(MessageBoard updateData)
        {
            string sql = $@"UPDATE MessageBoard 
                            SET 
                            content = @content,messageboard_image = @messageboard_image,
                            update_time = @update_time,update_id = @update_id 
                            WHERE 
                            messageboard_id = @Id;";
            try
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("@Id", updateData.messageboard_id);
                cmd.Parameters.AddWithValue("@content", updateData.content);
                cmd.Parameters.AddWithValue("@messageboard_image", updateData.messageboard_image);
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

        public void SoftDeleteMessageBoardById(Guid id)
        {
            string sql = $@"UPDATE MessageBoard SET is_delete = 1 WHERE messageboard_id = @Id;";

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